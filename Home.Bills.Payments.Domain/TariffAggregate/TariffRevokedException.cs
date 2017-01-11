using System;
using System.Runtime.Serialization;

namespace Home.Bills.Payments.Domain.TariffAggregate
{
    public class TariffRevokedException : Exception
    {
        public TariffRevokedException()
        {
        }

        public TariffRevokedException(string message) : base(message)
        {
        }

        public TariffRevokedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TariffRevokedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}