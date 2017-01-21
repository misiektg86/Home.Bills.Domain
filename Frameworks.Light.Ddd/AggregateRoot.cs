using System.Collections.Generic;
using Newtonsoft.Json;

namespace Frameworks.Light.Ddd
{
    public class AggregateRoot<TEntityId> : Entity<TEntityId>, IEventTracking
    {
        protected AggregateRoot()
        {
            _recorder = new EventRecorder();
        }

        [JsonIgnore]
        private EventRecorder _recorder;

        protected void Publish<T>(T message) where T : class
        {
            _recorder.Record(message);
        }

        public IEnumerable<object> GetEvents() => _recorder.GetEvents();
    }
}