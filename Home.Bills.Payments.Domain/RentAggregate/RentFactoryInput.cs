using System;
using System.Collections.Generic;

namespace Home.Bills.Payments.Domain.RentAggregate
{
    public class RentFactoryInput
    {
        public Guid RentId { get; set; }

        public DateTime? ValidTo { get; set; }

        public RentItem[] RentItems { get; set; }
    }
}