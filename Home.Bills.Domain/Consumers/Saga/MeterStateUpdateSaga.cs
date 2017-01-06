using System;
using System.Linq;
using Automatonymous;
using Home.Bills.Domain.AddressAggregate.Events;
using Home.Bills.Domain.MeterAggregate;
using Home.Bills.Domain.MeterReadAggregate;

namespace Home.Bills.Domain.Consumers
{
    public class MeterStateUpdateSaga : MassTransitStateMachine<MeterStateUpdateSmInstance>
    {
        public Event<MeterReadProcessBagan> MeterReadBegan { get; set; }

        public Event<MeterStateUpdated> MeterstateUpdated { get; set; }

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
                            instance.AddressId == context.Message.AddressId &&
                            instance.MetersToStateUpdate.Contains(context.Message.MeterId)));

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
                    .Send((instance, data) => new Uri("loopback://localhost/Home.Bills"),
                        context =>
                            new CreateUsageCalculation()
                            {
                                AddressId = context.Instance.AddressId,
                                MeterId = context.Data.MeterId,
                                MeterReadId = context.Instance.MeterReadId,
                                MeterState = context.Data.State
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
                    .Then(context => context.Instance.MetersToStateUpdate.Remove(context.Data.MeterId)).Send((instance, data) => new Uri("rabbitmq://localhost:5672/Home.Bills"),
                        context =>
                            new CreateUsageCalculation()
                            {
                                AddressId = context.Instance.AddressId,
                                MeterId = context.Data.MeterId,
                                MeterReadId = context.Instance.MeterReadId,
                                MeterState = context.Data.State
                            }),
                When(MeterstateUpdated, context => context.Instance.MetersToStateUpdate.Contains(context.Data.MeterId) &&
                                                   context.Instance.MetersToStateUpdate.Count == 1).Send((instance, data) => new Uri("rabbitmq://localhost:5672/Home.Bills"),
                        context =>
                            new CreateUsageCalculation()
                            {
                                AddressId = context.Instance.AddressId,
                                MeterId = context.Data.MeterId,
                                MeterReadId = context.Instance.MeterReadId,
                                MeterState = context.Data.State
                            })
                    .TransitionTo(CollectedMetersStates)
                    .Finalize());

            SetCompletedWhenFinalized();
        }
    }
}