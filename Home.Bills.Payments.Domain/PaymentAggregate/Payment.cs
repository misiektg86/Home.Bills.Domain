using System;
using System.Collections.Generic;
using System.Linq;
using Frameworks.Light.Ddd;
using MassTransit;
using Newtonsoft.Json;

namespace Home.Bills.Payments.Domain.PaymentAggregate
{
    public class Payment : AggregateRoot<Guid>
    {
        private List<PaymentItem> _paymentItems;

        public DateTime? Canceled { get; private set; }

        public DateTime? Setteled { get; private set; }

        public DateTime? Accepted { get; private set; }

        public Guid AddressId { get; }

        [JsonIgnore]
        public decimal TotalAmount => _paymentItems.Sum(item => item.Amount);

        [JsonIgnore]
        public IEnumerable<PaymentItem> PaymentItems
        {
            get { return _paymentItems.Select(i=>(PaymentItem)i.Clone()); }
        }

        internal Payment()
        {
            _paymentItems = new List<PaymentItem>();
        }

        internal Payment(Guid paymentId, Guid addressId, IBus messageBus) : base(messageBus)
        {
            _paymentItems = new List<PaymentItem>();
            AddressId = addressId;
            Id = paymentId;
        }

        public void AddPaymentItem(PaymentItem paymentItem)
        {
            if (Canceled.HasValue)
            {
                throw new CanceledPaymentException(Id.ToString());
            }

            if (Accepted.HasValue)
            {
                throw new PaymentAlreadyAcceptedException(Id.ToString());
            }

            _paymentItems.Add(paymentItem);
        }

        public void AcceptPayment()
        {
            if (Canceled.HasValue)
            {
                throw new CanceledPaymentException(Id.ToString());
            }

            if (Accepted.HasValue)
            {
                return;
            }

            Accepted = DateTime.Now;

            Publish(new PaymentAccepted(Id,AddressId));
        }

        public void CancelPayment()
        {
            if (Canceled.HasValue)
            {
                return;
            }

            if (Setteled.HasValue)
            {
                throw new CannotCancelSetteledPaymentException(Id.ToString());
            }

            Canceled = DateTime.Now;

            Publish(new PaymentCanceled(Id, AddressId));
        }

        public void SettlePayment()
        {
            if (Setteled.HasValue)
            {
                return;
            }

            if (Canceled.HasValue)
            {
                throw new CanceledPaymentException(Id.ToString());
            }

            Setteled = DateTime.Now;

            Publish(new PaymentSettled(Id, AddressId));
        }
    }
}