using System;

namespace Home.Bills.Payments.Domain.TariffAggregate
{
    public class TariffFactoryInput
    {
        public Guid TariffId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? ValidTo { get; set; }
        public decimal TariffValue { get; set; }
        public string Description { get; set; }
    }
}