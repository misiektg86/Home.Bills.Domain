using System;
using Home.Bills.Domain.Messages;

namespace Home.Bills.Domain.MeterAggregate
{
    public class MeterMountedAtAddress : IMeterMountedAtAddress
    {
        public Guid MeterId { get; set; }

        public string MeterSerialNumber { get; set; }

        public Guid AddressId { get; set; }
    }
}