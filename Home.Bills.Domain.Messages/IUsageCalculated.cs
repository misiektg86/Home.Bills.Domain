using System;

namespace Home.Bills.Domain.Messages
{
    public interface IUsageCalculated
    {
        DateTime ReadDateTime { get; }
        Guid AddressId { get; }
        double Value { get; }
    }
}