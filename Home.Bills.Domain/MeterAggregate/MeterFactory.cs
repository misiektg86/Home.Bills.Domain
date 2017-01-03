using System;
using Frameworks.Light.Ddd;
using MassTransit;

namespace Home.Bills.Domain.MeterAggregate
{
    public class MeterFactory : IAggregateFactory<Meter,MeterFactoryInput,Guid>
    {
        private readonly IBus _messageBus;
        public MeterFactory(IBus messageBus)
        {
            _messageBus = messageBus;
        }

        public Meter Create(MeterFactoryInput input)
        {
            return new Meter(id:input.MeterId,addressId:input.AddressId,state:input.State,serialNumber:input.SerialNumber,messageBus:_messageBus);
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