using System;
using Home.Bills.Domain.Messages;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    public class MeterReadProcessFinished : IMeterReadProcessFinished
    {
        public Guid MeterReadId { get; }
        public Guid AddressId { get; }

        public MeterReadProcessFinished(Guid meterReadId, Guid addressId)
        {
            MeterReadId = meterReadId;
            AddressId = addressId;
        }
    }
}