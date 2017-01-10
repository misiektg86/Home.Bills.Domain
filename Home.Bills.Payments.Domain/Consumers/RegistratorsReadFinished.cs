using System;

namespace Home.Bills.Payments.Domain.Consumers
{
    public class RegistratorsReadFinished
    {
        public Guid MeterReadId { get; set; }
        public Guid AddressId { get; set; }
    }
}