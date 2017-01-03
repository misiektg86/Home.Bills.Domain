using System;
using System.Runtime.Serialization;

namespace Home.Bills.Domain.MeterAggregate
{
    [Serializable]
    internal class CannotRemoveMeterFromAddressSpecifiedException : Exception
    {
        private Guid id;

        public CannotRemoveMeterFromAddressSpecifiedException()
        {
        }

        public CannotRemoveMeterFromAddressSpecifiedException(string message) : base(message)
        {
        }

        public CannotRemoveMeterFromAddressSpecifiedException(Guid id)
        {
            this.id = id;
        }

        public CannotRemoveMeterFromAddressSpecifiedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CannotRemoveMeterFromAddressSpecifiedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}