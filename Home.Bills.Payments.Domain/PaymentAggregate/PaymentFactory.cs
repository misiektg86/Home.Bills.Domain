using System;
using Frameworks.Light.Ddd;
using MassTransit;

namespace Home.Bills.Payments.Domain.PaymentAggregate
{
    public class PaymentFactory : IAggregateFactory<Payment,PaymentFactoryInput,Guid>
    {
        private readonly IBus _messageBus;
        public PaymentFactory(IBus messageBus)
        {
            _messageBus = messageBus;
        }

        public Payment Create(PaymentFactoryInput input)
        {
            return new Payment(input.PaymentId,input.AddressId,_messageBus);
        }
    }
}