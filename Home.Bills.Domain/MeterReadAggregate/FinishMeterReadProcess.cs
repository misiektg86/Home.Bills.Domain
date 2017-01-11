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

        public FinishMeterReadProcess() { }

        public Guid AddressId { get; private set; }

        public Guid MeterReadId { get; private set; }
    }
}