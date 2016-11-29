using System;

namespace Home.Bills.Models
{
    public class CheckOut
    {
        public Guid AddressId { get; set; }

        public int Persons { get; set; }
    }
}