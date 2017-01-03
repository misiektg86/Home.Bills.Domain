using System;
using System.Runtime.Serialization;

namespace Home.Bills.Domain.MeterAggregate
{
    [Serializable]
    internal class CannotUnmountMeterAtAddressSpecifiedException : Exception
    {
        private Guid id;

        public CannotUnmountMeterAtAddressSpecifiedException()
        {
        }

        public CannotUnmountMeterAtAddressSpecifiedException(string message) : base(message)
        {
        }

        public CannotUnmountMeterAtAddressSpecifiedException(Guid id)
        {
            this.id = id;
        }

        public CannotUnmountMeterAtAddressSpecifiedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CannotUnmountMeterAtAddressSpecifiedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}