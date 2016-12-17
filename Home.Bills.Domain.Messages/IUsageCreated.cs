using System;

namespace Home.Bills.Domain.Messages
{
    public interface IUsageCreated
    {
        string MeterSerialNumber { get; }
        DateTime ReadDateTime { get; }
        Guid AddressId { get; }
        double Value { get; }
    }
}