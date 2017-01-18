using System.Collections.Generic;

namespace Frameworks.Light.Ddd
{
    public interface IEventTracking
    {
        IEnumerable<object> GetEvents();
    }
}