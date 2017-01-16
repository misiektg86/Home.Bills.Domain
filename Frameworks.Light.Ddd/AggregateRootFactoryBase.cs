using System;
using System.Runtime.Serialization;
using Autofac;
using MassTransit;

namespace Frameworks.Light.Ddd
{
    public abstract class AggregateRootFactoryBase<TAggregateRoot, TCreateInput, TEntityId> : IAggregateFactory<TAggregateRoot, TCreateInput, TEntityId> where TAggregateRoot : AggregateRoot<TEntityId>
    {
        private Autofac.IContainer Container => AggreagteRootFactoryContainer.Container;

        public TAggregateRoot Create(TCreateInput input)
        {
            if (Container == null)
            {
                throw new AggregateRootFactoryContainerNotSetException("Register container through AggreagteRootFactoryContainer.SetFactoryContainer(container) static method.");
            }

            var root = CreateInternal(input);

            SetInfrastructure(root);

            return root;
        }

        private void SetInfrastructure(Entity<TEntityId> entity)
        {
            entity.MessageBus = Container.Resolve<IBus>();
            entity.Recorder = Container.Resolve<IPublishRecorder>();
        }

        protected abstract TAggregateRoot CreateInternal(TCreateInput input);
    }

    public class AggregateRootFactoryContainerNotSetException : Exception
    {
        public AggregateRootFactoryContainerNotSetException()
        {
        }

        public AggregateRootFactoryContainerNotSetException(string message) : base(message)
        {
        }

        public AggregateRootFactoryContainerNotSetException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AggregateRootFactoryContainerNotSetException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public static class AggreagteRootFactoryContainer
    {
        public static Autofac.IContainer Container { get; private set; }

        public static void SetFactoryContainer(Autofac.IContainer container)
        {
            Container = container;
        }
    }
}