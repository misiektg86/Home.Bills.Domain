using System;
using System.Collections.Generic;

namespace Home.Bills.DataAccess.Dto
{
    public class MeterRead
    {
        public DateTime ReadBeginDateTime { get; set; }

        public IEnumerable<Guid> Meters { get; set; }

        public Guid AddressId { get; set; }

        public bool IsCompleted { get; set; }

        public IEnumerable<Usage> Usages { get; set; }
        public Guid MeterReadId { get; set; }
    }
}