using System;
using Frameworks.Light.Ddd;
using MassTransit;

namespace Home.Bills.Payments.Domain.RegistratorAgregate
{
    public class Registrator : AggregateRoot<Guid>
    {
        public Guid RegistratorId { get; }
        public Guid AddressId { get; }
        public Guid TariffId { get; private set; }

        internal Registrator() { }

        internal Registrator(Guid registratorId, Guid addressId, Guid tariffId, IBus messageBus) : base(messageBus)
        {
            RegistratorId = registratorId;
            AddressId = addressId;
            TariffId = tariffId;
        }

        public void ApplyTariff(Guid tariffId)
        {
            TariffId = tariffId;

            Publish(new TariffAppliedForRegistrator { TariffId = TariffId, RegistratorId = RegistratorId, AddressId = AddressId });
        }
    }
}