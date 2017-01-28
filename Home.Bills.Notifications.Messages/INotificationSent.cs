using System;

namespace Home.Bills.Notifications.Messages
{
    public interface INotificationSent
    {
        Guid NotificationId { get; }
    }
}