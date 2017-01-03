using System;
using MediatR;

namespace Home.Bills.Domain.MeterAggregate
{
    public class MeterMountedAtAddress : INotification, IAsyncNotification
    {
        public Guid MeterId { get; set; }

        public string MeterSerialNumber { get; set; }

        public Guid AddressId { get; set; }
    }
}