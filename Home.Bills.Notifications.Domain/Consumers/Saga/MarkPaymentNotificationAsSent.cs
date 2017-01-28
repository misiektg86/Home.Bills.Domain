using System;

namespace Home.Bills.Notifications.Domain.Consumers.Saga
{
    public class MarkPaymentNotificationAsSent
    {
        public Guid PaymentId { get; set; }
    }
}