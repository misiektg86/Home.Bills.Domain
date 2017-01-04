using System;
using System.Collections.Generic;
using System.Linq;
using Automatonymous;
using Home.Bills.Domain.AddressAggregate.Events;
using Home.Bills.Domain.MeterAggregate;
using MassTransit.Util;

namespace Home.Bills.Domain.Consumers
{
    public class MeterReadProcessSaga : MassTransitStateMachine<MeterReadSmInstance>
    {
        public Event<MeterReadProcessBagan> MeterReadBegan { get; set; }
        
        public Event<MeterStateUpdated> MeterStateUpdated { get; set; }

        public State Initiated { get; set; }

        public State CollectingMeterReads { get; set; }

        public MeterReadProcessSaga()
        {
           InstanceState(instance => instance.CurrentState);

            Event(() => MeterReadBegan,
                configurator =>
                    configurator.CorrelateById(context => context.Message.MeterReadId)
                        .SelectId(context => context.Message.MeterReadId));
            Event(() => MeterStateUpdated,
                configurator =>
                    configurator.CorrelateBy(
                        (instance, context) =>
                            instance.AddressId == context.Message.AddressId &&
                            instance.Meters.Contains(context.Message.MeterId)));

            Initially(When(MeterReadBegan).Then(context =>
            {
                context.Instance.MetersCollected = new List<Guid>();
                context.Instance.AddressId = context.Data.AddressId;
                context.Instance.Meters = context.Data.MeterIds;
            }).TransitionTo(Initiated));

            During(Initiated,When(MeterStateUpdated).Then(context =>
            {
                context.Instance.MetersCollected.Add(context.Data.MeterId);
            }).TransitionTo(CollectingMeterReads));

            During(CollectingMeterReads,When(MeterStateUpdated,context => !context.Instance.MetersCollected.Contains(context.Data.MeterId)).Then(context =>
            {
                context.Instance.MetersCollected.Add(context.Data.MeterId);
            }),When(MeterStateUpdated,context => context.Instance.Meters.eq));
        }
    }

    public class MeterReadSmInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public State CurrentState { get; set; }

        public IEnumerable<Guid> Meters { get; set; }

        public List<Guid> MetersCollected { get; set; }

        public Guid AddressId { get; set; }
    }
}