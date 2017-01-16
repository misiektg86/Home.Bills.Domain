using System;
using System.Collections.Generic;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.Events;
using MassTransit;

namespace Home.Bills.Domain.AddressAggregate
{
    public class AddressFactory : AggregateRootFactoryBase<Address, AddressFactoryInput, Guid>
    {
        protected override Address CreateInternal(AddressFactoryInput input)
        {
            return new Address(input.Street, input.City, input.StreetNumber, input.HomeNumber, input.Id, input.SquareMeters);
        }
    }
}