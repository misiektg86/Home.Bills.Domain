using System;

namespace Home.Bills.Dtos
{
    public struct Payment
    {
        public Guid PaymentId { get; set; }

        public bool Paid { get; set; }

        public DateTime? PaidDate { get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }

        public decimal ToPay { get; set; }

        public static explicit operator Payment(Payments.Domain.PaymentInformation source)
        {
            return new Payment
            {
                Created = source.Created,
                Description = source.Description,
                Paid = source.Paid,
                PaidDate = source.PaidDate,
                PaymentId = source.PaymentId,
                ToPay = source.ToPay
            };
        }
    }
}