using System;

namespace Home.Bills.Domain.MeterAggregate
{
    public class MeterStateUpdated
    {
        public Guid MeterId { get; set; }
        public double State { get; set; }
    }
}