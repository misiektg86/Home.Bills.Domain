using System.Threading.Tasks;
using MassTransit;

namespace Home.Bills.Notifications.Domain.Consumers
{
    public class CreatePaymentNotificationConsumer : IConsumer<CreatePaymentNotificationConsumer>
    {
        public Task Consume(ConsumeContext<CreatePaymentNotificationConsumer> context)
        {
            throw new System.NotImplementedException();
        }
    }
}