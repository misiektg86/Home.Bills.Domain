using System;

namespace Home.Bills.Models
{
    public class Meter
    {
        public string SerialNumber { get; set; }

        public double State { get; set; }

        public Guid AddressId { get; set; }
    }
}