using System;

namespace Home.Bills.Notifications.Domain.Consumers.Saga
{
    public interface INotificationSent
    {
        Guid PaymentId { get; }
    }
}