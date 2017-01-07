using System;

namespace Home.Bills.Models
{
    public class Usage
    {
        public Guid AddressId { get; set; }

        public Guid UsageId { get; set; }

        public Guid PreviousUsageId { get; set; }

        public string MeterSerialNumber { get; set; }

        public Guid MeterId { get; set; }

        public DateTime ReadDateTime { get; set; }
    }
}