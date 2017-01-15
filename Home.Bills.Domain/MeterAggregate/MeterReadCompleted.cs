using System;

namespace Home.Bills.Domain.MeterAggregate
{
    public class MeterReadCompleted
    {
        public Guid MeterReadId { get; set; }

        public Guid AddressId { get; set; }
    }
}