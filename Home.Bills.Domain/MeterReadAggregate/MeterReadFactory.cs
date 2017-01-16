using System;
using Frameworks.Light.Ddd;

namespace Home.Bills.Domain.MeterReadAggregate
{
    public class MeterReadFactory : AggregateRootFactoryBase<MeterRead, MeterReadFactoryInput, Guid>
    {
        protected override MeterRead CreateInternal(MeterReadFactoryInput input)
        {
            return new MeterRead(input.MeterReadId, input.MeterIds, input.AddressId, input.ReadProcessStartDate);
        }
    }
}