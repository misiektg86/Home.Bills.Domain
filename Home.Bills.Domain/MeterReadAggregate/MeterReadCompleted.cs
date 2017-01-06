using System;

namespace Home.Bills.Domain.MeterReadAggregate
{
    public class FinishMeterReadProcess
    {
        public FinishMeterReadProcess(Guid instanceMeterReadId, Guid instanceAddressId)
        {
            MeterReadId = instanceMeterReadId;
            AddressId = instanceAddressId;
        }

        public Guid AddressId { get;  }
        public Guid MeterReadId { get; }
    }
}