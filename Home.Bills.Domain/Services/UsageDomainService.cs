using System;
using System.Linq;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate;
using Home.Bills.Domain.MeterReadAggregate;

namespace Home.Bills.Domain.Services
{
    public class UsageDomainService
    {
        private readonly IRepository<MeterRead, Guid> _meterReadRepository;
        private readonly IRepository<Address, Guid> _addressRepository;

        public UsageDomainService( IRepository<MeterRead, Guid> meterReadRepository, IRepository<Address, Guid> addressRepository)
        {
            _meterReadRepository = meterReadRepository;
            _addressRepository = addressRepository;
        }

        public async Task CalculateUsage(Guid meterReadId, Guid addressId, double meterState, Guid messageMeterId, Guid usageId)
        {
            var address = await _addressRepository.Get(addressId);

            var meterReadTask = _meterReadRepository.Get(meterReadId);

            MeterRead meterRead = null;

            if (!address.LastFinishedMeterReadProcess.HasValue)
            {
                meterRead = await meterReadTask;

                meterRead.CreateUsage(meterState, meterState, DateTime.Now, messageMeterId, Guid.NewGuid());

                _meterReadRepository.Update(meterRead);

                return;
            }

            var lastMeterRead = await _meterReadRepository.Get(address.LastFinishedMeterReadProcess.Value);

            var lastUsageForMeter = lastMeterRead.Usages.FirstOrDefault(i => i.MeterId == messageMeterId);

            meterRead = await meterReadTask;

            meterRead.CreateUsage(lastUsageForMeter?.CurrentRead ?? meterState, meterState, DateTime.Now, messageMeterId,
                usageId);

            _meterReadRepository.Update(meterRead);
        }
    }
}