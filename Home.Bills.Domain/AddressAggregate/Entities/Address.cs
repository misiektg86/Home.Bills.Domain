using System;
using System.Collections.Generic;
using System.Linq;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.Events;
using Home.Bills.Domain.AddressAggregate.Exceptions;
using Home.Bills.Domain.AddressAggregate.ValueObjects;
using MediatR;
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

        internal Address(IMediator mediator)
        {
            Mediator = mediator;
        }

        internal Address(string street, string city, string stretNumber, string homeNumber, List<Guid> meters, Guid id, IMediator mediator, double squareMeters) : this(mediator)
        {
            Id = id;
            _addressInformation = new AddressInformation(street, city, stretNumber, homeNumber, id, squareMeters);
            _meters = meters;

            Mediator.Publish(new AddressCreated(Id, squareMeters));
        }

        public void BeginMeterReadProcess(Guid activeReadId)
        {
            if (_activeReadId.HasValue)
            {
                throw new ActiveReadInProgressException(activeReadId.ToString());
            }

            _activeReadId = activeReadId;

            Mediator.Publish(new MeterReadProcessBagan {AddressId = Id,Id = activeReadId,MeterSerialNumbers = _meters.ToList()});
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

            Mediator.Publish(new MeterAssigned(meterId, Id));
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

        public void RemoveMeter(Guid meterId)
        {
            if (_meters.All(i => i != meterId))
            {
                throw new InvalidOperationException("Meter with serial number doesn't exists");
            }

            _meters.Remove(_meters.Find(i => i == meterId));

            _meters.Remove(meterId);

            Mediator.Publish(new MeterRemoved() {MeterId = meterId, AddressId = Id});
        }

        public IEnumerable<Guid> GetMeters()
        {
            return _meters.ToList();
        }
    }
}