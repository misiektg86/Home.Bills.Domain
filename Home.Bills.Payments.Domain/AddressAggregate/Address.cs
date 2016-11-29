using System;
using Frameworks.Light.Ddd;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    public class Address : AggregateRoot<Guid>
    {
        internal Address() { }

        internal Address(Guid id)
        {
            Id = id;
        }

        public void AssignMeterToTariff(string meterSerialNumber, Tariff tariff)
        {
            throw new NotImplementedException();
        }

        public void RegisterUsage(string meterSerialNumber, double value)
        {
            throw new NotImplementedException();
        }

        public void AcceptUsageForMonth()
        {
            throw new NotImplementedException();
        }
    }

    public class Tariff
    {
    }
}