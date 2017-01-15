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

        protected FinishMeterReadProcess() { }

        public Guid AddressId { get; private set; }

        public Guid MeterReadId { get; private set; }
    }
}