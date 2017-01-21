using System;
using System.Linq;
using Automatonymous;
using Home.Bills.Domain.AddressAggregate.Events;
using Home.Bills.Domain.Messages;
using Home.Bills.Domain.MeterReadAggregate;

namespace Home.Bills.Domain.Consumers
{
    public class MeterReadProcessSaga : MassTransitStateMachine<MeterReadSmInstance>
    {
        public Event<MeterReadProcessBagan> MeterReadBegan { get; set; }

        public Event<IUsageCalculated> UsageCalculated { get; set; }

        public Event<IMeterReadProcessCanceled> MeterReadCanceled { get; set; }

        public State Initiated { get; set; }

        public State CollectingUsageCalculations { get; set; }

        public State CollectedUsageCalculations { get; set; }

        public MeterReadProcessSaga()
        {
            InstanceState(instance => instance.CurrentState);

            Event(() => MeterReadBegan,
                configurator =>
                    configurator.CorrelateBy((instance, context) => instance.MeterReadId == context.Message.MeterReadId)
                        .SelectId(context => Guid.NewGuid()));

            Event(() => UsageCalculated,configurator => configurator.CorrelateBy((instance, context) => instance.MeterReadId == context.Message.MeterReadId));

            Event(() => MeterReadCanceled, configurator => configurator.CorrelateBy((instance, context) => instance.MeterReadId == context.Message.MeterReadId));

            Initially(When(MeterReadBegan).Then(context =>
            {
                context.Instance.AddressId = context.Data.AddressId;
                context.Instance.MeterReadId = context.Data.MeterReadId;
                context.Instance.UsagesToCalculate = context.Data.MeterIds.ToList();
            }).TransitionTo(Initiated));

            During(Initiated, When(UsageCalculated, context => context.Instance.UsagesToCalculate.Contains(context.Data.MeterId)).Then(context =>
              {
                  context.Instance.UsagesToCalculate.Remove(context.Data.MeterId);
              }).TransitionTo(CollectingUsageCalculations));

            WhenEnter(CollectingUsageCalculations,
                binder =>
                    binder.If(context => !context.Instance.UsagesToCalculate.Any(),
                        activityBinder =>
                            activityBinder.TransitionTo(CollectedUsageCalculations).Publish(
                                context =>
                                    new FinishMeterReadProcess(context.Instance.MeterReadId,
                                        context.Instance.AddressId)).Finalize()));
            During(CollectingUsageCalculations,
                When(UsageCalculated,
                        context =>
                            context.Instance.UsagesToCalculate.Contains(context.Data.MeterId) &&
                            context.Instance.UsagesToCalculate.Count > 1)
                    .Then(context => context.Instance.UsagesToCalculate.Remove(context.Data.MeterId)),
                When(UsageCalculated, context => context.Instance.UsagesToCalculate.Contains(context.Data.MeterId) &&
                                                 context.Instance.UsagesToCalculate.Count == 1)
                    .TransitionTo(CollectedUsageCalculations)
                    .Publish(context => new FinishMeterReadProcess(context.Instance.MeterReadId,
                        context.Instance.AddressId)).Finalize());

            DuringAny(When(MeterReadCanceled).Finalize());

            SetCompletedWhenFinalized();
        }
    }
}