using System;
using System.Runtime.Serialization;

namespace Home.Bills.Notifications.Domain.AddressAggregate
{
    public class PaymentAlreadyRegisteredException : Exception
    {
        public PaymentAlreadyRegisteredException()
        {
        }

        public PaymentAlreadyRegisteredException(string message) : base(message)
        {
        }

        public PaymentAlreadyRegisteredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PaymentAlreadyRegisteredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}