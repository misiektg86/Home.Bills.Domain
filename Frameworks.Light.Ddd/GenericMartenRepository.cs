using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Marten;
using MassTransit;

namespace Frameworks.Light.Ddd
{
    public class GenericMartenRepository<TEntity> : IRepository<TEntity, Guid> where TEntity : AggregateRoot<Guid>
    {
        private readonly IDocumentSession _session;
        private readonly IBus _messageBus;

        public GenericMartenRepository(IDocumentSession session, IBus messageBus)
        {
            _session = session;
            _messageBus = messageBus;
        }

        public async Task<TEntity> Get(Guid id)
        {
            var document = await _session.LoadAsync<TEntity>(id);

            if (document == null)
            {
                return null;
            }

            SetMediator(document);

            return document;
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

        private void SetMediator(TEntity entity) // change to Marten listener or Newtonsoft.Json configuration
        {
            var busProperty = entity.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(i => i.PropertyType == typeof(IBus));
            if (busProperty == null)
            {
                return;
            }

            busProperty.SetValue(entity, _messageBus);
        }
    }
}