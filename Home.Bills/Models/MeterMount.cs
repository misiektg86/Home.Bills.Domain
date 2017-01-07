using System;

namespace Home.Bills.Models
{
    public class MeterMount
    {
        public Guid AddressId { get; set; }

        public Guid MeterId { get; set; }
    }
}