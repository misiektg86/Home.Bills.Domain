using System.Threading.Tasks;
using Home.Bills.Domain.MeterAggregate;
using MassTransit;

namespace Home.Bills.Domain.Consumers
{
    public class MeterStateUpdatedConsumer : IConsumer<MeterStateUpdated>
    {
        public Task Consume(ConsumeContext<MeterStateUpdated> context)
        {
            throw new System.NotImplementedException();
        }
    }
}