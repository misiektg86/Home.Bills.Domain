using System;
using Frameworks.Light.Ddd;
using MassTransit;

namespace Home.Bills.Payments.Domain.TariffAggregate
{
    public class TariffFactory : IAggregateFactory<Tariff, TariffFactoryInput, Guid>
    {
        private readonly IBus _messageBus;
        public TariffFactory(IBus messageBus)
        {
            _messageBus = messageBus;
        }

        public Tariff Create(TariffFactoryInput input)
        {
            return new Tariff(input.TariffId, input.Created, input.ValidTo, input.TariffValue, input.Description, _messageBus);
        }
    }
}