using MassTransit;
using MediatR;

namespace Home.Bills.Infrastructure
{
    public class GenericBusNotificationHandler : INotificationHandler<INotification>
    {
        private readonly IBus _bus;

        public GenericBusNotificationHandler(IBus bus)
        {
            _bus = bus;
        }

        public async void Handle(INotification notification)
        {
            await _bus.Publish(notification);
        }
    }
}