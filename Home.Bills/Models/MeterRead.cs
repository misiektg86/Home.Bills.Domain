using System;

namespace Home.Bills.Models
{
    public class MeterRead
    {
        public Guid MeterReadId { get; set; }

        public Guid AddressId { get; set; }
    }
}