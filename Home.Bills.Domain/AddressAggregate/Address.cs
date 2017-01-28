using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.Events;
using Home.Bills.Domain.AddressAggregate.Exceptions;
using Home.Bills.Domain.AddressAggregate.ValueObjects;
using Newtonsoft.Json;

namespace Home.Bills.Domain.AddressAggregate
{
    public class Address : AggregateRoot<Guid>
    {
        private List<Guid> _meters;

        private AddressInformation _addressInformation;

        public int CheckedInPersons { get; private set; }

        public Guid? LastFinishedMeterReadProcess { get; private set; }

        [JsonIgnore]
        public AddressInformation Information => _addressInformation?.Clone();

        [JsonIgnore]
        public Guid? MeterReadId => _meterReadId;

        [JsonIgnore]
        public List<Guid> Meters => _meters;

        private Guid? _meterReadId;

        internal Address() { }

        internal Address(string street, string city, string stretNumber, string homeNumber, Guid id, double squareMeters)
        {
            Id = id;
            _addressInformation = new AddressInformation(street, city, stretNumber, homeNumber, id, squareMeters);
            _meters = new List<Guid>();

            Publish(new AddressCreated(Id, squareMeters));
        }

        public void BeginMeterReadProcess(Guid meterReadId)
        {
            if (_meterReadId.HasValue)
            {
                throw new ActiveReadInProgressException(meterReadId.ToString());
            }

            _meterReadId = meterReadId;

            Publish(new MeterReadProcessBagan { AddressId = Id, MeterReadId = meterReadId, MeterIds = _meters.ToList(), ReadProcessStartDate = DateTime.Now });
        }

        public void FinishMeterReadProcess(Guid meterReadId)
        {
            if (_meterReadId != meterReadId)
            {
                throw new ActiveReadDoesnotExistException(meterReadId.ToString());
            }

            LastFinishedMeterReadProcess = _meterReadId;

            _meterReadId = null;

            Publish(new MeterReadProcessFinished(meterReadId, Id));
        }

        public void CancelMeterReadProcess(Guid meterReadId)
        {
            if (_meterReadId != meterReadId)
            {
                throw new ActiveReadDoesnotExistException(meterReadId.ToString());
            }

            _meterReadId = null;

            Publish(new MeterReadProcessCanceled(meterReadId, Id));
        }

        public void AssignMeter(Guid meterId)
        {
            if (_meters.Any(i => i == meterId))
            {
                throw new InvalidOperationException("Meter with serial number already exists");
            }

            _meters.Add(meterId);

            Publish(new MeterAssigned(meterId, Id));
        }

        public void CheckInPersons(int persons)
        {
            CheckedInPersons += persons;

            Publish(new PersonsCheckedIn(Id, persons));
            Publish(new PersonsStatusChanged(Id, CheckedInPersons));
        }

        public void CheckOutPersons(int persons)
        {
            if (CheckedInPersons < persons)
            {
                throw new CannotCheckOutMorePersonsThanCheckedInException();
            }

            CheckedInPersons -= persons;

            Publish(new PersonsCheckedOut(Id, persons));
            Publish(new PersonsStatusChanged(Id, CheckedInPersons));
        }

        public void RemoveMeter(Guid meterId)
        {
            if (_meters.All(i => i != meterId))
            {
                throw new InvalidOperationException("Meter with serial number doesn't exists");
            }

            _meters.Remove(_meters.Find(i => i == meterId));

            _meters.Remove(meterId);

            Publish(new MeterRemoved() { MeterId = meterId, AddressId = Id });
        }

        public IEnumerable<Guid> GetMeters()
        {
            return _meters.ToList();
        }

        public void SetLastMeterRead(Guid? meterReadId)
        {
            if (_meterReadId.HasValue)
            {
                throw new ActiveReadInProgressException(_meterReadId.ToString());
            }

            Publish(new LastMeterReadChanged() {AddressId = Id, OldMeterRead = LastFinishedMeterReadProcess, NewMeterRead = meterReadId, ChangedDateTime = DateTime.Now});

            LastFinishedMeterReadProcess = meterReadId;
        }
    }
}