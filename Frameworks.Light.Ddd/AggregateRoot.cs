using MassTransit;

namespace Frameworks.Light.Ddd
{
    public class AggregateRoot<TEntityId> : Entity<TEntityId>
    {
        protected AggregateRoot() { }

        protected AggregateRoot(IBus messageBus)
        {
            MessageBus = messageBus;
        }
    }
}