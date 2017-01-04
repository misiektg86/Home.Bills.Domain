using System;
using System.Collections.Generic;
using Frameworks.Light.Ddd;
using MassTransit;

namespace Home.Bills.Domain.MeterReadAggregate
{
    public class MeterReadFactory : IAggregateFactory<MeterRead, MeterReadFactoryInput, Guid>
    {
        private readonly IBus _messageBus;

        public MeterReadFactory(IBus messageBus)
        {
            _messageBus = messageBus;
        }

        public MeterRead Create(MeterReadFactoryInput input)
        {
            return new MeterRead(input.MeterReadId, input.MeterIds, input.AddressId, input.ReadProcessStartDate);
        }
    }

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