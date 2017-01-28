using System.Threading.Tasks;
using Home.Bills.Notifications.Domain.Consumers.Saga;
using Home.Bills.Notifications.Domain.Services;
using Home.Bills.Notifications.Messages;
using MassTransit;

namespace Home.Bills.Notifications.Domain.Consumers
{
    public class SendPaymentNotificationConsumer : IConsumer<SendPaymentNotification>
    {
        private readonly PaymentNotificationDomainService _domainService;

        public SendPaymentNotificationConsumer(PaymentNotificationDomainService domainService)
        {
            _domainService = domainService;
        }

        public async Task Consume(ConsumeContext<SendPaymentNotification> context)
        {
            var message = await _domainService.CreatePaymentNotification(context.Message.PaymentId);

            await context.Publish<INotification>(message);
        }
    }
}