using System;
using System.Runtime.Serialization;

namespace Home.Bills.Notifications.Domain.Services
{
    public class AddressCannotBeEmptyException : Exception
    {
        public AddressCannotBeEmptyException()
        {
        }

        public AddressCannotBeEmptyException(string message) : base(message)
        {
        }

        public AddressCannotBeEmptyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AddressCannotBeEmptyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}