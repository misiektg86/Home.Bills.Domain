using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Marten;

namespace Home.Bills.Infrastructure
{
    public class GenericMartenRepository<TEntity> : IRepository<TEntity, Guid> where TEntity : AggregateRoot<Guid>
    {
        private readonly IDocumentSession _session;

        public GenericMartenRepository(IDocumentSession session)
        {
            _session = session;
        }

        public Task<TEntity> Get(Guid id)
        {
            return _session.LoadAsync<TEntity>(id);
        }

        public void Add(TEntity entity)
        {
            _session.Store(entity);
        }

        public void Update(TEntity entity)
        {
            _session.Store(entity);
        }

        public void Delete(Guid id)
        {
            _session.Delete<TEntity>(id);
        }
    }
}