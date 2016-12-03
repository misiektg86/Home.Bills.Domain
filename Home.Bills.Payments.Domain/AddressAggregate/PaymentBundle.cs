using System;
using System.Collections.Generic;
using System.Linq;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    public class PaymentBundle
    {
        private List<Guid> _items;

        public Guid Id { get; set; }
        public int Month { get; }
        public int Year { get; }
        public bool Accepted { get; private set; }

        public PaymentBundle(Guid id, int month, int year)
        {
            Id = id;
            Month = month;
            Year = year;
            _items = new List<Guid>();
        }

        public void AddBundleItem(Guid itemId)
        {
            _items.Add(itemId);
        }

        public IEnumerable<Guid> GetItems()
        {
            return _items.ToList();
        }

        public void Accept()
        {
            Accepted = true;
        }
    }
}