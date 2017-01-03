using MassTransit;
using Newtonsoft.Json;

namespace Frameworks.Light.Ddd
{
    public class AggregateRoot<TEntityId> : Entity<TEntityId>
    {
        protected AggregateRoot(){}

        protected AggregateRoot(IBus messageBus)
        {
            MessageBus = messageBus;
        }

        [JsonIgnore]
        protected IBus MessageBus { get; set; }
    }
}