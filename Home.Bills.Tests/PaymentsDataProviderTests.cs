﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Home.Bills.Infrastructure;
using Home.Bills.Payments.Domain;
using Marten;
using Xunit;

namespace Home.Bills.Tests
{
    public class PaymentsDataProviderTests : IClassFixture<MartenDatabaseFixture>
    {
        private IDocumentSession _documentSession;

        public PaymentsDataProviderTests(MartenDatabaseFixture martenDatabaseFixture)
        {
            _documentSession = martenDatabaseFixture.DocumentStore.OpenSession();
        }

        [Fact]
        public async Task ShouldGetPaymentsForAddress()
        {
            var paymentsRepo = new GenericMartenRepository<Payment>(_documentSession);

            Guid addressId = Guid.NewGuid();

            var payment = Payment.Create(addressId, 100m, "Test payment");

            paymentsRepo.Add(payment);

            await _documentSession.SaveChangesAsync();

            var paymentsProvider = new PaymentsDataProvider(_documentSession);

            var payments = await paymentsProvider.GetAllPaymentsForAddress(addressId);

            Assert.Contains(payment.Id, payments.Select(i => i.PaymentId));
        }
    }
}