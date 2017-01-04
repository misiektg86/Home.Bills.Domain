using System;

namespace Home.Bills.DataAccess.Dto
{
    public class Usage
    {
        public Guid AddressId { get; set; }

        public Guid UsageId { get; set; }

        public double Value { get; set; }

        public Guid MeterId { get; set; }

        public DateTime ReadDateTime { get; set; }

        public double PrevioudRead { get; set; }

        public double CurrentRead { get; set; }
    }
}