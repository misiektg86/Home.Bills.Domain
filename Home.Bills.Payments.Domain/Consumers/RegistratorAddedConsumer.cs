using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Payments.Domain.RegistratorAgregate;
using Home.Bills.Payments.Messages;
using Marten;
using MassTransit;

namespace Home.Bills.Payments.Domain.Consumers
{
    public class RegistratorAddedConsumer : IConsumer<IRegistratorAdded>
    {
        private readonly IRepository<Registrator, Guid> _repository;
        private readonly IAggregateFactory<Registrator, FactoryInput, Guid> _factory;
        private readonly IDocumentSession _documentSession;

        public RegistratorAddedConsumer(IRepository<Registrator, Guid> repository, IAggregateFactory<Registrator, FactoryInput, Guid> factory, IDocumentSession documentSession)
        {
            _repository = repository;
            _factory = factory;
            _documentSession = documentSession;
        }

        public async Task Consume(ConsumeContext<IRegistratorAdded> context)
        {
            var registrator =
                _factory.Create(new FactoryInput()
                {
                    AddressId = context.Message.AddressId,
                    Description = context.Message.Description,
                    RegistratorId = context.Message.RegistratorId
                });

            _repository.Add(registrator);

            await _documentSession.SaveChangesAsync();
        }
    }
}