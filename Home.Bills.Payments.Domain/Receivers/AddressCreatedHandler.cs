using System;
using Frameworks.Light.Ddd;
using Home.Bills.Payments.Domain.AddressAggregate;
using Home.Bills.Payments.Domain.Events;
using Marten;
using MediatR;

namespace Home.Bills.Payments.Domain.Receivers
{
    public class AddressCreatedHandler : INotificationHandler<AddressCreated>
    {
        private readonly IAggregateFactory<Address, AddressFactoryInput, Guid> _factory;
        private readonly IRepository<Address, Guid> _addressRepository;
        private readonly IDocumentSession _documentSession;

        public AddressCreatedHandler(IAggregateFactory<Address, AddressFactoryInput, Guid> factory, IRepository<Address, Guid> addressRepository, IDocumentSession documentSession)
        {
            _factory = factory;
            _addressRepository = addressRepository;
            _documentSession = documentSession;
        }

        public void Handle(AddressCreated notification)
        {
            var address = _factory.Create(new AddressFactoryInput() { AddressId = notification.Id, SquareMeters = notification.SquareMeters });

            _addressRepository.Add(address);

            _documentSession.SaveChanges();
        }
    }
}