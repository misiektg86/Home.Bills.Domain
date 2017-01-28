using System;
using System.Runtime.Serialization;

namespace Home.Bills.Notifications.Domain.PaymentAggregate
{
    public class CannotCreateEmptyPaymentException : Exception
    {
        public CannotCreateEmptyPaymentException()
        {
        }

        public CannotCreateEmptyPaymentException(string message) : base(message)
        {
        }

        public CannotCreateEmptyPaymentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CannotCreateEmptyPaymentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}