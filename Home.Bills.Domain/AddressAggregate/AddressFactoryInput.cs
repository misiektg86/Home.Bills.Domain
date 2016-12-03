using System;

namespace Home.Bills.Domain.AddressAggregate
{
    public class AddressFactoryInput
    {
        public string Street { get; set; }

        public string City { get; set; }

        public string StreetNumber { get; set; }

        public string HomeNumber { get; set; }

        public Guid Id { get; set; }
    }
}