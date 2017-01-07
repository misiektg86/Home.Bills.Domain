using System;
using Home.Bills.Domain.Messages;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    public class MeterReadProcessCanceled : IMeterReadProcessCanceled
    {
        public Guid MeterReadId { get; }
        public Guid AddressId { get; }

        public MeterReadProcessCanceled(Guid meterReadId, Guid addressId)
        {
            MeterReadId = meterReadId;
            AddressId = addressId;
        }
    }
}