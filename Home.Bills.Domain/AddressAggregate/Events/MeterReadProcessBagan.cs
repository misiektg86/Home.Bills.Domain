using System;
using System.Collections.Generic;
using Home.Bills.Domain.AddressAggregate.ValueObjects;
using MediatR;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    public class MeterReadProcessBagan : INotification
    {
        public Guid Id { get; set; }

        public Guid AddressId { get; set; }

        public IEnumerable<MeterId> MeterSerialNumbers { get; set; }

        public DateTime ReadProcessStartDate { get; set; }
    }
}