using System.Threading.Tasks;
using Home.Bills.Payments.Client;
using Home.Bills.Payments.Messages;
using MassTransit;

namespace Home.Bills.Notifications.Acl
{
    public class PaymentAcceptedConsumer : IConsumer<IPaymentAccepted>
    {
        private readonly IServiceClient _serviceClient;

        public PaymentAcceptedConsumer(IServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public Task Consume(ConsumeContext<IPaymentAccepted> context)
        {
            throw new System.NotImplementedException();
        }
    }
}