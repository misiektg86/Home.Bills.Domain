using System;
using Frameworks.Light.Ddd;

namespace Home.Bills.Notifications.Domain.AddressAggregate
{
    public class AddressFactory : AggregateRootFactoryBase<Address, Guid, Guid>
    {
        protected override Address CreateInternal(Guid addressId)
        {
            return new Address(addressId);
        }
    }
}