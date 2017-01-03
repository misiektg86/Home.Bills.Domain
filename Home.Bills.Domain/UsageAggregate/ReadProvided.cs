using System;

namespace Home.Bills.Domain.UsageAggregate
{
    internal class ReadProvided
    {
        public Guid AddressId { get; set; }
        public Guid MeterId { get; set; }
        public double OldRead { get; set; }
        public double Read { get; set; }
        public DateTime ReadDateTime { get; set; }
    }
}