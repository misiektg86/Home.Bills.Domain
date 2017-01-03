using System;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    internal class MeterRemoved
    {
        public Guid AddressId { get; set; }
        public Guid MeterId { get; set; }
    }
}