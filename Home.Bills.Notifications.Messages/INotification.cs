using System;

namespace Home.Bills.Notifications.Messages
{
    public interface INotification
    {
        Guid NotificationId { get; set; }

        string Message { get; set; }

        string Subject { get; set; }

        string ToAddress { get; set; }
    }
}