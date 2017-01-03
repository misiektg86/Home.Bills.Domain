using System;
using MediatR;

namespace Home.Bills.Domain.MeterAggregate
{
    internal class MeterStateCorrected : INotification
    {
        public Guid? AddressId { get; set; }
        public Guid MeterId { get; set; }
        public string MeterSerialNumber { get; set; }
    }
}