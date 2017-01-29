using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Notifications.Domain.Consumers.Saga;
using Home.Bills.Notifications.Domain.Services;
using Home.Bills.Notifications.Messages;
using MassTransit;

namespace Home.Bills.Notifications.Domain.Consumers
{
    public class SendPaymentNotificationConsumer : IConsumer<SendPaymentNotification>
    {
        private readonly PaymentNotificationDomainService _domainService;
        private readonly IAsyncUnitOfWork _asyncUnitOfWork;

        public SendPaymentNotificationConsumer(PaymentNotificationDomainService domainService, IAsyncUnitOfWork asyncUnitOfWork)
        {
            _domainService = domainService;
            _asyncUnitOfWork = asyncUnitOfWork;
        }

        public async Task Consume(ConsumeContext<SendPaymentNotification> context)
        {
            var message = await _domainService.CreatePaymentNotification(context.Message.PaymentId);

            await _asyncUnitOfWork.CommitAsync();

            await context.Publish<INotification>(message);
        }
    }
}