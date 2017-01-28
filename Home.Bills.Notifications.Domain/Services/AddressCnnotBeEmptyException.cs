using System;
using System.Runtime.Serialization;

namespace Home.Bills.Notifications.Domain.Services
{
    public class AddressCnnotBeEmptyException : Exception
    {
        public AddressCnnotBeEmptyException()
        {
        }

        public AddressCnnotBeEmptyException(string message) : base(message)
        {
        }

        public AddressCnnotBeEmptyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AddressCnnotBeEmptyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}