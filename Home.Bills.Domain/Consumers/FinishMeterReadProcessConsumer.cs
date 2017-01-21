using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate;
using Home.Bills.Domain.MeterAggregate;
using MassTransit;

namespace Home.Bills.Domain.Consumers
{
    public class FinishMeterReadProcessConsumer : IConsumer<MeterReadCompleted>
    {
        private readonly IAsyncUnitOfWork _asyncUnitOfWork;
        private readonly IRepository<Address, Guid> _meterReadRepository;
        public FinishMeterReadProcessConsumer(IAsyncUnitOfWork asyncUnitOfWork, IRepository<Address, Guid> meterReadRepository)
        {
            _asyncUnitOfWork = asyncUnitOfWork;
            _meterReadRepository = meterReadRepository;
        }
        public async Task Consume(ConsumeContext<MeterReadCompleted> context)
        {
                var address = await _meterReadRepository.Get(context.Message.AddressId);

                address.FinishMeterReadProcess(context.Message.MeterReadId);

                _meterReadRepository.Update(address);

                await _asyncUnitOfWork.CommitAsync();
        }
    }
}