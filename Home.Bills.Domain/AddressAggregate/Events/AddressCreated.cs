using System;
using Home.Bills.Domain.Contracts.Messages;
using MediatR;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    internal class AddressCreated : IAddressCreated
    {
        public Guid Id { get; }

        public AddressCreated(Guid id)
        {
            Id = id;
        }
    }
}