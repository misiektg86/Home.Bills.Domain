using System.Threading.Tasks;
using MassTransit;

namespace Home.Bills.Payments.Domain.Consumers
{
    public class RegistratorReadFinishedConsumer : IConsumer<RegistratorsReadFinished>
    {
        public Task Consume(ConsumeContext<RegistratorsReadFinished> context)
        {
            throw new System.NotImplementedException();
        }
    }
}