using System;

namespace Home.Bills.Payments.DataAccess.Dtos
{
    public class Registrator
    {
        public Guid RegistratorId { get; set; }
        public Guid AddressId { get;  set; }
        public Guid TariffId { get;  set; }
        public string Description { get;  set; }
    }
}