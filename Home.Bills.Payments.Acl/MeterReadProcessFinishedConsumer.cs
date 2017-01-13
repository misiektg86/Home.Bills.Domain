using System.Linq;
using System.Threading.Tasks;
using Home.Bills.Client;
using Home.Bills.DataAccess.Dto;
using Home.Bills.Domain.Messages;
using Home.Bills.Payments.Domain.Consumers;
using Home.Bills.Payments.Domain.Services;
using Marten;
using MassTransit;

namespace Home.Bills.Payments.Acl
{
    public class MeterReadProcessFinishedConsumer : IConsumer<IMeterReadProcessFinished>
    {
        private readonly IServiceClient _billsServiceClient;
        private readonly PaymentDomainService _paymentDomainService;
        private readonly IDocumentSession _documentSession;

        public MeterReadProcessFinishedConsumer(IServiceClient billsServiceClient, PaymentDomainService paymentDomainService, IDocumentSession documentSession)
        {
            _billsServiceClient = billsServiceClient;
            _paymentDomainService = paymentDomainService;
            _documentSession = documentSession;
        }

        public async Task Consume(ConsumeContext<IMeterReadProcessFinished> context)
        {
            var meterRead = await _billsServiceClient.GetMeterRead(context.Message.MeterReadId);

            await _paymentDomainService.CreatePayment(context.Message.MeterReadId, context.Message.AddressId,
                 meterRead.Usages.Select(Convert).ToList());

            await _documentSession.SaveChangesAsync();
        }

        public static RegisteredUsage Convert(Usage dto)
        {
            return new RegisteredUsage()
            {
                MeterId = dto.MeterId,
                Value = dto.Value,
                ReadDateTime = dto.ReadDateTime
            };
        }
    }
}