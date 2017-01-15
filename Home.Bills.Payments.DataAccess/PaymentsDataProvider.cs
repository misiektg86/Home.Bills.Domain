﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Home.Bills.Payments.DataAccess.Dtos;
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

        public async Task<IEnumerable<Payment>> GetPayments(Guid addressId)
        {         
                var payments = 
                (await _documentSession.Query<Domain.PaymentAggregate.Payment>()
                    .Where(i => i.AddressId == addressId)
                    .ToListAsync()).Select(ToDto).ToList();
                return payments;
        }

        private Payment ToDto(Domain.PaymentAggregate.Payment source)
        {
            return new Payment()
            {
                Accepted = source.Accepted,
                AddressId = source.AddressId,
                Canceled = source.Canceled,
                PaymentItems = source.PaymentItems?.Select(ToDto).ToList(),
                Setteled = source.Setteled
            };
        }

        private PaymentItem ToDto(Domain.PaymentAggregate.PaymentItem source)
        {
            return new PaymentItem
            {
                Amount = source.Amount,
                Description = source.Description
            };
        }
    }
}