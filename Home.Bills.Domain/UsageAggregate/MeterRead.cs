using System;
using System.Collections.Generic;
using System.Linq;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.Exceptions;

namespace Home.Bills.Domain.UsageAggregate
{
    public class MeterRead : AggregateRoot<Guid>
    {
        private IDictionary<Guid, double> _meters;

        private Guid _addressId;

        internal MeterRead() { }

        private List<Usage> _usages;

        public bool IsCompleted { get; private set; }

        internal MeterRead(Guid id, IDictionary<Guid, double> meters, Guid addressId)
        {
            _meters = meters;
            _addressId = addressId;
            Id = id;
            _usages = new List<Usage>();
        }

        public void ProvideRead(Guid usageId, double read, Guid meterId, DateTime readDateTime)
        {
            if(_usages.Any(i=>i.MeterId == meterId))
            {
                return;
            }

            var meter = _meters.FirstOrDefault(i => i.Key == meterId);

            if (meter.Equals(default(KeyValuePair<Guid, double>)))
            {
                throw new MeterNotFoundException($"Meter with id: {meterId} doesn't exist.");
            }

            Mediator.Publish(new ReadProvided() {OldRead = _meters[meterId], Read = read, MeterId = meterId, AddressId = _addressId, ReadDateTime = DateTime.Now});
        }

        public void CreateUsage(double oldRead, double read, DateTime readDateTime, Guid meterId, Guid usageId)
        {
            var usage = Usage.Create(usageId, meterId, _addressId, _meters[meterId], read, readDateTime, Mediator);

            _usages.Add(usage);

            if (_usages.All(i => _meters.ContainsKey(i.MeterId)))
            {
                IsCompleted = true;

                Mediator.Publish(new MeterReadCompleted() {MeterReadId = Id, AddressId = _addressId});
            }
        }
    }
}