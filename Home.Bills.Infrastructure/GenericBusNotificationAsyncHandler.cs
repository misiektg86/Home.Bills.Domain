using System.Threading.Tasks;
using MassTransit;
using MediatR;

namespace Home.Bills.Infrastructure
{
    public class GenericBusNotificationAsyncHandler : IAsyncNotificationHandler<IAsyncNotification>
    {
        private readonly IBus _bus;

        public GenericBusNotificationAsyncHandler(IBus bus)
        {
            _bus = bus;
        }

        public Task Handle(IAsyncNotification notification)
        {
            return _bus.Publish(notification);
        }
    }
}