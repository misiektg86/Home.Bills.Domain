using System;
using System.Runtime.Serialization;

namespace Home.Bills.Domain.AddressAggregate.Exceptions
{
    public class CannotCheckOutMorePersonsThanCheckedInException : Exception
    {
        public CannotCheckOutMorePersonsThanCheckedInException()
        {
        }

        public CannotCheckOutMorePersonsThanCheckedInException(string message) : base(message)
        {
        }

        public CannotCheckOutMorePersonsThanCheckedInException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CannotCheckOutMorePersonsThanCheckedInException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}