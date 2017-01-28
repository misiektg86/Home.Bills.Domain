using System;
using System.Threading.Tasks;
using Home.Bills.Payments.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Home.Bills.Notifications.Controllers
{
    [Route("api/[controller]")]
    public class PaymentsController : Controller
    {
        private readonly IBus _messageBus;
        public PaymentsController(IBus messageBus)
        {
            _messageBus = messageBus;
        }

        [HttpPost("notification/initiate/{addressId}/{paymentId}")]
        public Task InitiatePaymentNotification(Guid addressId, Guid paymentId)
        {
            return _messageBus.Publish<IPaymentAccepted>(new { AddressId = addressId, PaymentId = paymentId });
        }
    }
}