using System;
using MediatR;

namespace Home.Bills.Domain.MeterAggregate
{
    internal class MeterStateUpdated : INotification
    {
        public Guid? AddressId { get; set; }
        public Guid MeterId { get; set; }
        public string MeterSerialNumber { get; set; }
    }
}