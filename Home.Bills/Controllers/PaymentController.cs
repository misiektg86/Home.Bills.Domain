using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Payments.Domain;
using Microsoft.AspNetCore.Mvc;
using Payment = Home.Bills.Dtos.Payment;

namespace Home.Bills.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IPaymentsDataProvider _paymentDataProvider;
        private readonly IRepository<Payments.Domain.Payment, Guid> _paymentsRepository;

        public PaymentController(IPaymentsDataProvider paymentDataProvider, IRepository<Payments.Domain.Payment, Guid> paymentsRepository)
        {
            _paymentDataProvider = paymentDataProvider;
            _paymentsRepository = paymentsRepository;
        }

        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetPayments(Guid addressId)
        {
            var payments = await _paymentDataProvider.GetAllPaymentsForAddress(addressId);

            if (payments == null)
            {
                return new NotFoundResult();
            }

            return new ObjectResult(payments.Select(i => (Payment)i));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody]Guid addressId, [FromBody]decimal amount, [FromBody]string description)
        {
            var activePaymentExists = await _paymentDataProvider.ActivePaymentExists(addressId);

            if (activePaymentExists)
            {
                return StatusCode((int)HttpStatusCode.Conflict);
            }

            var payment = Payments.Domain.Payment.Create(addressId, amount, description);

            _paymentsRepository.Add(payment);

            return StatusCode(200, payment.Id);
        }

        [HttpPut("{paymentId}")]
        public async Task<IActionResult> Pay(Guid paymentId)
        {
            var payment = await _paymentsRepository.Get(paymentId);

            if (payment == null)
            {
                return NotFound(paymentId);
            }

            payment.Pay();

            _paymentsRepository.Update(payment);

            return StatusCode(200);
        }
    }
}