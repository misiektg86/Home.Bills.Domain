﻿using System;

namespace Home.Bills.Domain.AddressAggregate.ValueObjects
{
    public class Usage
    {
        public string MeterSerialNumber { get; }

        public double Value { get; }

        public DateTime ReadDateTime { get; }

        private Usage(string meterSerialNumber, double usage, DateTime readDateTime)
        {
            Value = usage;
            MeterSerialNumber = meterSerialNumber;
            ReadDateTime = readDateTime;
        }

        public static Usage Create(string meterSerialNumber, double previoudRead, double currentRead, DateTime readDateTime)
        {
            if (previoudRead > currentRead)
            {
                throw new InvalidOperationException("Previous read cannot be bigger than current read");
            }

            return new Usage(meterSerialNumber, currentRead - previoudRead, readDateTime);
        }

        internal Usage Clone() => MemberwiseClone() as Usage;
    }
}