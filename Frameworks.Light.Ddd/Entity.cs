using System.Threading.Tasks;
using MassTransit;
using MassTransit.Util;
using Newtonsoft.Json;

namespace Frameworks.Light.Ddd
{
    public class Entity<TEntityId>
    {
        public TEntityId Id { get; protected set; }
    }
}