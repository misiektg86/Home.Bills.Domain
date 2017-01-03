using System;
using MediatR;

namespace Home.Bills.Domain.MeterAggregate
{
    public class MeterAssignedToAddress : INotification
    {
        public Guid MeterId { get; set; }

        public string MeterSerialNumber { get; set; }

        public Guid AddressId { get; set; }
    }
}