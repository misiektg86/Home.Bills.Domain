using System;
using System.Collections.Generic;
using Frameworks.Light.Ddd;

namespace Home.Bills.Notifications.Domain.AddressAggregate
{
    public class Address : AggregateRoot<Guid>
    {
        internal Address() { }

        private IList<Guid> _acceptedPayments;

        internal Address(Guid addressId, string fullAddress)
        {
            FullAddress = fullAddress;
            _acceptedPayments = new List<Guid>();
            Id = addressId;
        }

        public string FullAddress { get; private set; }

        public void RegisterAcceptedPayment(Guid paymentId)
        {
            if (_acceptedPayments.Contains(paymentId))
            {
                throw new PaymentAlreadyRegisteredException(paymentId.ToString());
            }

            _acceptedPayments.Add(paymentId);

            Publish(new RegisteredAcceptedPayment() { PaymentId = paymentId, AddressId = Id });
        }

        public void UpdateAddrress(string fullAddress)
        {
            FullAddress = fullAddress;

            Publish(new AddressChanged {FullAddress = fullAddress});
        }
    }
}