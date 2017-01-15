using System;

namespace Home.Bills.Payments.Models
{
    public class CreatePayment
    {
        public Guid MeterReadId { get; set; }
        public Guid AddressId { get; set; }
        public Guid PaymentId { get; set; }
    }
}