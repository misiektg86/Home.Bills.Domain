using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Notifications.Domain.AddressAggregate;
using Home.Bills.Notifications.Domain.Consumers.Saga;
using Home.Bills.Notifications.Domain.Services;
using MassTransit;

namespace Home.Bills.Notifications.Domain.Consumers
{
    public class RegisteredAcceptedPaymentConsumer : IConsumer<IPaymentAccepted>
    {
        private readonly IRepository<Address, Guid> _addressRepository;
        private readonly IAsyncUnitOfWork _asyncUnitOfWork;

        public RegisteredAcceptedPaymentConsumer(IRepository<Address,Guid> addressRepository, IAsyncUnitOfWork asyncUnitOfWork )
        {
            _addressRepository = addressRepository;
            _asyncUnitOfWork = asyncUnitOfWork;
        }

        public async Task Consume(ConsumeContext<IPaymentAccepted> context)
        {
            var address = await _addressRepository.Get(context.Message.AddressId);

            if (address == null)
            {
                throw new AddressCannotBeEmptyException(context.Message.AddressId.ToString());
            }

            address.RegisterAcceptedPayment(context.Message.PaymentId);

            _addressRepository.Update(address);

            await _asyncUnitOfWork.CommitAsync();
        }
    }
}