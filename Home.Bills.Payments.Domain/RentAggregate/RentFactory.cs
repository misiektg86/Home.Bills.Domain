using System;
using Frameworks.Light.Ddd;

namespace Home.Bills.Payments.Domain.RentAggregate
{
    public class RentFactory: AggregateRootFactoryBase<Rent,RentFactoryInput,Guid>
    {
        protected override Rent CreateInternal(RentFactoryInput input)
        {
            return new Rent(input.RentId,input.ValidTo,input.RentItems);
        }
    }
}