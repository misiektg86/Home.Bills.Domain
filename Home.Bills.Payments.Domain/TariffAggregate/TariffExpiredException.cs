using System;
using System.Runtime.Serialization;

namespace Home.Bills.Payments.Domain.TariffAggregate
{
    public class TariffExpiredException : Exception
    {
        public TariffExpiredException()
        {
        }

        public TariffExpiredException(string message) : base(message)
        {
        }

        public TariffExpiredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TariffExpiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}