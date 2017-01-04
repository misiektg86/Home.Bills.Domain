using System;

namespace Home.Bills.Domain.Messages
{
    public interface IMeterReadProcessFinished
    {
        Guid MeterReadId { get; }
        Guid AddressId { get; }
    }
}