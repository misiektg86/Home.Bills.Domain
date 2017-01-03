using MediatR;
using Newtonsoft.Json;

namespace Frameworks.Light.Ddd
{
    public class AggregateRoot<TEntityId> : Entity<TEntityId>
    {
        protected AggregateRoot(){}

        protected AggregateRoot(IMediator mediator)
        {
            Mediator = mediator;
        }

        [JsonIgnore]
        protected IMediator Mediator { get; set; }
    }
}