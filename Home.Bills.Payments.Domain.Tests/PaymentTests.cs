using System;
using Xunit;

namespace Home.Bills.Payments.Domain.Tests
{
    public class PaymentTests : IClassFixture<MartenDatabaseFixture>
    {
        private readonly MartenDatabaseFixture _martenDatabaseFixture;

        public PaymentTests(MartenDatabaseFixture martenDatabaseFixture)
        {
            _martenDatabaseFixture = martenDatabaseFixture;
        }

        [Fact]
        public void ShouldCreatePayment()
        {
            var payment = CreatePayment();

            Assert.NotNull(payment);
            Assert.Equal(100m, payment.PaymentInformations.ToPay);
            Assert.Equal("Test payment", payment.PaymentInformations.Description);
            Assert.Equal(DateTime.Now.ToShortDateString(), payment.PaymentInformations.Created.ToShortDateString());
        }

        private static Payment CreatePayment()
        {
            var payment = Payment.Create(Guid.NewGuid(), 100m, "Test payment");
            return payment;
        }

        [Fact]
        public void ShodlBeAbleToPayPayment()
        {
            var payment = CreatePayment();

            payment.Pay();

            Assert.Equal(true, payment.PaymentInformations.Paid);
        }

        [Fact]
        public void ShouldThrowExceptionIfAlreadyPaid()
        {
            var payment = CreatePayment();

            payment.Pay();

            Assert.True(payment.PaymentInformations.Paid);

            Assert.Throws<PaymentAlreadyPaidException>(() => payment.Pay());
        }

        [Fact]
        public void ShouldSetPaidDateforPayment()
        {
            var payment = CreatePayment();

            payment.Pay();

            Assert.NotNull(payment.PaidDate);
        }
    }
}
