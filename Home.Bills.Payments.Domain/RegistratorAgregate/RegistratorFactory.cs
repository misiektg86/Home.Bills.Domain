using System;
using Frameworks.Light.Ddd;

namespace Home.Bills.Payments.Domain.RegistratorAgregate
{
    public class RegistratorFactory : AggregateRootFactoryBase<Registrator, FactoryInput, Guid>
    {
        protected override Registrator CreateInternal(FactoryInput input)
        {
            return new Registrator(input.RegistratorId, input.AddressId, input.TariffId, input.Description);
        }
    }
}