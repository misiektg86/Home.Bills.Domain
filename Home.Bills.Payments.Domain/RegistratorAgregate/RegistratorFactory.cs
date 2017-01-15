using System;
using Frameworks.Light.Ddd;
using MassTransit;

namespace Home.Bills.Payments.Domain.RegistratorAgregate
{
    public class RegistratorFactory : IAggregateFactory<Registrator, FactoryInput, Guid>
    {
        private readonly IBus _messageBus;
        public RegistratorFactory(IBus messageBus)
        {
            _messageBus = messageBus;
        }

        public Registrator Create(FactoryInput input)
        {
            return new Registrator(input.RegistratorId, input.AddressId, input.TariffId, input.Description, _messageBus);
        }
    }

    public class FactoryInput
    {
        public Guid RegistratorId { get; set; }
        public Guid AddressId { get; set; }
        public Guid? TariffId { get; set; }
        public string Description { get; set; }
    }
}