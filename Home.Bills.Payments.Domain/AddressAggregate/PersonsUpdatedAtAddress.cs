using System;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    internal class PersonsUpdatedAtAddress
    {
        public Guid AddressId { get; set; }

        public int Persons { get; set; }
    }
}