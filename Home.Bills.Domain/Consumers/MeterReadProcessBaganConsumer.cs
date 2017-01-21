using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.Events;
using Home.Bills.Domain.MeterReadAggregate;
using MassTransit;

namespace Home.Bills.Domain.Consumers
{
    public class MeterReadProcessBaganConsumer : IConsumer<MeterReadProcessBagan>
    { 
        private readonly IAsyncUnitOfWork _asyncUnitOfWork;
        private readonly IRepository<MeterRead, Guid> _meterReadRepository;
        private readonly IAggregateFactory<MeterRead, MeterReadFactoryInput, Guid> _meterReadFactory;
        public MeterReadProcessBaganConsumer(IAsyncUnitOfWork asyncUnitOfWork, IRepository<MeterRead, Guid> meterReadRepository, IAggregateFactory<MeterRead, MeterReadFactoryInput, Guid> meterReadFactory)
        {
            _asyncUnitOfWork = asyncUnitOfWork;
            _meterReadRepository = meterReadRepository;
            _meterReadFactory = meterReadFactory;
        }

        public async Task Consume(ConsumeContext<MeterReadProcessBagan> context)
        {
            var meterRead =
                _meterReadFactory.Create(new MeterReadFactoryInput(context.Message.MeterReadId,
                    context.Message.AddressId, context.Message.MeterIds, context.Message.ReadProcessStartDate));

            _meterReadRepository.Add(meterRead);
            await _asyncUnitOfWork.CommitAsync();
        }
    }
}