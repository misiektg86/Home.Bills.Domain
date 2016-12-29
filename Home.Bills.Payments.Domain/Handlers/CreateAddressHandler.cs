using System;
using Frameworks.Light.Ddd;
using Home.Bills.Payments.Domain.AddressAggregate;
using Home.Bills.Payments.Domain.Commands;
using Marten;
using MediatR;

namespace Home.Bills.Payments.Domain.Handlers
{
    public class CreateAddressHandler : INotificationHandler<CreateAddress>
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

        public void Handle(CreateAddress notification)
        {
            var address = _factory.Create(new AddressFactoryInput() { AddressId = notification.Id, SquareMeters = notification.SquareMeters });

            _addressRepository.Add(address);

            _documentSession.SaveChanges();
        }
    }
}