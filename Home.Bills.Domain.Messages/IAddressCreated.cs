using System;

namespace Home.Bills.Domain.Messages
{
    public interface IAddressCreated
    {
        Guid Id { get; }

        double SquareMeters { get; }
    }
}