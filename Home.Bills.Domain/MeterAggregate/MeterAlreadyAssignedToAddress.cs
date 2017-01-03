using System;
using System.Runtime.Serialization;

namespace Home.Bills.Domain.MeterAggregate
{
    [Serializable]
    internal class MeterAlreadyAssignedToAddressException : Exception
    {
        public MeterAlreadyAssignedToAddressException()
        {
        }

        public MeterAlreadyAssignedToAddressException(string message) : base(message)
        {
        }

        public MeterAlreadyAssignedToAddressException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MeterAlreadyAssignedToAddressException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}