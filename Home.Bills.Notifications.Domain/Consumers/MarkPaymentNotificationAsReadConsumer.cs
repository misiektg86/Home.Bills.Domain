using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Notifications.Domain.Consumers.Saga;
using Home.Bills.Notifications.Domain.PaymentAggregate;
using MassTransit;

namespace Home.Bills.Notifications.Domain.Consumers
{
    public class MarkPaymentNotificationAsReadConsumer : IConsumer<MarkPaymentNotificationAsSent>
    {
        private readonly IRepository<Payment, Guid> _paymentRepository;
        private readonly IAsyncUnitOfWork _asyncUnitOfWork;

        public MarkPaymentNotificationAsReadConsumer(IRepository<Payment,Guid> paymentRepository, IAsyncUnitOfWork asyncUnitOfWork )
        {
            _paymentRepository = paymentRepository;
            _asyncUnitOfWork = asyncUnitOfWork;
        }

        public async Task Consume(ConsumeContext<MarkPaymentNotificationAsSent> context)
        {
            var entity = await _paymentRepository.Get(context.Message.PaymentId);

            entity.MarkAsSent();

            _paymentRepository.Update(entity);

            await _asyncUnitOfWork.CommitAsync();
        }
    }
}