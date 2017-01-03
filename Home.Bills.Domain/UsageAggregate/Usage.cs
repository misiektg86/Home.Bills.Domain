using System;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.Events;
using MassTransit;

namespace Home.Bills.Domain.UsageAggregate
{
    public class Usage : AggregateRoot<Guid>
    {
        public Guid AddressId { get; }

        public Guid MeterId { get; private set; }

        public double Value { get; }

        public DateTime ReadDateTime { get; private set; }

        internal Usage() { }

        private Usage(Guid id, Guid addressId, Guid meterId, double usage, DateTime readDateTime, IBus messageBus)
        {
            MessageBus = messageBus;
            AddressId = addressId;
            Id = id;
            Value = usage;
            MeterId = meterId;
            ReadDateTime = readDateTime;
        }

        public static Usage Create(Guid usageId, Guid meterId, Guid addressId, double previoudRead, double currentRead, DateTime readDateTime, IBus mesageBus)
        {
            if (previoudRead > currentRead)
            {
                throw new InvalidOperationException("Previous read cannot be bigger than current read");
            }

            var usage = new Usage(usageId, addressId, meterId, currentRead - previoudRead, readDateTime, mesageBus);

            mesageBus.Publish(new UsageCreated(usage.Value, meterId, readDateTime, addressId));

            return usage;
        }

        internal Usage Clone() => MemberwiseClone() as Usage;
    }
}