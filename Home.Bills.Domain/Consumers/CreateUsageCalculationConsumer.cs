using System.Threading.Tasks;
using Home.Bills.Domain.MeterReadAggregate;
using Home.Bills.Domain.Services;
using MassTransit;

namespace Home.Bills.Domain.Consumers
{
    public class CreateUsageCalculationConsumer : IConsumer<CreateUsageCalculation>
    {
        private readonly UsageDomainService _usageDomainService;

        public CreateUsageCalculationConsumer(UsageDomainService usageDomainService)
        {
            _usageDomainService = usageDomainService;
        }
        public async Task Consume(ConsumeContext<CreateUsageCalculation> context)
        {
            await _usageDomainService.CalculateUsage(context.Message.MeterReadId, context.Message.AddressId, context.Message.MeterState, context.Message.MeterId);
        }
    }
}