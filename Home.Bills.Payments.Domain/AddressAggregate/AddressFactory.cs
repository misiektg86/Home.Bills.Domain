using System;
using Frameworks.Light.Ddd;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    public class AddressFactory : AggregateRootFactoryBase<Address, AddressFactoryInput, Guid>
    {
        protected override Address CreateInternal(AddressFactoryInput input)
        {
            return new Address(input.AddressId, input.SquareMeters);
        }
    }
}