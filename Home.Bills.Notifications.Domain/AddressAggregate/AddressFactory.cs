using System;
using Frameworks.Light.Ddd;

namespace Home.Bills.Notifications.Domain.AddressAggregate
{
    public class AddressFactory : AggregateRootFactoryBase<Address, AddressFactoryInput, Guid>
    {
        protected override Address CreateInternal(AddressFactoryInput input)
        {
            return new Address(input.AddressId, input.FullAddressName);
        }
    }
}