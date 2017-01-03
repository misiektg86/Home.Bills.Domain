using System;

namespace Home.Bills.Domain.MeterAggregate
{
    public class MeterMountedAtAddress
    {
        public Guid MeterId { get; set; }

        public string MeterSerialNumber { get; set; }

        public Guid AddressId { get; set; }
    }
}