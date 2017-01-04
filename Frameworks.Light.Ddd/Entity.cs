using System.Threading.Tasks;
using MassTransit;
using MassTransit.Util;
using Newtonsoft.Json;

namespace Frameworks.Light.Ddd
{
    public class Entity<TEntityId>
    {
        public TEntityId Id { get; protected set; }

        [JsonIgnore]
        protected IBus MessageBus { get; set; }

        protected void Await(Task task)
        {
            TaskUtil.Await(task);
        }

        protected void Publish<T>(T message) where T : class
        {
            Await(MessageBus.Publish(message));
        }
    }
}