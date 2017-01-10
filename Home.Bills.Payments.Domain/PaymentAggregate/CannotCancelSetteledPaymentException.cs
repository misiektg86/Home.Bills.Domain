using System;
using System.Runtime.Serialization;

namespace Home.Bills.Payments.Domain.PaymentAggregate
{
    public class CannotCancelSetteledPaymentException : Exception
    {
        public CannotCancelSetteledPaymentException()
        {
        }

        public CannotCancelSetteledPaymentException(string message) : base(message)
        {
        }

        public CannotCancelSetteledPaymentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CannotCancelSetteledPaymentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}