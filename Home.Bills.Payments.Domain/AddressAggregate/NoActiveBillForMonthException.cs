using System;
using System.Runtime.Serialization;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    public class NoActiveBillForMonthException : Exception
    {
        public NoActiveBillForMonthException()
        {
        }

        public NoActiveBillForMonthException(string message) : base(message)
        {
        }

        public NoActiveBillForMonthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoActiveBillForMonthException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}