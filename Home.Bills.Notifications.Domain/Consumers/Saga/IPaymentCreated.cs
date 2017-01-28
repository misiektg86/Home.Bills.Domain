using System;

namespace Home.Bills.Notifications.Domain.Consumers.Saga
{
    public interface IPaymentCreated
    {
        Guid PaymentId { get; }
    }
}