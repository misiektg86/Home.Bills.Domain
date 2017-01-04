using System;

namespace Home.Bills.Models
{
    public class ProvidedRead
    {
        public Guid AddressId { get; set; }

        public double Read { get; set; }

        public string MeterSerialNumber { get; set; }

        public DateTime ReadDate { get; set; }
    }
}