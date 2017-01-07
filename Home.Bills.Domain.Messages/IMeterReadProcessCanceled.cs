using System;

namespace Home.Bills.Domain.Messages
{
    public interface IMeterReadProcessCanceled
    {
        Guid MeterReadId { get; }
        Guid AddressId { get; }
    }
}