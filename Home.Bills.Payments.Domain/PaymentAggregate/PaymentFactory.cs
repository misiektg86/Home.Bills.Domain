using System;
using Frameworks.Light.Ddd;
using MassTransit;

namespace Home.Bills.Payments.Domain.PaymentAggregate
{
    public class PaymentFactory : AggregateRootFactoryBase<Payment, PaymentFactoryInput, Guid>
    {
        protected override Payment CreateInternal(PaymentFactoryInput input)
        {
            return new Payment(input.PaymentId, input.AddressId);
        }
    }
}