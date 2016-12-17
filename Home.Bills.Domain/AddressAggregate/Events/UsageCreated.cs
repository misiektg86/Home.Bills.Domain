using System;
using MediatR;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    public class UsageCreated : IUsageCreated,INotification
    {
        public UsageCreated(double value, string meterSerialNumber, DateTime readDateTime, Guid addressId)
        {
            Value = value;
            MeterSerialNumber = meterSerialNumber;
            ReadDateTime = readDateTime;
            AddressId = addressId;
        }

        public string MeterSerialNumber { get; }

        public DateTime ReadDateTime { get; }

        public Guid AddressId { get; }

        public double Value { get; }
    }
}