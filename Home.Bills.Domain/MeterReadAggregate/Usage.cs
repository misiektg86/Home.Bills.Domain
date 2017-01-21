using System;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.Events;
using MassTransit;

namespace Home.Bills.Domain.MeterReadAggregate
{
    public class Usage : Entity<Guid>
    {
        public Guid AddressId { get; private set; }

        public Guid MeterReadId { get; private set; }

        public Guid MeterId { get; private set; }

        public double PrevioudRead { get; private set; }

        public double CurrentRead { get; private set; }

        public double Value { get; private set; }

        public DateTime ReadDateTime { get; private set; }

        internal Usage() { }

        private Usage(Guid id, Guid addressId, Guid meterReadId, Guid meterId, double previoudRead, double currentRead, DateTime readDateTime)
        {
            AddressId = addressId;
            MeterReadId = meterReadId;
            Id = id;
            MeterId = meterId;
            PrevioudRead = previoudRead;
            CurrentRead = currentRead;
            ReadDateTime = readDateTime;
            CalculateUsage();
        }

        private void CalculateUsage()
        {
            Value = CurrentRead - PrevioudRead;
        }

        public static Usage Create(Guid usageId, Guid meterReadId, Guid meterId, Guid addressId, double previoudRead, double currentRead, DateTime readDateTime)
        {
            if (previoudRead > currentRead)
            {
                throw new InvalidOperationException("Previous read cannot be bigger than current read");
            }

            var usage = new Usage(usageId, addressId, meterReadId, meterId, previoudRead, currentRead, readDateTime);

            return usage;
        }

        internal Usage Clone() => MemberwiseClone() as Usage;
    }
}