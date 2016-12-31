using MediatR;
using Newtonsoft.Json;

namespace Frameworks.Light.Ddd
{
    public class AggregateRoot<TEntityId> : Entity<TEntityId>
    {
        [JsonIgnore]
        protected IMediator Mediator { get; set; }
    }
}