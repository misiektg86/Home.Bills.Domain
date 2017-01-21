using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.MeterReadAggregate;
using Home.Bills.Domain.Services;
using MassTransit;

namespace Home.Bills.Domain.Consumers
{
    public class CreateUsageCalculationConsumer : IConsumer<CreateUsageCalculation>
    {
        private readonly UsageDomainService _usageDomainService;
        private readonly IAsyncUnitOfWork _asyncUnitOfWork;

        public CreateUsageCalculationConsumer(UsageDomainService usageDomainService, IAsyncUnitOfWork asyncUnitOfWork)
        {
            _usageDomainService = usageDomainService;
            _asyncUnitOfWork = asyncUnitOfWork;
        }

        public async Task Consume(ConsumeContext<CreateUsageCalculation> context)
        {
            await _usageDomainService.CalculateUsage(context.Message.MeterReadId, context.Message.AddressId, context.Message.MeterState, context.Message.MeterId, context.Message.UsageId);

            await _asyncUnitOfWork.CommitAsync();
        }
    }
}