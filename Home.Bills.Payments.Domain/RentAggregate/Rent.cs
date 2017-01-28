using System;
using System.Collections.Generic;
using System.Linq;
using Frameworks.Light.Ddd;
using Newtonsoft.Json;

namespace Home.Bills.Payments.Domain.RentAggregate
{
    public class Rent :AggregateRoot<Guid>
    {
        public DateTime? ValidTo { get; private set; }

        private List<RentItem> _rentItems;

        [JsonIgnore]
        public IEnumerable<RentItem> RentItems => _rentItems.Select(i=>(RentItem)i.Clone());

        internal Rent()
        {
            _rentItems = new List<RentItem>();
        }

        internal Rent(Guid rentId, DateTime? validTo,params RentItem[] items)
        {
            Id = rentId;
            ValidTo = validTo;
            _rentItems = new List<RentItem>(items);
        }

        public bool HasExpired() => ValidTo.HasExpired();

        public void AddRentItem(RentItem item)
        {
            if (ValidTo.HasExpired())
            {
                throw new RentHasExpiredException();
            }

            if (_rentItems.Any(i => i.ItemPosition == item.ItemPosition))
            {
                throw new RentItemAlreadyExistsOnPosition();
            }
        }

        public void RemoveRentItem(int position)
        {
            if (ValidTo.HasExpired())
            {
                throw new RentHasExpiredException();
            }

            var rentItem = _rentItems.Single(item => item.ItemPosition == position);

            _rentItems.Remove(rentItem);
        }
    }
}