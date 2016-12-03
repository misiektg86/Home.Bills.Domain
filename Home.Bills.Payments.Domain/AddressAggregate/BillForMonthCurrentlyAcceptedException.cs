using System;
using System.Runtime.Serialization;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    public class BillForMonthCurrentlyAcceptedException : Exception
    {
        public BillForMonthCurrentlyAcceptedException()
        {
        }

        public BillForMonthCurrentlyAcceptedException(string message) : base(message)
        {
        }

        public BillForMonthCurrentlyAcceptedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BillForMonthCurrentlyAcceptedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}