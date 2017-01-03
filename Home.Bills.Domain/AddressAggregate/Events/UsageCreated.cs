using System;
using Home.Bills.Domain.Messages;
using MediatR;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    public class UsageCreated : IUsageCreated,INotification
    {
        public UsageCreated(double value,Guid meterId, DateTime readDateTime, Guid addressId)
        {
            Value = value;
            MeterId = meterId;
            ReadDateTime = readDateTime;
            AddressId = addressId;
        }

        public DateTime ReadDateTime { get; }

        public Guid AddressId { get; }

        public double Value { get; }
        public Guid MeterId { get; private set; }
    }
}