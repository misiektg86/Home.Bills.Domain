using System;

namespace Home.Bills.Domain.MeterReadAggregate
{
    internal class ReadProvided
    {
        public Guid AddressId { get; set; }
        public Guid MeterId { get; set; }
        public double PreviousRead { get; set; }
        public double NewRead { get; set; }
        public DateTime ReadDateTime { get; set; }
    }
}