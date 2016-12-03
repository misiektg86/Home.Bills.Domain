using System;
using System.Collections.Generic;
using Frameworks.Light.Ddd;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    public class Address : AggregateRoot<Guid>
    {
        internal Address() { }

        private Dictionary<string, decimal> _tariffAssigments;

        internal Address(Guid id)
        {
            Id = id;
            _tariffAssigments = new Dictionary<string, decimal>();
        }

        public static Address Create(Guid id)
        {
            return new Address(id);
        }

        public void ApplyTariff(string meterSerialNumber, decimal tariff)
        {
            if (_tariffAssigments.ContainsKey(meterSerialNumber))
            {
                _tariffAssigments[meterSerialNumber] = tariff;
                return;
            }

            _tariffAssigments.Add(meterSerialNumber, tariff);
        }

        public void RegisterUsage(string meterSerialNumber, double value)
        {
            throw new NotImplementedException();
        }

        public void AcceptUsageForMonth()
        {
            throw new NotImplementedException();
        }

        public decimal GetTariff(string meterSerialNumber)
        {
            return _tariffAssigments[meterSerialNumber];
        }
    }
}