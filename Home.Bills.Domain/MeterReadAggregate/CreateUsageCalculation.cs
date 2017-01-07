using System;

namespace Home.Bills.Domain.MeterReadAggregate
{
    public class CreateUsageCalculation
    {
        public Guid MeterId { get; set; }

        public Guid MeterReadId { get; set; }

        public Guid AddressId { get; set; }

        public double MeterState { get; set; }

        public Guid UsageId { get; set; }
    }
}