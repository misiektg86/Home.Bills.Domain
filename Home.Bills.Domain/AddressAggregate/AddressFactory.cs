using System;
using System.Collections.Generic;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.Entities;
using MassTransit;

namespace Home.Bills.Domain.AddressAggregate
{
    public class AddressFactory : IAggregateFactory<Address, AddressFactoryInput, Guid>
    {
        private readonly IBus _messageBus;

        public AddressFactory(IBus messageBus)
        {
            _messageBus = messageBus;
        }

        public Address Create(AddressFactoryInput input)
        {
            return new Address(input.Street, input.City, input.StreetNumber, input.HomeNumber, new List<Guid>(), input.Id, _messageBus, input.SquareMeters);
        }
    }
}