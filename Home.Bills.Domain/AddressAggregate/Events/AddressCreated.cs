using System;
using Home.Bills.Domain.Messages;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    internal class AddressCreated : IAddressCreated
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