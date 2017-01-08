using System.Threading.Tasks;
using Home.Bills.Client;
using Home.Bills.Domain.Messages;
using MassTransit;

namespace Home.Bills.Payments.Acl
{
    public class MeterReadProcessFinishedConsumer : IConsumer<IMeterReadProcessFinished>
    {
        private readonly IServiceClient _billsServiceClient;
        public MeterReadProcessFinishedConsumer(IServiceClient billsServiceClient)
        {
            _billsServiceClient = billsServiceClient;
        }

        public Task Consume(ConsumeContext<IMeterReadProcessFinished> context)
        {
            throw new System.NotImplementedException();
        }
    }
}