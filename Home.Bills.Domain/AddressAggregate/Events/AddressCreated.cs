using System;
using MediatR;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    internal class AddressCreated : INotification
    {
        public Guid Id { get; }

        public AddressCreated(Guid id)
        {
            Id = id;
        }
    }
}