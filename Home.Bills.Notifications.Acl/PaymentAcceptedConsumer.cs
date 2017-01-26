using System.Threading.Tasks;
using Home.Bills.Payments.Client;
using Home.Bills.Payments.Messages;
using MassTransit;

namespace Home.Bills.Notifications.Acl
{
    public class PaymentAcceptedConsumer : IConsumer<IPaymentAccepted>
    {
        public Task Consume(ConsumeContext<IPaymentAccepted> context)
        {
           return context.Publish<Domain.Consumers.Saga.IPaymentAccepted>(
                new {context.Message.PaymentId, context.Message.AddressId});
        }
    }
}