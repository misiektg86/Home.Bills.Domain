using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.MeterReadAggregate;
using MassTransit;

namespace Home.Bills.Domain.Consumers
{
    public class MeterReadProcessFinishedConsumer : IConsumer<FinishMeterReadProcess>
    {
        private readonly IAsyncUnitOfWork _asyncUnitOfWork;
        private readonly IRepository<MeterRead, Guid> _meterReadRepository;
        public MeterReadProcessFinishedConsumer(IAsyncUnitOfWork asyncUnitOfWork, IRepository<MeterRead, Guid> meterReadRepository)
        {
            _asyncUnitOfWork = asyncUnitOfWork;
            _meterReadRepository = meterReadRepository;
        }
        public async Task Consume(ConsumeContext<FinishMeterReadProcess> context)
        {
            var meterRead = await _meterReadRepository.Get(context.Message.MeterReadId);

            meterRead.CompleteMeterRead();

            _meterReadRepository.Update(meterRead);

            await _asyncUnitOfWork.CommitAsync();
        }
    }
}