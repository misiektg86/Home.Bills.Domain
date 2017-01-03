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
    public class MeterMountedReceiver : IAsyncNotificationHandler<MeterMountedAtAddress>, INotificationHandler<MeterMountedAtAddress>
    {
        private readonly IRepository<Address, Guid> _addressRepository;
        private readonly IDocumentSession _documentSession;

        public MeterMountedReceiver(IRepository<Address, Guid> addressRepository, IDocumentSession documentSession)
        {
            _addressRepository = addressRepository;
            _documentSession = documentSession;
        }

        public async Task Handle(MeterMountedAtAddress notification)
        {
            var address = await _addressRepository.Get(notification.AddressId);

            address.AssignMeter(notification.MeterId);

            _addressRepository.Update(address);

            await _documentSession.SaveChangesAsync();
        }

        void INotificationHandler<MeterMountedAtAddress>.Handle(MeterMountedAtAddress notification)
        {
            TaskUtil.Await(async () => await Handle(notification));
        }
    }
}