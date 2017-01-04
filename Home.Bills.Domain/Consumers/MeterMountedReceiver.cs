using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate;
using Home.Bills.Domain.AddressAggregate.Events;
using Home.Bills.Domain.MeterAggregate;
using Marten;
using MassTransit;

namespace Home.Bills.Domain.Consumers
{
    public class MeterMountedReceiver : IConsumer<MeterMountedAtAddress>
    {
        private readonly IRepository<Address, Guid> _addressRepository;
        private readonly IDocumentSession _documentSession;

        public MeterMountedReceiver(IRepository<Address, Guid> addressRepository, IDocumentSession documentSession)
        {
            _addressRepository = addressRepository;
            _documentSession = documentSession;
        }

        public async Task Consume(ConsumeContext<MeterMountedAtAddress> context)
        {
            var address = await _addressRepository.Get(context.Message.AddressId);

            address.AssignMeter(context.Message.MeterId);

            _addressRepository.Update(address);

            await _documentSession.SaveChangesAsync();
        }
    }
}