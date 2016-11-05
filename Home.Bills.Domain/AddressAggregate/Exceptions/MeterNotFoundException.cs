using System;
using System.Runtime.Serialization;

namespace Home.Bills.Domain.AddressAggregate.Exceptions
{
    public class MeterNotFoundException : Exception
    {
        public MeterNotFoundException()
        {
        }

        public MeterNotFoundException(string message) : base(message)
        {
        }

        public MeterNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MeterNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}