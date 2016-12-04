using System;
using System.Collections.Generic;
using System.Linq;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    public class PaymentBundle
    {
        private List<IPayment> _items;

        public Guid Id { get; set; }
        public int Month { get; }
        public int Year { get; }
        public bool Accepted { get; private set; }

        public PaymentBundle(Guid id, int month, int year)
        {
            Id = id;
            Month = month;
            Year = year;
            _items = new List<IPayment>();
        }

        public void AddBundleItem(IPayment itemId)
        {
            _items.Add(itemId);
        }

        public IEnumerable<IPayment> GetItems()
        {
            return _items.ToList();
        }

        public void Accept()
        {
            Accepted = true;
        }
    }
}