using System;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Notifications.Domain.AddressAggregate;
using Home.Bills.Notifications.Domain.PaymentAggregate;

namespace Home.Bills.Notifications.Domain.Services
{
    public class SendPaymentNotificationDomainService
    {
        private readonly IRepository<Payment, Guid> _paymentRepository;

        public SendPaymentNotificationDomainService(IRepository<Payment,Guid> paymentRepository)
        {
            _paymentRepository = paymentRepository;
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

            StringBuilder message = new StringBuilder();

            foreach (var paymentPaymentItem in payment.PaymentItems)
            {
                message.Append(BuildLineItem($"{paymentPaymentItem.Description} - {paymentPaymentItem.Amount} zł"));
            }


        }

        private string BuildLineItem(string item)
        {
            return $"{item}</br></br>";
        }
    }

    public class NotificationMessage
    {
        public string Message { get; set; }

        public string Subject { get; set; }
    }

    public class AddressCnnotBeEmptyException : Exception
    {
        public AddressCnnotBeEmptyException()
        {
        }

        public AddressCnnotBeEmptyException(string message) : base(message)
        {
        }

        public AddressCnnotBeEmptyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AddressCnnotBeEmptyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public interface INotifier
    {
        Task Notify(string message, string subject);
    }
}