using System;
using MediatR;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    internal class MeterRemoved : INotification, IAsyncNotification
    {
        public Guid AddressId { get; set; }
        public Guid MeterId { get; set; }
    }
}