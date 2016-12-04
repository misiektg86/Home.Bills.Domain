using System;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    public class Rent : IPayment
    {
        public Rent(Guid id, decimal amountToPay)
        {
            Id = id;
            AmountToPay = amountToPay;
        }

        public Guid Id { get; }
        public decimal AmountToPay { get; }
    }
}