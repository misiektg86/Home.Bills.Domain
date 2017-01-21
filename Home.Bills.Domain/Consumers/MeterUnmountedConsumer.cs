using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate;
using Home.Bills.Domain.MeterAggregate;
using MassTransit;

namespace Home.Bills.Domain.Consumers
{
    internal class MeterUnmountedConsumer : IConsumer<MeterUnmountedAtAddress>
    {
        private readonly IRepository<Address, Guid> _addressRepository;
        private readonly IAsyncUnitOfWork _asyncUnitOfWork;

        public MeterUnmountedConsumer(IRepository<Address, Guid> addressRepository, IAsyncUnitOfWork asyncUnitOfWork)
        {
            _addressRepository = addressRepository;
            _asyncUnitOfWork = asyncUnitOfWork;
        }

        public async Task Consume(ConsumeContext<MeterUnmountedAtAddress> context)
        {
            var address = await _addressRepository.Get(context.Message.AddressId);

            address.RemoveMeter(context.Message.MeterId);

            _addressRepository.Update(address);

            await _asyncUnitOfWork.CommitAsync();
        }
    }
}