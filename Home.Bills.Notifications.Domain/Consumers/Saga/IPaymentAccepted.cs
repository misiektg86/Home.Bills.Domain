using System;

namespace Home.Bills.Notifications.Domain.Consumers.Saga
{
    public interface IPaymentAccepted
    {
        Guid PaymentId { get; }

        Guid AddressId { get; }
    }
}