using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Controllers;
using Home.Bills.Infrastructure;
using Home.Bills.Payments.Domain;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using Payment = Home.Bills.Dtos.Payment;

namespace Home.Bills.Tests
{
    public class PaymentControllerTests
    {
        [Fact]
        public async Task ShouldGetPayments()
        {
            var paymentsDataProvider = Substitute.For<IPaymentsDataProvider>();

            var paymentsRepository = Substitute.For<IRepository<Payments.Domain.Payment, Guid>>();

            var paymentController = new PaymentController(paymentsDataProvider, paymentsRepository);

            Guid addressId = Guid.NewGuid();
            Guid paymentId = Guid.NewGuid();

            paymentsDataProvider.GetAllPaymentsForAddress(addressId)
                .Returns(
                    Task.FromResult(
                        new List<PaymentInformation>()
                        {
                            new PaymentInformation(paymentId, true, DateTime.Now, "test", addressId, 100m,
                                DateTime.Now)
                        }.AsEnumerable()));

            var result = await paymentController.GetPayments(addressId) as ObjectResult;

            var payments = result.Value as IEnumerable<Payment>;

            Assert.Contains(paymentId, payments.Select(i => i.PaymentId));
        }

        [Fact]
        public async Task ShouldAddNewPaymentIfNotActivePaymentExists()
        {
            var paymentsDataProvider = Substitute.For<IPaymentsDataProvider>();

            var paymentsRepository = Substitute.For<IRepository<Payments.Domain.Payment, Guid>>();

            var paymentController = new PaymentController(paymentsDataProvider, paymentsRepository);

            Guid addressId = Guid.NewGuid();
            Guid paymentId = Guid.NewGuid();

            paymentsDataProvider.ActivePaymentExists(addressId)
                .Returns(
                    Task.FromResult(false));

            ObjectResult result = await paymentController.CreatePayment(paymentId, 100m, "test") as ObjectResult;

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task ShouldNotAddNewPaymentIfActivePaymentExists()
        {
            var paymentsDataProvider = Substitute.For<IPaymentsDataProvider>();

            var paymentsRepository = Substitute.For<IRepository<Payments.Domain.Payment, Guid>>();

            var paymentController = new PaymentController(paymentsDataProvider, paymentsRepository);

            Guid addressId = Guid.NewGuid();

            paymentsDataProvider.ActivePaymentExists(addressId)
                .Returns(
                    Task.FromResult(true));

            StatusCodeResult result = await paymentController.CreatePayment(addressId, 100m, "test") as StatusCodeResult;

            Assert.Equal((int)HttpStatusCode.Conflict, result.StatusCode);
        }


        //[Fact]
        //public async Task ShouldPayPayment()
        //{
        //    var paymentsDataProvider = Substitute.For<IPaymentsDataProvider>();

        //    var paymentsRepository = Substitute.For<IRepository<Payments.Domain.Payment, Guid>>();

        //    var paymentController = new PaymentController(paymentsDataProvider, paymentsRepository);

        //    Guid payment = Guid.NewGuid();

            

        //    StatusCodeResult result = await paymentController.CreatePayment(addressId, 100m, "test") as StatusCodeResult;

        //    Assert.Equal((int)HttpStatusCode.Conflict, result.StatusCode);
        //}
    }
}