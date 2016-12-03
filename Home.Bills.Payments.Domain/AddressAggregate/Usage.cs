using System;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    public class Usage
    {
        public Usage(double value, string meterSerialNumber, DateTime registeredDate, decimal amountToPay)
        {
            Value = value;
            MeterSerialNumber = meterSerialNumber;
            RegisteredDate = registeredDate;
            AmountToPay = amountToPay;
        }

        public double Value { get; }

        public string MeterSerialNumber { get; }

        public DateTime RegisteredDate { get; }

        public decimal AmountToPay { get; }
    }
}