using System;

namespace Home.Bills.Payments.Domain.PaymentAggregate
{
    public class PaymentCanceled
    {
        public Guid PaymentId { get; }
        public Guid AddressId { get; }

        public PaymentCanceled(Guid paymentId, Guid addressId)
        {
            PaymentId = paymentId;
            AddressId = addressId;
        }
    }
}