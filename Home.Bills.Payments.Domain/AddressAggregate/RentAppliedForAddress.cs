using System;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    internal class RentAppliedForAddress
    {
        public Guid AddressId { get; internal set; }
        public Guid RentId { get; internal set; }
    }
}