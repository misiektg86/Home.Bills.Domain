using System;
using Frameworks.Light.Ddd;
using MassTransit;

namespace Home.Bills.Payments.Domain.AddressAggregate
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
            return new Address(input.AddressId, input.SquareMeters, _messageBus);
        }
    }
}