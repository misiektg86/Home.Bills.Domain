using System;

namespace Home.Bills.Domain.Messages
{
    public interface IUsageCalculated
    {
         double PreviousRead { get; }

         double CurrentRead { get; }

         DateTime ReadDateTime { get; }

         Guid AddressId { get; }

         double Value { get; }

         Guid MeterId { get;}

         Guid MeterReadId { get; }
    }
}