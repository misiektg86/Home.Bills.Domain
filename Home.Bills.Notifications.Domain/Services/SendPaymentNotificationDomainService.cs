using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Notifications.Domain.PaymentAggregate;

namespace Home.Bills.Notifications.Domain.Services
{
    public class SendPaymentNotificationDomainService
    {
        private readonly IRepository<Payment, Guid> _paymentRepository;
        private readonly INotifier _notifier;

        public SendPaymentNotificationDomainService(IRepository<Payment,Guid> paymentRepository, INotifier notifier )
        {
            _paymentRepository = paymentRepository;
            _notifier = notifier;
        }

        public async Task SendPaymentNotification(Guid paymentId)
        {
            var payment = await _paymentRepository.Get(paymentId);

            if (payment == null)
            {
                throw new PaymentNotFoundException(paymentId.ToString());
            }


        }

        private string BuildLineItem(string item)
        {
            return $"{item}</br></br>";
        }
    }

    public interface INotifier
    {
        Task Notify(string message, string subject);
    }
}