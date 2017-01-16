using System;
using Frameworks.Light.Ddd;

namespace Home.Bills.Payments.Domain.TariffAggregate
{
    public class TariffFactory : AggregateRootFactoryBase<Tariff, TariffFactoryInput, Guid>
    {
        protected override Tariff CreateInternal(TariffFactoryInput input)
        {
            return new Tariff(input.TariffId, input.Created, input.ValidTo, input.TariffValue, input.Description);
        }
    }
}