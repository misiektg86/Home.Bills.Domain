using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Home.Bills.Payments.DataAccess.Dtos
{
    public interface IPaymentsDataProvider
    {
        Task<IEnumerable<Payment>> GetPayments(Guid addressId);

        Task<Payment> GetPayment(Guid paymentId);
    }
}