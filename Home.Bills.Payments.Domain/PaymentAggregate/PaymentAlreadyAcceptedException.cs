using System;
using System.Runtime.Serialization;

namespace Home.Bills.Payments.Domain.PaymentAggregate
{
    public class PaymentAlreadyAcceptedException : Exception
    {
        public PaymentAlreadyAcceptedException()
        {
        }

        public PaymentAlreadyAcceptedException(string message) : base(message)
        {
        }

        public PaymentAlreadyAcceptedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PaymentAlreadyAcceptedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}