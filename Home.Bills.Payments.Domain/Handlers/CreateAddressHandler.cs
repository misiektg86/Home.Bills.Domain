using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Payments.Domain.AddressAggregate;
using Home.Bills.Payments.Domain.Commands;
using Marten;
using MassTransit;

namespace Home.Bills.Payments.Domain.Handlers
{
    public class CreateAddressHandler : IConsumer<CreateAddress>
    {
        private readonly IAggregateFactory<Address, AddressFactoryInput, Guid> _factory;
        private readonly IRepository<Address, Guid> _addressRepository;
        private readonly IDocumentSession _documentSession;

        public CreateAddressHandler(IAggregateFactory<Address, AddressFactoryInput, Guid> factory, IRepository<Address, Guid> addressRepository, IDocumentSession documentSession)
        {
            _factory = factory;
            _addressRepository = addressRepository;
            _documentSession = documentSession;
        }

        public async Task Consume(ConsumeContext<CreateAddress> context)
        {
            var address = _factory.Create(new AddressFactoryInput() { AddressId = context.Message.Id, SquareMeters = context.Message.SquareMeters });

            _addressRepository.Add(address);

           await _documentSession.SaveChangesAsync();
        }
    }
}