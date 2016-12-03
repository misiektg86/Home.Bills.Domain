using System;
using System.Runtime.Serialization;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    public class TariffNotAssignedException : Exception
    {
        public TariffNotAssignedException()
        {
        }

        public TariffNotAssignedException(string message) : base(message)
        {
        }

        public TariffNotAssignedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TariffNotAssignedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}