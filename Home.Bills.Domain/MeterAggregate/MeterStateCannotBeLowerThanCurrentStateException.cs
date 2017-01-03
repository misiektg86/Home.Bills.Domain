using System;
using System.Runtime.Serialization;

namespace Home.Bills.Domain.MeterAggregate
{
    [Serializable]
    internal class MeterStateCannotBeLowerThanCurrentStateException : Exception
    {
        public MeterStateCannotBeLowerThanCurrentStateException()
        {
        }

        public MeterStateCannotBeLowerThanCurrentStateException(string message) : base(message)
        {
        }

        public MeterStateCannotBeLowerThanCurrentStateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MeterStateCannotBeLowerThanCurrentStateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}