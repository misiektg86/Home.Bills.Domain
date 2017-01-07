using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.Messages;
using Home.Bills.Domain.MeterReadAggregate;
using Marten;
using MassTransit;

namespace Home.Bills.Domain.Consumers
{
    public class MeterReadProcessCanceledConsumer : IConsumer<IMeterReadProcessCanceled>
    {
        private readonly IDocumentSession _documentSession;
        private readonly IRepository<MeterRead, Guid> _meterReadRepository;
        public MeterReadProcessCanceledConsumer(IDocumentSession documentSession, IRepository<MeterRead, Guid> meterReadRepository)
        {
            _documentSession = documentSession;
            _meterReadRepository = meterReadRepository;
        }
        public async Task Consume(ConsumeContext<IMeterReadProcessCanceled> context)
        {
            _meterReadRepository.Delete(context.Message.MeterReadId);

            await _documentSession.SaveChangesAsync();
        }
    }
}