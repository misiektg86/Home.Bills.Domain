using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Payments.Domain.AddressAggregate;
using Home.Bills.Payments.Messages;
using MassTransit;

namespace Home.Bills.Payments.Domain.Consumers
{
    public class AddressAddedConsumer : IConsumer<IAddressAdded>
    {
        private readonly IRepository<Address, Guid> _repository;
        private readonly IAggregateFactory<Address, AddressFactoryInput, Guid> _factory;
        private readonly IAsyncUnitOfWork _asyncUnitOfWork;

        public AddressAddedConsumer(IRepository<Address, Guid> repository, IAggregateFactory<Address, AddressFactoryInput, Guid> factory, IAsyncUnitOfWork asyncUnitOfWork)
        {
            _repository = repository;
            _factory = factory;
            _asyncUnitOfWork = asyncUnitOfWork;
        }

        public async Task Consume(ConsumeContext<IAddressAdded> context)
        {
            var address =
              _factory.Create(new AddressFactoryInput()
              {
                  AddressId = context.Message.Id,
                  SquareMeters = context.Message.SquareMeters
              });

            _repository.Add(address);

            await _asyncUnitOfWork.CommitAsync();
        }
    }
}