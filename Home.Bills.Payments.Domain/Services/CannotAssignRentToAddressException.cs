using System;
using System.Runtime.Serialization;

namespace Home.Bills.Payments.Domain.Services
{
    public class CannotAssignRentToAddressException : Exception
    {
        public CannotAssignRentToAddressException()
        {
        }

        public CannotAssignRentToAddressException(string message) : base(message)
        {
        }

        public CannotAssignRentToAddressException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CannotAssignRentToAddressException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}