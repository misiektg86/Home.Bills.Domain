using System;
using System.Collections.Generic;
using System.Linq;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.Exceptions;

namespace Home.Bills.Domain.MeterReadAggregate
{
    public class MeterRead : AggregateRoot<Guid>
    {
        public DateTime ReadBeginDateTime { get; }

        public IEnumerable<Guid> Meters { get; }

        public Guid AddressId { get; }

        internal MeterRead() { }

        private List<Usage> _usages;

        public bool IsCompleted { get; private set; }

        public IEnumerable<Usage> Usages
        {
            get { return _usages.Select(i => i.Clone()).ToList(); }
        }

        internal MeterRead(Guid id, IEnumerable<Guid> meters, Guid addressId, DateTime readBeginDateTime)
        {
            ReadBeginDateTime = readBeginDateTime;
            Meters = meters.ToList();
            AddressId = addressId;
            Id = id;
            _usages = new List<Usage>();
        }

        public void ProvideRead(Guid usageId, double previousRead, double read, Guid meterId, DateTime readDateTime)
        {
            if (_usages.Any(i => i.MeterId == meterId))
            {
                return;
            }

            var meter = Meters.FirstOrDefault(i => i == meterId);

            if (meter.Equals(default(Guid)))
            {
                throw new MeterNotFoundException($"Meter with id: {meterId} doesn't exist.");
            }

            MessageBus.Publish(new ReadProvided() { PreviousRead = previousRead, NewRead = read, MeterId = meterId, AddressId = AddressId, ReadDateTime = DateTime.Now });
        }

        public void CreateUsage(double previousRead, double newRead, DateTime readDateTime, Guid meterId, Guid usageId)
        {
            var usage = Usage.Create(usageId, Id, meterId, AddressId, previousRead, newRead, readDateTime, MessageBus);

            _usages.Add(usage);
        }

        public void CompleteMeterRead()
        {
            IsCompleted = true;
        }
    }
}