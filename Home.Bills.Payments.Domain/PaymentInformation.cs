using System;

namespace Home.Bills.Payments.Domain
{
    public struct PaymentInformation
    {
        public PaymentInformation(Guid paymentId, bool paid, DateTime created, string description, Guid addressId, decimal toPay, DateTime? paidDate)
        {
            PaymentId = paymentId;
            Paid = paid;
            Created = created;
            Description = description;
            AddressId = addressId;
            ToPay = toPay;
            PaidDate = paidDate;
        }

        public Guid PaymentId { get; }
        public bool Paid { get; }
        public DateTime? PaidDate { get; }
        public DateTime Created { get; }
        public string Description { get; }
        public Guid AddressId { get; }
        public decimal ToPay { get; }
    }
}