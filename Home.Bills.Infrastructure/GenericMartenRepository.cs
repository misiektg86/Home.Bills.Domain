using System;
using Frameworks.Light.Ddd;

namespace Home.Bills.Infrastructure
{
    public class GenericMartenRepository<TEntity, TEntityId> : IRepository<TEntity, TEntityId> where TEntity : AggregateRoot<TEntityId>
    {

        public void Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntityId id)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(TEntityId id)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}