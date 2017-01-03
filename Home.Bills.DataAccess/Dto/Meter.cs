using System;

namespace Home.Bills.DataAccess.Dto
{
    public class Meter
    {
        public string SerialNumber { get; set; }

        public double State { get; set; }

        public Guid? AddressId { get; set; }

        public Guid MeterId { get; set; }
    }
}