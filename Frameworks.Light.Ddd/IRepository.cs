namespace Frameworks.Light.Ddd
{
    public interface IRepository<TEntity, in TEntityId> where TEntity : AggregateRoot<TEntityId>
    {
        TEntity Get(TEntityId id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntityId id);
    }
}