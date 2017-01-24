using System;

namespace Home.Bills.Notifications.Domain.Consumers.Saga
{
    public class SendPaymentNotification
    {
        public Guid PaymentId { get; set; }
    }
}