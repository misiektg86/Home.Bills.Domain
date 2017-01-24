using System;

namespace Home.Bills.Notifications.Domain.Consumers.Saga
{
    public interface INotificationCreated
    {
        Guid PaymentId { get; }
    }
}