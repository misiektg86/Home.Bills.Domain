using System;

namespace Home.Bills.Payments.Domain.Consumers
{
    public class RegisteredUsage
    {
        public double Value { get; set; }

        public Guid MeterId { get; set; }

        public DateTime ReadDateTime { get; set; }
    }
}