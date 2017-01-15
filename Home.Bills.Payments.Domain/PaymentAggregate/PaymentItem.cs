using System;

namespace Home.Bills.Payments.Domain.PaymentAggregate
{
    public class PaymentItem : ICloneable
    {
        public string Description { get; private set; }
        public decimal Amount { get; private set; }

        internal PaymentItem() { }

        internal PaymentItem(string description, decimal amount)
        {
            Description = description;
            Amount = amount;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}