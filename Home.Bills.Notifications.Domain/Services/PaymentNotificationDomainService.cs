using System;
using System.Text;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Notifications.Domain.AddressAggregate;
using Home.Bills.Notifications.Domain.PaymentAggregate;

namespace Home.Bills.Notifications.Domain.Services
{
    public class PaymentNotificationDomainService
    {
        private readonly IRepository<Payment, Guid> _paymentRepository;
        private readonly IRepository<Address, Guid> _addressRepository;

        public PaymentNotificationDomainService(IRepository<Payment, Guid> paymentRepository, IRepository<Address, Guid> addressRepository)
        {
            _paymentRepository = paymentRepository;
            _addressRepository = addressRepository;
        }

        public async Task<NotificationMessage> CreatePaymentNotification(Guid paymentId)
        {
            var payment = await _paymentRepository.Get(paymentId);

            if (payment == null)
            {
                throw new PaymentNotFoundException(paymentId.ToString());
            }

            if (string.IsNullOrEmpty(payment.FullAddress))
            {
                throw new AddressCnnotBeEmptyException();
            }

            var administratorEmail = (await _addressRepository.Get(payment.AddressId))?.BuildingAdministratorEmail;

            if (string.IsNullOrEmpty(administratorEmail))
            {
                throw new BuildingAdmistratorEmailCannotBeEmptyException(payment.AddressId.ToString());
            }

            StringBuilder message = new StringBuilder();

            foreach (var paymentPaymentItem in payment.PaymentItems)
            {
                message.Append(BuildLineItem($"{paymentPaymentItem.Description} - {paymentPaymentItem.Amount} zł")); // TODO refactor...
            }

            return new NotificationMessage() { Message = message.ToString(), Subject = payment.FullAddress, ToAddress = administratorEmail, NotificationId = paymentId };
        }

        private string BuildLineItem(string item)
        {
            return $"{item}<br><br>";
        }
    }
}