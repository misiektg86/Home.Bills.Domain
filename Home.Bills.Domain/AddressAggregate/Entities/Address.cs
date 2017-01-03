using System;
using System.Collections.Generic;
using System.Linq;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.Events;
using Home.Bills.Domain.AddressAggregate.Exceptions;
using Home.Bills.Domain.AddressAggregate.ValueObjects;
using MassTransit;
using Newtonsoft.Json;

namespace Home.Bills.Domain.AddressAggregate.Entities
{
    public class Address : AggregateRoot<Guid>
    {
        private List<Guid> _meters;
        
        private AddressInformation _addressInformation;

        public int CheckedInPersons { get; private set; }

        [JsonIgnore]
        public AddressInformation Information => _addressInformation?.Clone();

        private Guid? _activeReadId;

        internal Address() { }

        internal Address(IBus messageBus)
        {
            MessageBus = messageBus;
        }

        internal Address(string street, string city, string stretNumber, string homeNumber, List<Guid> meters, Guid id, IBus messageBus, double squareMeters) : this(messageBus)
        {
            Id = id;
            _addressInformation = new AddressInformation(street, city, stretNumber, homeNumber, id, squareMeters);
            _meters = meters;

            MessageBus.Publish(new AddressCreated(Id, squareMeters));
        }

        public void BeginMeterReadProcess(Guid activeReadId)
        {
            if (_activeReadId.HasValue)
            {
                throw new ActiveReadInProgressException(activeReadId.ToString());
            }

            _activeReadId = activeReadId;

            MessageBus.Publish(new MeterReadProcessBagan {AddressId = Id,Id = activeReadId,MeterSerialNumbers = _meters.ToList()});
        }

        public void FinishMeterReadProcess(Guid activeReadId)
        {
            if (_activeReadId != activeReadId)
            {
                throw new ActiveReadDoesnotExistException(activeReadId.ToString());
            }

            _activeReadId = null;
        }

        public void AssignMeter(Guid meterId)
        {
            if (_meters.Any(i => i == meterId))
            {
                throw new InvalidOperationException("Meter with serial number already exists");
            }

            _meters.Add(meterId);

            MessageBus.Publish(new MeterAssigned(meterId, Id));
        }

        public void CheckInPersons(int persons)
        {
            CheckedInPersons += persons;

            MessageBus.Publish(new PersonsCheckedIn(Id, persons));
            MessageBus.Publish(new PersonsStatusChanged(Id, CheckedInPersons));
        }

        public void CheckOutPersons(int persons)
        {
            if (CheckedInPersons < persons)
            {
                throw new CannotCheckOutMorePersonsThanCheckedInException();
            }

            CheckedInPersons -= persons;

            MessageBus.Publish(new PersonsCheckedOut(Id, persons));
            MessageBus.Publish(new PersonsStatusChanged(Id, CheckedInPersons));
        }

        public void RemoveMeter(Guid meterId)
        {
            if (_meters.All(i => i != meterId))
            {
                throw new InvalidOperationException("Meter with serial number doesn't exists");
            }

            _meters.Remove(_meters.Find(i => i == meterId));

            _meters.Remove(meterId);

            MessageBus.Publish(new MeterRemoved() {MeterId = meterId, AddressId = Id});
        }

        public IEnumerable<Guid> GetMeters()
        {
            return _meters.ToList();
        }
    }
}