using System;
using Home.Bills.Domain.Messages;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    internal class MeterAssigned : IMeterAdded
    {
        public Guid MeterId { get; }
        public Guid AddressId { get; }

        public MeterAssigned(Guid meterId, Guid addressId)
        {
            MeterId = meterId;
            AddressId = addressId;
        }
    }
}