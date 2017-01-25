using System;
using System.Runtime.Serialization;

namespace Home.Bills.Notifications.Domain.Services
{
    public class PaymentNotFoundException : Exception
    {
        public PaymentNotFoundException()
        {
        }

        public PaymentNotFoundException(string message) : base(message)
        {
        }

        public PaymentNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PaymentNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}