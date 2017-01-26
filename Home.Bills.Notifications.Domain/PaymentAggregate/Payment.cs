using System;
using System.Collections.Generic;
using System.Linq;
using Frameworks.Light.Ddd;
using Newtonsoft.Json;

namespace Home.Bills.Notifications.Domain.PaymentAggregate
{
    public class Payment : AggregateRoot<Guid>
    {
        public DateTime? SentDateTime { get; private set; }

        public bool Sent { get; private set; }

        public Guid AddressId { get; private set; }

        public IEnumerable<PaymentItem> PaymentItems { get; private set; }
        public string FullAddress { get; private set; }

        [JsonIgnore]
        public decimal TotalAmount => PaymentItems?.Sum(i => i.Amount) ?? 0m;

        internal Payment() { }

        internal Payment(Guid paymentId, Guid addressId, IEnumerable<PaymentItem> paymentItems, string fullAddress)
        {
            AddressId = addressId;
            PaymentItems = paymentItems;
            FullAddress = fullAddress;
            Id = paymentId;
        }

        public static Payment Create(Guid paymentId, Guid addressId, IEnumerable<PaymentItem> paymentItems, string fullAddress)
        {
            if (paymentItems == null)
            {
                throw new CannotCreateEmptyPaymentException(paymentId.ToString());
            }

            return new Payment(paymentId, addressId, paymentItems, fullAddress);
        }

        public void MarkAsSent()
        {
            Sent = true;

            SentDateTime = DateTime.Now;
        }
    }
}