using System;

namespace Home.Bills.Domain.MeterAggregate
{
    internal class MeterStateCorrected
    {
        public Guid? AddressId { get; set; }
        public Guid MeterId { get; set; }
        public string MeterSerialNumber { get; set; }
    }
}