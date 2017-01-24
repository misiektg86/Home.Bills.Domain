using System.Threading.Tasks;
using Home.Bills.Notifications.Domain.Consumers.Saga;
using MassTransit;

namespace Home.Bills.Notifications.Domain.Consumers
{
    public class SendPaymentNotificationConsumer : IConsumer<SendPaymentNotification>
    {
        public Task Consume(ConsumeContext<SendPaymentNotification> context)
        {
            throw new System.NotImplementedException();
        }
    }
}