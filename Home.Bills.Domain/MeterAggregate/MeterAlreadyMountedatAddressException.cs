using System;
using System.Runtime.Serialization;

namespace Home.Bills.Domain.MeterAggregate
{
    [Serializable]
    internal class MeterAlreadyMountedatAddressException : Exception
    {
        public MeterAlreadyMountedatAddressException()
        {
        }

        public MeterAlreadyMountedatAddressException(string message) : base(message)
        {
        }

        public MeterAlreadyMountedatAddressException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MeterAlreadyMountedatAddressException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}