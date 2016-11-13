using System;
using Frameworks.Light.Ddd;

namespace Home.Bills.Payments.Domain
{
    public class Payment : AggregateRoot<Guid>
    {
        private readonly Guid _addressId;
        private readonly decimal _toPay;
        private readonly string _description;
        private readonly DateTime _created;
        private bool _paid;
        private DateTime? _paidDate;

        public DateTime PaidDate { get; set; }

        internal Payment() { }

        private Payment(Guid id, Guid addressId, decimal toPay, string description, DateTime created)
        {
            _addressId = addressId;
            _toPay = toPay;
            _description = description;
            _created = created;
            Id = id;
        }

        public static Payment Create(Guid addressId, decimal toPay, string description)
        {
            return new Payment(Guid.NewGuid(), addressId, toPay, description, DateTime.Now);
        }

        public void Pay()
        {
            if (_paid)
            {
                throw new PaymentAlreadyPaidException($"Payment {Id} has already been paid.");
            }

            _paid = true;
            _paidDate = DateTime.Now;
        }

        public PaymentInformation PaymentInformations
            => new PaymentInformation(Id, _paid, _created, _description, _addressId, _toPay, _paidDate);

        public Guid AddressId => _addressId;
    }
}
