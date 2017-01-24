using System.Threading.Tasks;
using Home.Bills.Notifications.Domain.Consumers.Saga;
using MassTransit;

namespace Home.Bills.Notifications.Domain.Consumers
{
    public class MarkPaymentNotificationAsReadConsumer : IConsumer<MarkPaymentNotificationAsSent>
    {
        public Task Consume(ConsumeContext<MarkPaymentNotificationAsSent> context)
        {
            throw new System.NotImplementedException();
        }
    }
}