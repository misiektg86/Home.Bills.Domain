using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.Messages;
using Home.Bills.Domain.MeterReadAggregate;
using MassTransit;

namespace Home.Bills.Domain.Consumers
{
    public class MeterReadProcessCanceledConsumer : IConsumer<IMeterReadProcessCanceled>
    {
        private readonly IAsyncUnitOfWork _asyncUnitOfWork;
        private readonly IRepository<MeterRead, Guid> _meterReadRepository;
        public MeterReadProcessCanceledConsumer(IAsyncUnitOfWork asyncUnitOfWork, IRepository<MeterRead, Guid> meterReadRepository)
        {
            _asyncUnitOfWork = asyncUnitOfWork;
            _meterReadRepository = meterReadRepository;
        }
        public async Task Consume(ConsumeContext<IMeterReadProcessCanceled> context)
        {
            _meterReadRepository.Delete(context.Message.MeterReadId);

            await _asyncUnitOfWork.CommitAsync();
        }
    }
}