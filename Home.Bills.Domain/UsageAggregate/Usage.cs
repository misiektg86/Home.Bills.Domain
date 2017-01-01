using System;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.Events;
using MediatR;

namespace Home.Bills.Domain.UsageAggregate
{
    public class Usage : AggregateRoot<Guid>
    {
        public Guid AddressId { get; }

        public string MeterSerialNumber { get; private set; }

        public double Value { get; private set; }

        public DateTime ReadDateTime { get; private set; }

        internal Usage() { }

        private Usage(Guid id, Guid addressId, string meterSerialNumber, double usage, DateTime readDateTime, IMediator mediator)
        {
            Mediator = mediator;
            AddressId = addressId;
            Id = id;
            Value = usage;
            MeterSerialNumber = meterSerialNumber;
            ReadDateTime = readDateTime;
            Mediator.Publish(new UsageCreated(usage, meterSerialNumber, readDateTime, addressId));
        }

        public static Usage Create(string meterSerialNumber, Guid addressId, double previoudRead, double currentRead, DateTime readDateTime, IMediator mediator)
        {
            if (previoudRead > currentRead)
            {
                throw new InvalidOperationException("Previous read cannot be bigger than current read");
            }

            return new Usage(Guid.NewGuid(), addressId, meterSerialNumber, currentRead - previoudRead, readDateTime, mediator);
        }

        internal Usage Clone() => MemberwiseClone() as Usage;
    }
}