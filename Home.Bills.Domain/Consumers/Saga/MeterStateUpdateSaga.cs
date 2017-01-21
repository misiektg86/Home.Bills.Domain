using System;
using System.Linq;
using Automatonymous;
using Home.Bills.Domain.AddressAggregate.Events;
using Home.Bills.Domain.Messages;
using Home.Bills.Domain.MeterAggregate;
using Home.Bills.Domain.MeterReadAggregate;

namespace Home.Bills.Domain.Consumers
{
    public class MeterStateUpdateSaga : MassTransitStateMachine<MeterStateUpdateSmInstance>
    {
        public Event<MeterReadProcessBagan> MeterReadBegan { get; set; }

        public Event<MeterStateUpdated> MeterstateUpdated { get; set; }

        public Event<IMeterReadProcessCanceled> MeterReadCanceled { get; set; }

        public State Initiated { get; set; }

        public State CollectingMetersStates { get; set; }

        public State CollectedMetersStates { get; set; }

        public MeterStateUpdateSaga()
        {
            InstanceState(instance => instance.CurrentState);

            Event(() => MeterReadBegan,
                configurator =>
                    configurator.CorrelateBy((instance, context) => instance.MeterReadId == context.Message.MeterReadId)
                        .SelectId(context => Guid.NewGuid()));

            Event(() => MeterstateUpdated,
                configurator =>
                    configurator.CorrelateBy(
                        (instance, context) =>
                            instance.MetersToStateUpdate.Contains(context.Message.MeterId)));

            Event(() => MeterReadCanceled, configurator => configurator.CorrelateBy((instance, context) => instance.MeterReadId == context.Message.MeterReadId));

            Initially(When(MeterReadBegan).Then(context =>
            {
                context.Instance.AddressId = context.Data.AddressId;
                context.Instance.MeterReadId = context.Data.MeterReadId;
                context.Instance.MetersToStateUpdate = context.Data.MeterIds.ToList();
            }).TransitionTo(Initiated));

            During(Initiated,
                When(MeterstateUpdated, context => context.Instance.MetersToStateUpdate.Contains(context.Data.MeterId))
                    .Then(context =>
                    {
                        context.Instance.MetersToStateUpdate.Remove(context.Data.MeterId);
                    })
                    .Send((instance, data) => new Uri("rabbitmq://dev-machine:5672/test/Home.Bills"),
                        context =>
                            new CreateUsageCalculation()
                            {
                                AddressId = context.Instance.AddressId,
                                MeterId = context.Data.MeterId,
                                MeterReadId = context.Instance.MeterReadId,
                                MeterState = context.Data.State,
                                UsageId = Guid.NewGuid()
                            })
                    .TransitionTo(CollectingMetersStates));

            WhenEnter(CollectingMetersStates,
                binder =>
                    binder.If(context => !context.Instance.MetersToStateUpdate.Any(),
                        activityBinder =>
                            activityBinder.TransitionTo(CollectedMetersStates).Finalize()));

            During(CollectingMetersStates,
                When(MeterstateUpdated,
                        context =>
                            context.Instance.MetersToStateUpdate.Contains(context.Data.MeterId) &&
                            context.Instance.MetersToStateUpdate.Count > 1)
                    .Then(context => context.Instance.MetersToStateUpdate.Remove(context.Data.MeterId)).Send((instance, data) => new Uri("rabbitmq://dev-machine:5672/test/Home.Bills"),
                        context =>
                            new CreateUsageCalculation()
                            {
                                AddressId = context.Instance.AddressId,
                                MeterId = context.Data.MeterId,
                                MeterReadId = context.Instance.MeterReadId,
                                MeterState = context.Data.State,
                                UsageId = Guid.NewGuid()
                            }),
                When(MeterstateUpdated, context => context.Instance.MetersToStateUpdate.Contains(context.Data.MeterId) &&
                                                   context.Instance.MetersToStateUpdate.Count == 1).Send((instance, data) => new Uri("rabbitmq://dev-machine:5672/test/Home.Bills"),
                        context =>
                            new CreateUsageCalculation()
                            {
                                AddressId = context.Instance.AddressId,
                                MeterId = context.Data.MeterId,
                                MeterReadId = context.Instance.MeterReadId,
                                MeterState = context.Data.State,
                                UsageId = Guid.NewGuid()
                            })
                    .TransitionTo(CollectedMetersStates)
                    .Finalize());

            DuringAny(When(MeterReadCanceled).Finalize());

            SetCompletedWhenFinalized();
        }
    }
}