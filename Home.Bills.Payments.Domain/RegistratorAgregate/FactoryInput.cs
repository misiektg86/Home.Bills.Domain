using System;

namespace Home.Bills.Payments.Domain.RegistratorAgregate
{
    public class FactoryInput
    {
        public Guid RegistratorId { get; set; }
        public Guid AddressId { get; set; }
        public Guid? TariffId { get; set; }
        public string Description { get; set; }
    }
}