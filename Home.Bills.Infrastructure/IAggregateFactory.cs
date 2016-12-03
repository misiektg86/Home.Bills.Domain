using Frameworks.Light.Ddd;

namespace Home.Bills.Infrastructure
{
    public interface IAggregateFactory<out TAggregateRoot, in TCreateInput, TEntityId> where TAggregateRoot : AggregateRoot<TEntityId>
    {
        TAggregateRoot Create(TCreateInput input);
    }
}