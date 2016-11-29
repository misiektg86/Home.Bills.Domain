using System;

namespace Home.Bills.Models
{
    public class CheckIn
    {
        public Guid AddressId { get; set; }

        public int Persons { get; set; }
    }
}