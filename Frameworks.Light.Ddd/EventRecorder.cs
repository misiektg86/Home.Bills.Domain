using System.Collections.Generic;
using System.Linq;

namespace Frameworks.Light.Ddd
{
    public class EventRecorder
    {
        List<object> _events = new List<object>();

        public void Record(object @event) => _events.Add(@event);

        public IEnumerable<object> GetEvents() => _events.ToList();
    }
}