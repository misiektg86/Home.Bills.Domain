using System;
using Home.Bills.Domain.Messages;
using MediatR;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    internal class AddressCreated : IAddressCreated, INotification
    {
        public Guid Id { get; }
        public double SquareMeters { get; }

        public AddressCreated(Guid id, double squareMeters)
        {
            Id = id;
            SquareMeters = squareMeters;
        }
    }
}