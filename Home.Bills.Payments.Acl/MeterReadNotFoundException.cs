using System;
using System.Runtime.Serialization;

namespace Home.Bills.Payments.Acl
{
    public class MeterReadNotFoundException : Exception
    {
        public MeterReadNotFoundException()
        {
        }

        public MeterReadNotFoundException(string message) : base(message)
        {
        }

        public MeterReadNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MeterReadNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}