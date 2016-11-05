using System;

namespace Home.Bills.Models
{
    public class MeterRead
    {
        public Guid AddressId { get; set; }

        public double Read { get; set; }

        public string MeterSerialNumber { get; set; }

        public DateTime ReadDate { get; set; }
    }
}