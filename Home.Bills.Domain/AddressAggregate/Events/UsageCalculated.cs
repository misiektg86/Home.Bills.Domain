using System;
using Home.Bills.Domain.Messages;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    public class UsageCalculated : IUsageCalculated
    {
        public UsageCalculated(Guid meterId, Guid meterReadId, double value, DateTime readDateTime, Guid addressId, double previousRead, double currentRead)
        {
            Value = value;
            MeterId = meterId;
            MeterReadId = meterReadId;
            ReadDateTime = readDateTime;
            AddressId = addressId;
            PreviousRead = previousRead;
            CurrentRead = currentRead;
        }

        public double PreviousRead { get; }

        public double CurrentRead { get; }

        public DateTime ReadDateTime { get; }

        public Guid AddressId { get; }

        public double Value { get; }

        public Guid MeterId { get; private set; }

        public Guid MeterReadId { get; }
    }
}