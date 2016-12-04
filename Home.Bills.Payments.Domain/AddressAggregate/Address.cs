using System;
using System.Collections.Generic;
using System.Linq;
using Frameworks.Light.Ddd;
using MediatR;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    public class Address : AggregateRoot<Guid>
    {
        public IMediator Mediator { get; }

        internal Address() { }

        private Dictionary<string, decimal> _tariffAssigments;

        private List<Usage> _usages;

        private List<PaymentBundle> _historyBundles;

        private PaymentBundle _paymentBundle;

        private int _persons;

        internal Address(IMediator mediator)
        {
            Mediator = mediator;
        }

        internal Address(Guid id, IMediator mediator) : this(mediator)
        {
            Id = id;
            _tariffAssigments = new Dictionary<string, decimal>();
            _usages = new List<Usage>();
            _paymentBundle = new PaymentBundle(Guid.NewGuid(), DateTime.Now.Month, DateTime.Now.Year);
            _historyBundles = new List<PaymentBundle>();
        }

        public static Address Create(Guid id)
        {
            return new Address(id, new Mediator(type => type, type => Enumerable.Empty<Type>()));
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
            if (!_tariffAssigments.ContainsKey(meterSerialNumber))
            {
                throw new TariffNotAssignedException();
            }

            if (_paymentBundle.Accepted)
            {
                _paymentBundle = new PaymentBundle(Guid.NewGuid(), DateTime.Now.Month, DateTime.Now.Year);
            }

            var tariff = _tariffAssigments[meterSerialNumber];

            var amountToPay = tariff * new decimal(value);

            var usage = new Usage(Guid.NewGuid(), value, meterSerialNumber, DateTime.Now, amountToPay);

            _usages.Add(usage);

            _paymentBundle.AddBundleItem(usage.Id);
        }

        public void AcceptUsageForMonth()
        {
            if (_paymentBundle.Accepted)
            {
                throw new BillForMonthCurrentlyAcceptedException();
            }

            _paymentBundle.Accept();

            _historyBundles.Add(_paymentBundle);
        }

        public decimal GetTariff(string meterSerialNumber)
        {
            return _tariffAssigments[meterSerialNumber];
        }

        public IEnumerable<Usage> GetUsages()
        {
            return _usages.Select(i => (Usage)i.Clone()).ToList();
        }

        public void UpdatePersons(int persons)
        {
            _persons = persons;
        }
    }

    public interface IApartmentRentCalculator
    {
        decimal Calculate(int persons);
    }
}