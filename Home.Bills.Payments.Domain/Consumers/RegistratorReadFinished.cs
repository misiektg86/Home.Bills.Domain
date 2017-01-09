using System;

namespace Home.Bills.Payments.Domain.Consumers
{
    public class RegistratorReadFinished
    {
        public Guid MeterReadId { get; set; }
        public Guid AddressId { get; set; }
    }
}