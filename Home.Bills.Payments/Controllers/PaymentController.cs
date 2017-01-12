using System;
using System.Linq;
using System.Threading.Tasks;
using Home.Bills.Client;
using Home.Bills.DataAccess.Dto;
using Home.Bills.Payments.Domain.Consumers;
using Home.Bills.Payments.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Home.Bills.Payments.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IServiceClient _serviceClient;
        private readonly PaymentDomainService _paymentDomainService;

        public PaymentController(IServiceClient serviceClient, PaymentDomainService paymentDomainService )
        {
            _serviceClient = serviceClient;
            _paymentDomainService = paymentDomainService;
        }

        //[HttpGet("{addressId}")]
        //public async Task<IActionResult> GetPayments(Guid addressId)
        //{
        //    var payments = await _paymentDataProvider.GetAllPaymentsForAddress(addressId);

        //    if (payments == null)
        //    {
        //        return new NotFoundResult();
        //    }

        //    return new ObjectResult(payments.Select(i => (Payment)i));
        //}

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody]CreatePayment model)
        {
            var meterRead = await _serviceClient.GetMeterRead(model.MeterReadId);

            await _paymentDomainService.CreatePayment(model.MeterReadId, model.AddressId, meterRead.Usages.Select(Convert).ToList());

            return StatusCode(200, model.PaymentId);
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

    public class CreatePayment
    {
        public Guid MeterReadId { get; set; }
        public Guid AddressId { get; set; }
        public Guid PaymentId { get; set; }
    }
}