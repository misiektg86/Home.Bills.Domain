using System;
using System.Collections.Generic;
using System.Linq;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.Events;
using Home.Bills.Domain.AddressAggregate.Exceptions;
using Home.Bills.Domain.AddressAggregate.ValueObjects;
using Home.Bills.Domain.UsageAggregate;
using MediatR;
using Newtonsoft.Json;

namespace Home.Bills.Domain.AddressAggregate.Entities
{
    public class Address : AggregateRoot<Guid>
    {
        private List<Meter> _meters;

        private List<Guid> _usages;

        private AddressInformation _addressInformation;

        public int CheckedInPersons { get; private set; }

        [JsonIgnore]
        public AddressInformation Information => _addressInformation?.Clone();

        internal Address() { }

        internal Address(IMediator mediator)
        {
            Mediator = mediator;
        }

        internal Address(string street, string city, string stretNumber, string homeNumber, List<Meter> meters, List<Guid> usages, Guid id, IMediator mediator, double squareMeters) : this(mediator)
        {
            Id = id;
            _addressInformation = new AddressInformation(street, city, stretNumber, homeNumber, id, squareMeters);
            _meters = meters;
            _usages = usages;

            Mediator.Publish(new AddressCreated(Id, squareMeters));
        }

        public Usage ProvideRead(double read, string meterSerialNumber, DateTime readDateTime)
        {
            var meter = _meters.FirstOrDefault(i => i.Id == meterSerialNumber);

            if (meter == null)
            {
                throw new MeterNotFoundException($"Meter with serial number: {meterSerialNumber} doesn't exist.");
            }

            var oldState = meter.State;

            meter.State = read;

            var usage = Usage.Create(meterSerialNumber, Id, oldState, read, readDateTime, Mediator);

            _usages.Add(usage.Id);

            return usage;
        }

        public void AddMeter(string meterSerialNumber, double state)
        {
            if (_meters.Any(i => i.Id == meterSerialNumber))
            {
                throw new InvalidOperationException("Meter with serial number already exists");
            }

            _meters.Add(new Meter(new MeterId(meterSerialNumber)) { State = state });

            Mediator.Publish(new MeterAdded(meterSerialNumber, Id, state));
        }

        public void CheckInPersons(int persons)
        {
            CheckedInPersons += persons;

            Mediator.Publish(new PersonsCheckedIn(Id, persons));
            Mediator.Publish(new PersonsStatusChanged(Id, CheckedInPersons));
        }

        public void CheckOutPersons(int persons)
        {
            if (CheckedInPersons < persons)
            {
                throw new CannotCheckOutMorePersonsThanCheckedInException();
            }

            CheckedInPersons -= persons;

            Mediator.Publish(new PersonsCheckedOut(Id, persons));
            Mediator.Publish(new PersonsStatusChanged(Id, CheckedInPersons));
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
            return _meters.Select(i => i.Clone()).ToList();
        }

        public IEnumerable<Guid> GetUsages()
        {
            return _usages.AsReadOnly();
        }
    }
}