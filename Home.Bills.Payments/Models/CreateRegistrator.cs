using System;

namespace Home.Bills.Payments.Models
{
    public class CreateRegistrator
    {
        public Guid RegistratorId { get; set; }
        public Guid AddressId { get; set; }
        public Guid TariffId { get;  set; }
        public string Description { get; set; }

    }
}