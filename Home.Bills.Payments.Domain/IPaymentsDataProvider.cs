using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Home.Bills.Payments.Domain
{
    public interface IPaymentsDataProvider
    {
        Task<IEnumerable<PaymentInformation>> GetAllPaymentsForAddress(Guid addressId);
    }
}