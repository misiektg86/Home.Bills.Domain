using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Marten;
using MediatR;

namespace Home.Bills.Infrastructure
{
    public class GenericMartenRepository<TEntity> : IRepository<TEntity, Guid> where TEntity : AggregateRoot<Guid>
    {
        private readonly IDocumentSession _session;
        private readonly IMediator _mediator;

        public GenericMartenRepository(IDocumentSession session, IMediator mediator)
        {
            _session = session;
            _mediator = mediator;
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
            var mediatorProperty = entity.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(i => i.PropertyType == typeof(IMediator));
            if (mediatorProperty == null)
            {
                return;
            }

            mediatorProperty.SetValue(entity, _mediator);
        }
    }
}