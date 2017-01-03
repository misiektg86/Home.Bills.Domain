using System;
using System.Runtime.Serialization;

namespace Home.Bills.Domain.AddressAggregate.Exceptions
{
    public class ActiveReadDoesnotExistException : Exception
    {
        public ActiveReadDoesnotExistException()
        {
        }

        public ActiveReadDoesnotExistException(string message) : base(message)
        {
        }

        public ActiveReadDoesnotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ActiveReadDoesnotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}