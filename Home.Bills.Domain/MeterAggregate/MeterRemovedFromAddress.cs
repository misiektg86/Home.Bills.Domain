using System;
using MediatR;

namespace Home.Bills.Domain.MeterAggregate
{
    internal class MeterRemovedFromAddress : INotification
    {
        public Guid AddressId { get; set; }
        public Guid MeterId { get; set; }
        public string MeterSerialNumber { get; set; }
    }
}