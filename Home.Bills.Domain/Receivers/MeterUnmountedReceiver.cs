using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.Entities;
using Home.Bills.Domain.MeterAggregate;
using Marten;
using MassTransit.Util;
using MediatR;

namespace Home.Bills.Domain.Receivers
{
    internal class MeterUnmountedReceiver : IAsyncNotificationHandler<MeterUnmountedAtAddress>, INotificationHandler<MeterUnmountedAtAddress>
    {
        private readonly IRepository<Address, Guid> _addressRepository;
        private readonly IDocumentSession _documentSession;

        public MeterUnmountedReceiver(IRepository<Address, Guid> addressRepository, IDocumentSession documentSession)
        {
            _addressRepository = addressRepository;
            _documentSession = documentSession;
        }

        public async Task Handle(MeterUnmountedAtAddress notification)
        {
            var address = await _addressRepository.Get(notification.AddressId);

            address.RemoveMeter(notification.MeterId);

            _addressRepository.Update(address);

            await _documentSession.SaveChangesAsync();
        }

        void INotificationHandler<MeterUnmountedAtAddress>.Handle(MeterUnmountedAtAddress notification)
        {
            TaskUtil.Await(async () => await Handle(notification));
        }
    }
}