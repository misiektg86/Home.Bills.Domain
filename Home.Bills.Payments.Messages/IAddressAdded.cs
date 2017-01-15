using System;

namespace Home.Bills.Payments.Messages
{
    public interface IAddressAdded
    {
        Guid Id { get; }
        double SquareMeters { get; }
    }
}