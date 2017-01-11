using Marten;

namespace Home.Bills.Payments.DataAccess
{
    public class PaymentsDataProvider : IPaymentsDataProvider
    {
        private readonly IDocumentSession _documentSession;
        public PaymentsDataProvider(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }
    }
}