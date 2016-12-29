using System;
using MediatR;

namespace Home.Bills.Payments.Domain.Commands
{
    public class RegisterUsage : INotification, IAsyncNotification
    {
        public string MeterSerialNumber { get; set; }
        public DateTime ReadDateTime { get; set; }
        public Guid AddressId { get; set; }
        public double Value { get; set; }
    }
}