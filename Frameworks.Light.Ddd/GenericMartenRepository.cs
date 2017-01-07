﻿using System;
using System.Threading.Tasks;
using Marten;
using MassTransit;

namespace Frameworks.Light.Ddd
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
            var document = _session.Load<TEntity>(id);

            return Task.FromResult(document);
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