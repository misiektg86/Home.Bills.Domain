namespace Frameworks.Light.Ddd
{
    public interface IAggregateFactory<out TAggregateRoot, TEntityId> where TAggregateRoot : AggregateRoot<TEntityId>
    {
        TAggregateRoot Create();
    }
}