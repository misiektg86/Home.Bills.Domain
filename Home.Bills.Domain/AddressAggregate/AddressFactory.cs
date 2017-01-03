using System;
using System.Collections.Generic;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.Entities;
using Home.Bills.Domain.AddressAggregate.Exceptions;
using Home.Bills.Domain.AddressAggregate.ValueObjects;
using Home.Bills.Domain.UsageAggregate;
using MediatR;

namespace Home.Bills.Domain.AddressAggregate
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
            return new Address(input.Street, input.City, input.StreetNumber, input.HomeNumber, new List<Meter>(), new List<Guid>(), input.Id, _mediator, input.SquareMeters);
        }
    }
}