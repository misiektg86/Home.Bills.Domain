using System;
using MediatR;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    public class MeterAdded : INotification
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