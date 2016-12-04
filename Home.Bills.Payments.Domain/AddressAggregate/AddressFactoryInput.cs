using System;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    public class AddressFactoryInput
    {
        public Guid AddressId { get; set; }

        public double SquareMeters { get; set; }
    }
}