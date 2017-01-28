using System;

namespace Home.Bills.Payments.Messages
{
    public interface IPaymentAccepted
    {
        Guid PaymentId { get; }
        Guid AddressId { get; }
    }
}