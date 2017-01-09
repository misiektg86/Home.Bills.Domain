using System.Threading.Tasks;
using MassTransit;

namespace Home.Bills.Payments.Domain.Consumers
{
    public class RegistratorReadFinishedConsumer : IConsumer<RegistratorReadFinished>
    {
        public Task Consume(ConsumeContext<RegistratorReadFinished> context)
        {
            throw new System.NotImplementedException();
        }
    }
}