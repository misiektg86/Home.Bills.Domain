using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Payments.Domain.AddressAggregate;
using Home.Bills.Payments.Domain.Commands;
using Marten;
using MediatR;

namespace Home.Bills.Payments.Domain.Handlers
{
    public class RegisteredUsageHandler : IAsyncNotificationHandler<RegisterUsage>
    {
        private readonly IDocumentSession _documentSession;
        private readonly IRepository<Address, Guid> _addressRepository;

        public RegisteredUsageHandler(IDocumentSession documentSession, IRepository<Address, Guid> addressRepository)
        {
            _documentSession = documentSession;
            _addressRepository = addressRepository;
        }

        public async Task Handle(RegisterUsage notification)
        {
            var address = await _addressRepository.Get(notification.AddressId);

            address.RegisterUsage(notification.MeterSerialNumber, notification.Value);

            _addressRepository.Update(address);

            _documentSession.SaveChanges();
        }
    }
}