using System;
using Home.Bills.Notifications.Messages;

namespace Home.Bills.Notifications.Domain.Services
{
    public class NotificationMessage : INotification
    {
        public string Message { get; set; }

        public string Subject { get; set; }

        public string ToAddress { get; set; }

        public Guid NotificationId { get; set; }
    }
}