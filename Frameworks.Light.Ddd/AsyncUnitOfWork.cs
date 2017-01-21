using System.Linq;
using System.Threading.Tasks;
using Marten;
using MassTransit;

namespace Frameworks.Light.Ddd
{
    public class AsyncUnitOfWork<T> : IAsyncUnitOfWork where T : struct 
    {
        private readonly IDocumentSession _documentSession;
        private readonly IBus _messageBus;

        public AsyncUnitOfWork(IDocumentSession documentSession, IBus messageBus)
        {
            _documentSession = documentSession;
            _messageBus = messageBus;
        }

        public async Task CommitAsync()
        {
            var eventsToPublish = _documentSession.PendingChanges.AllChangedFor<AggregateRoot<T>>().ToList();

            await _documentSession.SaveChangesAsync();

            foreach (var eventToPublish in eventsToPublish.SelectMany(i => i.GetEvents()))
            {
                await _messageBus.Publish(eventToPublish);
            }
        }
    }
}