using System;
using System.Threading.Tasks;
using Home.Bills.Payments.DataAccess.Dtos;

namespace Home.Bills.Payments.Client
{
    public interface IServiceClient
    {
        Task<Payment> GetPayment(Guid paymentId);
    }
}
