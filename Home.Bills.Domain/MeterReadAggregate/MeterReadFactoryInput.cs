using System;
using System.Collections.Generic;

namespace Home.Bills.Domain.MeterReadAggregate
{
    public class MeterReadFactoryInput
    {
        public Guid AddressId { get; }
        public IEnumerable<Guid> MeterIds { get; }
        public DateTime ReadProcessStartDate { get; }

        public MeterReadFactoryInput(Guid meterReadId, Guid addressId, IEnumerable<Guid> meterIds, DateTime readProcessStartDate)
        {
            MeterReadId = meterReadId;
            this.AddressId = addressId;
            this.MeterIds = meterIds;
            this.ReadProcessStartDate = readProcessStartDate;
        }

        public Guid MeterReadId { get; set; }
    }
}