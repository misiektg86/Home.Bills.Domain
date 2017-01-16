using System;
using Frameworks.Light.Ddd;

namespace Home.Bills.Domain.MeterAggregate
{
    public class MeterFactory : AggregateRootFactoryBase<Meter,MeterFactoryInput,Guid>
    {
        protected override Meter CreateInternal(MeterFactoryInput input)
        {
            return new Meter(id: input.MeterId, addressId: input.AddressId, state: input.State, serialNumber: input.SerialNumber);
        }
    }

    public class MeterFactoryInput
    {
        public string SerialNumber { get; set; }

        public double State { get; set; }

        public Guid? AddressId { get; set; }

        public Guid MeterId { get; set; }
    }
}