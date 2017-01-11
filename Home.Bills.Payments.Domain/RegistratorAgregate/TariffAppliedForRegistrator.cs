using System;

namespace Home.Bills.Payments.Domain.RegistratorAgregate
{
    internal class TariffAppliedForRegistrator
    {
        public Guid TariffId { get; set; }
        public Guid RegistratorId { get; set; }
        public Guid AddressId { get; set; }
    }
}