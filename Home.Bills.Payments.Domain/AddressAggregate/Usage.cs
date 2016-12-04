using System;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    public class Usage : ICloneable, IPayment
    {
        public Usage(Guid id, double value, string meterSerialNumber, DateTime registeredDate, decimal amountToPay)
        {
            Id = id;
            Value = value;
            MeterSerialNumber = meterSerialNumber;
            RegisteredDate = registeredDate;
            AmountToPay = amountToPay;
        }

        public Guid Id { get; }

        public double Value { get; }

        public string MeterSerialNumber { get; }

        public DateTime RegisteredDate { get; }

        public decimal AmountToPay { get; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public interface IPayment
    {
        Guid Id { get; }

        decimal AmountToPay { get; }
    }
}