using System;
using System.Collections.Generic;
using System.Linq;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.Exceptions;
using Newtonsoft.Json;

namespace Home.Bills.Domain.MeterReadAggregate
{
    public class MeterRead : AggregateRoot<Guid>
    {
        public DateTime ReadBeginDateTime { get; private set; }

        public IEnumerable<Guid> Meters { get; private set; }

        public Guid AddressId { get; private set; }

        internal MeterRead()
        {
            _usages = new List<Usage>();
            Meters = new List<Guid>();
        }

        private List<Usage> _usages;

        public bool IsCompleted { get; private set; }

        [JsonIgnore]
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

        public Usage CreateUsage(double previousRead, double newRead, DateTime readDateTime, Guid meterId, Guid usageId)
        {
            if (_usages.Any(i => i.Id == usageId))
            {
                return _usages.First(i => i.Id == usageId);
            }

            var usage = Usage.Create(usageId, Id, meterId, AddressId, previousRead, newRead, readDateTime, MessageBus);

            _usages.Add(usage);

            return usage;
        }

        public void CompleteMeterRead()
        {
            IsCompleted = true;
        }
    }
}