using System;
using Frameworks.Light.Ddd;
using MediatR;

namespace Home.Bills.Domain.MeterAggregate
{
    public class MeterFactory : IAggregateFactory<Meter,MeterFactoryInput,Guid>
    {
        private readonly IMediator _mediator;
        public MeterFactory(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Meter Create(MeterFactoryInput input)
        {
            return new Meter(id:input.MeterId,addressId:input.AddressId,state:input.State,serialNumber:input.SerialNumber,mediator:_mediator);
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