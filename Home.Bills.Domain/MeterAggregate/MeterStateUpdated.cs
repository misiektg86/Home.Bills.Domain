using System;

namespace Home.Bills.Domain.MeterAggregate
{
    public class MeterStateUpdated
    {
        public Guid? AddressId { get; set; }
        public Guid MeterId { get; set; }
        public string MeterSerialNumber { get; set; }

        public double State { get; set; }
    }
}