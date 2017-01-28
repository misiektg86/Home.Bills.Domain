using System;

namespace Home.Bills.Payments.Models
{
    public class CreateRent
    {
        public Guid RentId { get; set; }

        public DateTime? ValidTo { get; set; }

        public RentItem[] RentItems { get; set; }
    }
}