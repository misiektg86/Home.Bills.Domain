using System.Threading.Tasks;
using Marten;

namespace Frameworks.Light.Ddd
{
    public class AsyncUnitOfWork : IAsyncUnitOfWork
    {
        private readonly IDocumentSession _documentSession;
        private readonly IPublishRecorder _publishRecorder;

        public AsyncUnitOfWork(IDocumentSession documentSession, IPublishRecorder publishRecorder)
        {
            _documentSession = documentSession;
            _publishRecorder = publishRecorder;
        }

        public async Task CommitAsync()
        {
            await _documentSession.SaveChangesAsync();

            _publishRecorder.Play();
        }
    }
}