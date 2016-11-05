namespace Frameworks.Light.Ddd
{
    public class Entity<TEntityId>
    {
        public TEntityId Id { get; protected set; }
    }
}