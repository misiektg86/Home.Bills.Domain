using System;

namespace Home.Bills.Models
{
    public class MeterCreate
    {
        public Guid? AddressId { get; set; }

        public Guid MeterId { get; set; }

        public string SerialNumber { get; set; }

        public double State { get; set; }
    }
}