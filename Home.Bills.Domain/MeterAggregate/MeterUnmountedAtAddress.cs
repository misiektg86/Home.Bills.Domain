using System;

namespace Home.Bills.Domain.MeterAggregate
{
    internal class MeterUnmountedAtAddress
    {
        public Guid AddressId { get; set; }
        public Guid MeterId { get; set; }
        public string MeterSerialNumber { get; set; }
    }
}