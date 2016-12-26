using System;
using Home.Bills.Domain.Messages;
using MediatR;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    internal class MeterAdded : IMeterAdded, INotification
    {
        public string MeterSerialNumber { get; }
        public Guid AddressId { get; }
        public double State { get; }

        public MeterAdded(string meterSerialNumber, Guid addressId, double state)
        {
            MeterSerialNumber = meterSerialNumber;
            AddressId = addressId;
            State = state;
        }
    }
}