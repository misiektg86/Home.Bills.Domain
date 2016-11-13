using System;
using System.Linq;
using System.Threading.Tasks;
using Home.Bills.Payments.Domain;
using Microsoft.AspNetCore.Mvc;
using Payment = Home.Bills.Dtos.Payment;

namespace Home.Bills.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IPaymentsDataProvider _paymentDataProvider;

        public PaymentController(IPaymentsDataProvider paymentDataProvider)
        {
            _paymentDataProvider = paymentDataProvider;
        }

        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetPayments(Guid addressId)
        {
            var payments = await _paymentDataProvider.GetAllPaymentsForAddress(addressId);

            if (payments == null)
            {
                return new NotFoundResult();
            }

            return new ObjectResult(payments.Cast<Payment>());
        }
    }
}