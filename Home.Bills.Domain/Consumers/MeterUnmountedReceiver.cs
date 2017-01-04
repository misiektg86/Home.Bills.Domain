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
    internal class MeterUnmountedReceiver : IConsumer<MeterUnmountedAtAddress>
    {
        private readonly IRepository<Address, Guid> _addressRepository;
        private readonly IDocumentSession _documentSession;

        public MeterUnmountedReceiver(IRepository<Address, Guid> addressRepository, IDocumentSession documentSession)
        {
            _addressRepository = addressRepository;
            _documentSession = documentSession;
        }

        public async Task Consume(ConsumeContext<MeterUnmountedAtAddress> context)
        {
            var address = await _addressRepository.Get(context.Message.AddressId);

            address.RemoveMeter(context.Message.MeterId);

            _addressRepository.Update(address);

            await _documentSession.SaveChangesAsync();
        }
    }
}