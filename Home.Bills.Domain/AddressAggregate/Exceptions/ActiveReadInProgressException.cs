using System;
using System.Runtime.Serialization;

namespace Home.Bills.Domain.AddressAggregate.Exceptions
{
    public class ActiveReadInProgressException : Exception
    {
        public ActiveReadInProgressException()
        {
        }

        public ActiveReadInProgressException(string message) : base(message)
        {
        }

        public ActiveReadInProgressException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ActiveReadInProgressException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}