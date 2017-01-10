using System;

namespace Home.Bills.Payments.Domain.PaymentAggregate
{
    public class PaymentSettled
    {
        public Guid PaymentId { get; }
        public Guid AddressId { get; }

        public PaymentSettled(Guid paymentId, Guid addressId)
        {
            PaymentId = paymentId;
            AddressId = addressId;
        }
    }
}