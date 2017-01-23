using System.Threading.Tasks;
using Home.Bills.Notifications.Domain.AddressAggregate;
using MassTransit;

namespace Home.Bills.Notifications.Domain.Consumers
{
    public class RegisteredAcceptedPaymentConsumer : IConsumer<RegisteredAcceptedPayment>
    {
        public Task Consume(ConsumeContext<RegisteredAcceptedPayment> context)
        {
            throw new System.NotImplementedException();
        }
    }
}