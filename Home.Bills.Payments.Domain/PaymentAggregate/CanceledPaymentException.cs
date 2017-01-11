using System;
using System.Runtime.Serialization;

namespace Home.Bills.Payments.Domain.PaymentAggregate
{
    public class CanceledPaymentException : Exception
    {
        public CanceledPaymentException()
        {
        }

        public CanceledPaymentException(string message) : base(message)
        {
        }

        public CanceledPaymentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CanceledPaymentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}