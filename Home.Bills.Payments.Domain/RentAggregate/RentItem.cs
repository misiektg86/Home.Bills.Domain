using System;

namespace Home.Bills.Payments.Domain.RentAggregate
{
    public class RentItem : ICloneable
    {
        public string Description { get; private set; }
        public decimal AmountPerUnit { get; private set; }

        public int ItemPosition { get; private set; }
        public RentUnit RentUnit { get; private set; }

        public RentItem(string description, decimal amountPerUnit, int itemPosition, RentUnit rentUnit)
        {
            Description = description;
            AmountPerUnit = amountPerUnit;
            ItemPosition = itemPosition;
            RentUnit = rentUnit;
        }

        internal RentItem() { }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}