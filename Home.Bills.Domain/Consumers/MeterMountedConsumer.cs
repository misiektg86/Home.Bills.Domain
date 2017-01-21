using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate;
using Home.Bills.Domain.MeterAggregate;
using MassTransit;

namespace Home.Bills.Domain.Consumers
{
    public class MeterMountedConsumer : IConsumer<MeterMountedAtAddress>
    {
        private readonly IRepository<Address, Guid> _addressRepository;
        private readonly IAsyncUnitOfWork _asyncUnitOfWork;

        public MeterMountedConsumer(IRepository<Address, Guid> addressRepository, IAsyncUnitOfWork asyncUnitOfWork)
        {
            _addressRepository = addressRepository;
            _asyncUnitOfWork = asyncUnitOfWork;
        }

        public async Task Consume(ConsumeContext<MeterMountedAtAddress> context)
        {
            var address = await _addressRepository.Get(context.Message.AddressId);

            address.AssignMeter(context.Message.MeterId);

            _addressRepository.Update(address);

            await _asyncUnitOfWork.CommitAsync();
        }
    }
}