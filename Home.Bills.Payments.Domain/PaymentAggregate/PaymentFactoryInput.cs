using System;

namespace Home.Bills.Payments.Domain.PaymentAggregate
{
    public class PaymentFactoryInput
    {
        public Guid PaymentId { get; set; }
        public Guid AddressId { get; set; }
    }
}