using System;
using System.Runtime.Serialization;

namespace Home.Bills.Payments.Domain.RentAggregate
{
    public class RentHasExpiredException : Exception
    {
        public RentHasExpiredException()
        {
        }

        public RentHasExpiredException(string message) : base(message)
        {
        }

        public RentHasExpiredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RentHasExpiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}