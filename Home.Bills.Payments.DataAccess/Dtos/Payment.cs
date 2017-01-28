using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Home.Bills.Payments.DataAccess.Dtos
{
    public class Payment
    {
        public DateTime? Canceled { get; set; }

        public DateTime? Setteled { get; set; }

        public DateTime? Accepted { get; set; }

        public Guid AddressId { get; set; }

        public Guid PaymentId { get; set; }

        public IEnumerable<PaymentItem> PaymentItems { get; set; }

        public decimal TotalAmount => Math.Round(PaymentItems?.Sum(i => i.Amount) ?? 0m, 2);
    }
}