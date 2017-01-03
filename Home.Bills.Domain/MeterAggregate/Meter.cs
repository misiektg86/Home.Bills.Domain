using System;
using System.Globalization;
using Frameworks.Light.Ddd;

namespace Home.Bills.Domain.MeterAggregate
{
    public class Meter :AggregateRoot<Guid>
    {
        private Guid? _addressId;

        private double _state;

        private string _serialNumber;
        internal Meter() { }

        internal Meter(Guid id, Guid? addressId, double state, string serialNumber)
        {
            _addressId = addressId;
            _state = state;
            _serialNumber = serialNumber;
            Id = id;
        }

        public void AssignToAddress(Guid addressId)
        {
            if (_addressId.HasValue)
            {
                throw new MeterAlreadyAssignedToAddressException(Id.ToString());
            }

            _addressId = addressId;

            Mediator.Publish(new MeterAssignedToAddress() {AddressId = _addressId.Value,MeterSerialNumber = _serialNumber, MeterId = Id});
        }

        public void RemoveFromAddress(Guid addressId)
        {
            if (addressId != _addressId)
            {
                throw new CannotRemoveMeterFromAddressSpecifiedException(Id);
            }

            _addressId = null;

            Mediator.Publish(new MeterRemovedFromAddress() {AddressId = addressId,MeterId = Id, MeterSerialNumber = _serialNumber});
        }

        public void UpdateState(double state)
        {
            if (state < _state)
            {
                throw new  MeterStateCannotBeLowerThanCurrentStateException(_state.ToString(CultureInfo.InvariantCulture));
            }

            _state = state;

            Mediator.Publish(new MeterStateUpdated() { AddressId = _addressId, MeterId = Id, MeterSerialNumber = _serialNumber });
        }

        public void CorrectState(double state)
        {
            _state = state;

            Mediator.Publish(new MeterStateCorrected() { AddressId = _addressId, MeterId = Id, MeterSerialNumber = _serialNumber });
        }
    }
}