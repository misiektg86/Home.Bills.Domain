using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Marten;

namespace Home.Bills.Infrastructure
{
    public class GenericMartenRepository<TEntity> : IRepository<TEntity, Guid>, IUnitOfWorkAsync, IDisposable where TEntity : AggregateRoot<Guid>
    {
        private readonly DocumentStore _documentStore;
        private IDocumentSession _session;

        public GenericMartenRepository(DocumentStore documentStore)
        {
            _documentStore = documentStore;
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

        public void Dispose()
        {
            _session.Dispose();
        }

        public Task StartAsync()
        {
            _session = _documentStore.OpenSession();

            return Task.FromResult(0);
        }

        public Task CommitAsync()
        {
            return _session.SaveChangesAsync();
        }
    }

    public interface IUnitOfWorkAsync
    {
        Task StartAsync();
        Task CommitAsync();
    }
}