using System;
using Home.Bills.Domain.Messages;
using MediatR;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    internal class MeterAssigned : IMeterAdded, INotification
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