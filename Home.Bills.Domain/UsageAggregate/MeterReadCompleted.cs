using System;

namespace Home.Bills.Domain.UsageAggregate
{
    internal class MeterReadCompleted
    {
        public Guid AddressId { get; set; }
        public Guid MeterReadId { get; set; }
    }
}