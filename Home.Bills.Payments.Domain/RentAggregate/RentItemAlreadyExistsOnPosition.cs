using System;
using System.Runtime.Serialization;

namespace Home.Bills.Payments.Domain.RentAggregate
{
    public class RentItemAlreadyExistsOnPosition : Exception
    {
        public RentItemAlreadyExistsOnPosition()
        {
        }

        public RentItemAlreadyExistsOnPosition(string message) : base(message)
        {
        }

        public RentItemAlreadyExistsOnPosition(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RentItemAlreadyExistsOnPosition(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}