using System;
using System.Linq;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Notifications.Domain.AddressAggregate;
using Home.Bills.Notifications.Domain.Consumers.Saga;
using Home.Bills.Notifications.Domain.PaymentAggregate;
using Home.Bills.Notifications.Domain.Services;
using Home.Bills.Payments.Client;
using MassTransit;

namespace Home.Bills.Notifications.Domain.Consumers
{
    public class CreatePaymentConsumer : IConsumer<CreatePaymentNotification>
    {
        private readonly IAsyncUnitOfWork _unitOfWork;

        private readonly IRepository<Payment, Guid> _paymentRepository;

        private readonly IServiceClient _serviceClient;

        private readonly Client.IServiceClient _billsServiceClient;

        public CreatePaymentConsumer(IAsyncUnitOfWork unitOfWork, IRepository<Payment,Guid> paymentRepository, IServiceClient serviceClient, Client.IServiceClient billsServiceClient  )
        {
            _unitOfWork = unitOfWork;
            _paymentRepository = paymentRepository;
            _serviceClient = serviceClient;
            _billsServiceClient = billsServiceClient;
        }
        public async Task Consume(ConsumeContext<CreatePaymentNotification> context)
        {
            var paymentDetails = await _serviceClient.GetPayment(context.Message.PaymentId);

            var address = await _billsServiceClient.GetAddressDetails(paymentDetails.AddressId);

            var payment = Payment.Create(context.Message.PaymentId, address.AddressId,
                paymentDetails.PaymentItems.Select(Convert).ToList(),
                $"{address.StreetNumber} {address.StreetNumber}/{address.HomeNumber}, {address.City}");

            _paymentRepository.Add(payment);

            await _unitOfWork.CommitAsync();

            await context.Publish<IPaymentCreated>(new {context.Message.PaymentId});
        }

        private PaymentItem Convert(Payments.DataAccess.Dtos.PaymentItem paymentItem)
        {
            return new PaymentItem() {Amount = paymentItem.Amount, Description = paymentItem.Description};
        }
    }
}