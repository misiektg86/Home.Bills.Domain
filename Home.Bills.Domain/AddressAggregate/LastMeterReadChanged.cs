using System;

namespace Home.Bills.Domain.AddressAggregate
{
    public class LastMeterReadChanged
    {
        public Guid AddressId { get; set; }
        public Guid? OldMeterRead { get; set; }
        public Guid? NewMeterRead { get; set; }
        public DateTime ChangedDateTime { get; set; }
    }
}