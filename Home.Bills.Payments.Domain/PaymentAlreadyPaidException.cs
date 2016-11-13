using System;
using System.Runtime.Serialization;

namespace Home.Bills.Payments.Domain
{
    public class PaymentAlreadyPaidException : Exception
    {
        public PaymentAlreadyPaidException()
        {
        }

        public PaymentAlreadyPaidException(string message) : base(message)
        {
        }

        public PaymentAlreadyPaidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PaymentAlreadyPaidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}