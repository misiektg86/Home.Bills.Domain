using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate;
using Home.Bills.Domain.AddressAggregate.Events;
using Home.Bills.Domain.MeterAggregate;
using Home.Bills.Domain.MeterReadAggregate;
using Marten;
using MassTransit;

namespace Home.Bills.Domain.Consumers
{
    public class FinishMeterReadProcessConsumer : IConsumer<MeterReadCompleted>
    {
        private readonly IDocumentSession _documentSession;
        private readonly IRepository<Address, Guid> _meterReadRepository;
        public FinishMeterReadProcessConsumer(IDocumentSession documentSession, IRepository<Address, Guid> meterReadRepository)
        {
            _documentSession = documentSession;
            _meterReadRepository = meterReadRepository;
        }
        public async Task Consume(ConsumeContext<MeterReadCompleted> context)
        {
            try
            {
                var address = await _meterReadRepository.Get(context.Message.AddressId);

                address.FinishMeterReadProcess(context.Message.MeterReadId);

                _meterReadRepository.Update(address);

                await _documentSession.SaveChangesAsync();
            }
            finally
            {
                _documentSession?.Dispose();
            }

        }
    }
}