using System;
using System.Runtime.Serialization;

namespace Home.Bills.Payments.Domain.Services
{
    public class RentNotFoundException : Exception
    {
        public RentNotFoundException()
        {
        }

        public RentNotFoundException(string message) : base(message)
        {
        }

        public RentNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RentNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}