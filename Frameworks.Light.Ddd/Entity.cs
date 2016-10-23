namespace Frameworks.Light.Ddd
{
    public class Entity<TEntityId> : Aggregate
    {
        public TEntityId Id { get; protected set; }
    }

    public class Aggregate
    {
    }
}