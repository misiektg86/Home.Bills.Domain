using System;
using Frameworks.Light.Ddd;
using MediatR;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    public class AddressFactory : IAggregateFactory<Address, AddressFactoryInput, Guid>
    {
        private readonly IMediator _mediator;

        public AddressFactory(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Address Create(AddressFactoryInput input)
        {
            return new Address(input.AddressId, input.SquareMeters, _mediator);
        }
    }
}