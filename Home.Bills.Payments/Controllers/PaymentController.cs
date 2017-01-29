using System;
using System.Linq;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Client;
using Home.Bills.DataAccess.Dto;
using Home.Bills.Payments.DataAccess.Dtos;
using Home.Bills.Payments.Domain.Consumers;
using Home.Bills.Payments.Domain.Services;
using Home.Bills.Payments.Models;
using Microsoft.AspNetCore.Mvc;
using Payment = Home.Bills.Payments.Domain.PaymentAggregate.Payment;

namespace Home.Bills.Payments.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IServiceClient _serviceClient;
        private readonly PaymentDomainService _paymentDomainService;
        private readonly IPaymentsDataProvider _paymentsDataProvider;
        private readonly IRepository<Payment, Guid> _repository;

        public PaymentController(IServiceClient serviceClient, PaymentDomainService paymentDomainService, IPaymentsDataProvider paymentsDataProvider, IRepository<Payment, Guid> repository)
        {
            _serviceClient = serviceClient;
            _paymentDomainService = paymentDomainService;
            _paymentsDataProvider = paymentsDataProvider;
            _repository = repository;
        }

        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetPayments(Guid addressId)
        {
            var payments = await _paymentsDataProvider.GetPayments(addressId);

            if (payments == null)
            {
                return new NotFoundResult();
            }

            return new ObjectResult(payments);
        }

        [HttpGet("GetPaymentById/{paymentId}")]
        public async Task<IActionResult> Get(Guid paymentId)
        {
            return new ObjectResult(await _repository.Get(paymentId));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody]CreatePayment model)
        {
            var meterRead = await _serviceClient.GetMeterRead(model.MeterReadId);

            await _paymentDomainService.CreatePayment(model.MeterReadId, model.AddressId, meterRead.Usages.Select(Convert).ToList());

            return StatusCode(200, model.PaymentId);
        }

        [HttpPut("cancel/{paymentId}")]
        public async Task<IActionResult> CancelPayment(Guid paymentId)
        {
            var payment = await _repository.Get(paymentId);

            payment.CancelPayment();

            _repository.Update(payment);

            return StatusCode(204);
        }

        private static RegisteredUsage Convert(Usage dto)
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