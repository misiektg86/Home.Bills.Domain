using System;
using Home.Bills.Payments.Messages;

namespace Home.Bills.Payments.Domain.PaymentAggregate
{
    public class PaymentAccepted : IPaymentAccepted
    {
        public Guid PaymentId { get; }
        public Guid AddressId { get; }

        public PaymentAccepted(Guid paymentId, Guid addressId)
        {
            PaymentId = paymentId;
            AddressId = addressId;
        }
    }
}