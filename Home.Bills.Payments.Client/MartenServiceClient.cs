using System;
using System.Threading.Tasks;
using Home.Bills.Payments.DataAccess.Dtos;
using Marten;

namespace Home.Bills.Payments.Client
{
    internal class MartenServiceClient : IServiceClient
    {
        private readonly IPaymentsDataProvider _paymentsDataProvider;

        public MartenServiceClient(IPaymentsDataProvider paymentsDataProvider)
        {
            _paymentsDataProvider = paymentsDataProvider;
        }    

        public Task<Payment> GetPayment(Guid paymentId)
        {
            return _paymentsDataProvider.GetPayment(paymentId);
        }
    }
}