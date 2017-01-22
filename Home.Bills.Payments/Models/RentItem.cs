namespace Home.Bills.Payments.Models
{
    public class RentItem
    {
        public string Description { get; set; }
        public decimal AmountPerUnit { get; set; }

        public int ItemPosition { get; set; }

        public RentUnit RentUnit { get; set; }
    }
}