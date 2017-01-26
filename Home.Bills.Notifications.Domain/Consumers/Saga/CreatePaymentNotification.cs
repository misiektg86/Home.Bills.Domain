using System;

namespace Home.Bills.Notifications.Domain.Consumers.Saga
{
    public class CreatePaymentNotification
    {
        public Guid PaymentId { get; set; }

        public Guid AddressId { get; set; }
    }
}