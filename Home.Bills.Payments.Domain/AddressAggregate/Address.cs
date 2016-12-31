using System;
using System.Collections.Generic;
using System.Linq;
using Frameworks.Light.Ddd;
using MediatR;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    public class Address : AggregateRoot<Guid>
    {
        private readonly double _squareMeters;

        internal Address() { }

        private Dictionary<string, decimal> _tariffAssigments;

        private List<Usage> _usages;

        private List<PaymentBundle> _historyBundles;

        private PaymentBundle _paymentBundle;

        private int _persons;

        private IRentPolicy _rentforApartmentPolicy;

        private double _squereMeters;

        internal Address(IMediator mediator)
        {
            Mediator = mediator;
        }

        internal Address(Guid id, double squareMeters, IMediator mediator) : this(mediator)
        {
            _squareMeters = squareMeters;
            _rentforApartmentPolicy = new StandardRentForApartmentPolicy();
            Id = id;
            _tariffAssigments = new Dictionary<string, decimal>();
            _usages = new List<Usage>();
            _paymentBundle = new PaymentBundle(Guid.NewGuid(), DateTime.Now.Month, DateTime.Now.Year);
            _historyBundles = new List<PaymentBundle>();
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

            _paymentBundle.AddBundleItem(usage);
        }

        public void AcceptUsageForMonth()
        {
            if (_paymentBundle.Accepted)
            {
                throw new BillForMonthCurrentlyAcceptedException();
            }

            var rent = new Rent(Guid.NewGuid(), _rentforApartmentPolicy.Calculate(_squereMeters, _persons));

            _paymentBundle.AddBundleItem(rent);
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
}