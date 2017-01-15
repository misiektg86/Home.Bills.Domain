using System;

namespace Home.Bills.Payments.Models
{
    public class CreateTariff
    {
        public Guid TariffId { get; set; }
        public decimal TariffPrice { get; set; }
        public string Description { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}