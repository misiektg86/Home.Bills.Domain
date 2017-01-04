using System;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.Events;
using MassTransit;

namespace Home.Bills.Domain.MeterReadAggregate
{
    public class Usage : Entity<Guid>
    {
        public Guid AddressId { get; }
        public Guid MeterReadId { get; }

        public Guid MeterId { get; private set; }

        public double PrevioudRead { get; }

        public double CurrentRead { get; }

        public double Value { get; private set; }

        public DateTime ReadDateTime { get; }

        internal Usage() { }

        private Usage(Guid id, Guid addressId, Guid meterReadId, Guid meterId, double previoudRead, double currentRead, DateTime readDateTime, IBus messageBus)
        {
            MessageBus = messageBus;
            AddressId = addressId;
            MeterReadId = meterReadId;
            Id = id;
            MeterId = meterId;
            PrevioudRead = previoudRead;
            CurrentRead = currentRead;
            ReadDateTime = readDateTime;
        }

        public void CalculateUsage()
        {
            Value = CurrentRead - PrevioudRead;

            Publish(new UsageCalculated(MeterId, MeterReadId, Value, ReadDateTime, AddressId, PrevioudRead, CurrentRead));
        }

        public static Usage Create(Guid usageId, Guid meterReadId, Guid meterId, Guid addressId, double previoudRead, double currentRead, DateTime readDateTime, IBus mesageBus)
        {
            if (previoudRead > currentRead)
            {
                throw new InvalidOperationException("Previous read cannot be bigger than current read");
            }

            var usage = new Usage(usageId, addressId, meterReadId, meterId, previoudRead, currentRead, readDateTime, mesageBus);

            return usage;
        }

        internal Usage Clone() => MemberwiseClone() as Usage;
    }
}