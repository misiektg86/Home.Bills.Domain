using System;

namespace Home.Bills.Notifications.Domain.AddressAggregate
{
    public class RegisteredAcceptedPayment
    {
        public Guid PaymentId { get; set; }
        public Guid AddressId { get; set; }
    }
}