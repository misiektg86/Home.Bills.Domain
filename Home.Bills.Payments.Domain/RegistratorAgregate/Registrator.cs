using System;
using Frameworks.Light.Ddd;
using MassTransit;

namespace Home.Bills.Payments.Domain.RegistratorAgregate
{
    public class Registrator : AggregateRoot<Guid>
    {
        public Guid AddressId { get; private set; }
        public Guid TariffId { get; private set; }
        public string Description { get; private set; }

        internal Registrator() { }

        internal Registrator(Guid registratorId, Guid addressId, Guid tariffId,string description, IBus messageBus) : base(messageBus)
        {
            Id = registratorId;
            AddressId = addressId;
            TariffId = tariffId;
            Description = description;
        }

        public void ApplyTariff(Guid tariffId)
        {
            TariffId = tariffId;

            Publish(new TariffAppliedForRegistrator { TariffId = TariffId, RegistratorId = Id, AddressId = AddressId });
        }
    }
}