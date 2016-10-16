﻿using System;
using System.Collections.Generic;
using System.Linq;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.ValueObjects;

namespace Home.Bills.Domain.AddressAggregate.Entities
{
    public class Address : AggregateRoot<Guid>
    {
        private readonly List<Meter> _meters;

        private readonly List<Usage> _usages;

        private readonly AddressInformation _addressInformation;

        public AddressInformation Information => _addressInformation.Clone();

        private Address(string street, string city, string stretNumber, string homeNumber, List<Meter> meters, List<Usage> usages, Guid id)
        {
            Id = id;
            _addressInformation = new AddressInformation(street, city, stretNumber, homeNumber);
            _meters = meters;
            _usages = usages;
        }

        public static Address Create(string street, string city, string stretNumber, string homeNumber)
        {
            return new Address(street, city, stretNumber, homeNumber, new List<Meter>(), new List<Usage>(), Guid.NewGuid());
        }

        public void ProvideRead(double read, string meterSerialNumber, DateTime readDateTime)
        {
            var meter = _meters.First(i => i.Id == meterSerialNumber);

            var oldState = meter.State;

            var usage = Usage.Create(meterSerialNumber, oldState, read, readDateTime);

            _usages.Add(usage);

            meter.State = read;
        }

        public void AddMeter(string meterSerialNumber, double state)
        {
            if (_meters.Any(i => i.Id == meterSerialNumber))
            {
                throw new InvalidOperationException("Meter with serial number already exists");
            }

            _meters.Add(new Meter(new MeterId(meterSerialNumber)) { State = state });
        }

        public void ExchangeMeter(string meterSerialNumber, string newMeterSerialNumber, double state)
        {
            if (_meters.All(i => i.Id != meterSerialNumber))
            {
                throw new InvalidOperationException("Meter with serial number doesn't exists");
            }

            _meters.Remove(_meters.Find(i => i.Id == meterSerialNumber));

            AddMeter(newMeterSerialNumber, state);
        }

        public IEnumerable<Meter> GetMeters()
        {
            return _meters.Select(i => (Meter)i.Clone()).ToList();
        }

        public IEnumerable<Usage> GetUsages()
        {
            return _usages.AsReadOnly();
        }
    }
}