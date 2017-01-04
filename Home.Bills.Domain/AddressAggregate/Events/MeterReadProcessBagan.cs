using System;
using System.Collections.Generic;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    public class MeterReadProcessBagan
    {
        public Guid MeterReadId { get; set; }

        public Guid AddressId { get; set; }

        public IEnumerable<Guid> MeterIds { get; set; }

        public DateTime ReadProcessStartDate { get; set; }
    }
}