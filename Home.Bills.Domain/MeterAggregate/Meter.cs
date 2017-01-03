using System;
using System.Globalization;
using Frameworks.Light.Ddd;
using MassTransit;

namespace Home.Bills.Domain.MeterAggregate
{
    public class Meter : AggregateRoot<Guid>
    {
        public Guid? AddressId;

        public double State { get; private set; }

        public string SerialNumber { get; }

        internal Meter() { }

        internal Meter(Guid id, Guid? addressId, double state, string serialNumber, IBus messageBus) : base(messageBus)
        {
            AddressId = addressId;
            State = state;
            SerialNumber = serialNumber;
            Id = id;
        }

        public void MountAtAddress(Guid addressId)
        {
            if (AddressId.HasValue)
            {
                throw new MeterAlreadyMountedatAddressException(Id.ToString());
            }

            AddressId = addressId;

            MessageBus.Publish(new MeterMountedAtAddress() { AddressId = AddressId.Value, MeterSerialNumber = SerialNumber, MeterId = Id });
        }

        public void UnmountAtAddress(Guid addressId)
        {
            if (addressId != AddressId)
            {
                throw new CannotUnmountMeterAtAddressSpecifiedException(Id);
            }

            AddressId = null;

            MessageBus.Publish(new MeterUnmountedAtAddress() { AddressId = addressId, MeterId = Id, MeterSerialNumber = SerialNumber });
        }

        public void UpdateState(double state)
        {
            if (state < State)
            {
                throw new MeterStateCannotBeLowerThanCurrentStateException(State.ToString(CultureInfo.InvariantCulture));
            }

            State = state;

            MessageBus.Publish(new MeterStateUpdated() { AddressId = AddressId, MeterId = Id, MeterSerialNumber = SerialNumber });
        }

        public void CorrectState(double state)
        {
            State = state;

            MessageBus.Publish(new MeterStateCorrected() { AddressId = AddressId, MeterId = Id, MeterSerialNumber = SerialNumber });
        }
    }
}