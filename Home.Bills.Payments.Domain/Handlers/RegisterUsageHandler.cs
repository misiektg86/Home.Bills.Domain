using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Payments.Domain.AddressAggregate;
using Home.Bills.Payments.Domain.Commands;
using Marten;
using MassTransit;

namespace Home.Bills.Payments.Domain.Handlers
{
    public class RegisteredUsageHandler : IConsumer<RegisterUsage>
    {
        private readonly IDocumentSession _documentSession;
        private readonly IRepository<Address, Guid> _addressRepository;

        public RegisteredUsageHandler(IDocumentSession documentSession, IRepository<Address, Guid> addressRepository)
        {
            _documentSession = documentSession;
            _addressRepository = addressRepository;
        }

        public async Task Consume(ConsumeContext<RegisterUsage> context)
        {
            var address = await _addressRepository.Get(context.Message.AddressId);

            address.RegisterUsage(context.Message.MeterSerialNumber, context.Message.Value);

            _addressRepository.Update(address);

            _documentSession.SaveChanges();
        }
    }
}