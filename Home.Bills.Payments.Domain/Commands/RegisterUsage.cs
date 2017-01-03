using System;

namespace Home.Bills.Payments.Domain.Commands
{
    public class RegisterUsage
    {
        public string MeterSerialNumber { get; set; }
        public DateTime ReadDateTime { get; set; }
        public Guid AddressId { get; set; }
        public double Value { get; set; }
    }
}