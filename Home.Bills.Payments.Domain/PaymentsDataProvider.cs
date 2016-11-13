using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten;

namespace Home.Bills.Payments.Domain
{
    public class PaymentsDataProvider : IPaymentsDataProvider
    {
        private readonly IDocumentSession _documentSession;
        public PaymentsDataProvider(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public async Task<IEnumerable<PaymentInformation>> GetAllPaymentsForAddress(Guid addressId)
        {
            var payments = await _documentSession.Query<Payment>()
                .Where(i => i.AddressId == addressId).ToListAsync();

            return payments.Select(i => i.PaymentInformations).ToList();
        }
    }
}