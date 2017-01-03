using System;
using MediatR;

namespace Home.Bills.Domain.UsageAggregate
{
    internal class MeterReadCompleted : INotification
    {
        public Guid AddressId { get; set; }
        public Guid MeterReadId { get; set; }
    }
}