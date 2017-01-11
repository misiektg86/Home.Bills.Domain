using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Home.Bills.Payments.Domain.Consumers;

namespace Home.Bills.Payments.Domain.Services
{
    public class PaymentDomainService
    {
        public PaymentDomainService() { }

        public Task CreatePayment(Guid messageMeterReadId, Guid messageAddressId, List<RegisteredUsage> toList)
        {
            throw new NotImplementedException();
        }
    }
}