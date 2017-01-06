using System;
using System.Linq;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate;
using Home.Bills.Domain.MeterReadAggregate;
using Marten;

namespace Home.Bills.Domain.Services
{
    public class UsageDomainService
    {
        private readonly IDocumentSession _documentSession;
        private readonly IRepository<MeterRead, Guid> _meterReadRepository;
        private readonly IRepository<Address, Guid> _addressRepository;

        public UsageDomainService(IDocumentSession documentSession, IRepository<MeterRead, Guid> meterReadRepository, IRepository<Address, Guid> addressRepository)
        {
            _documentSession = documentSession;
            _meterReadRepository = meterReadRepository;
            _addressRepository = addressRepository;
        }

        public async Task CalculateUsage(Guid meterReadId, Guid addressId, double meterState, Guid messageMeterId)
        {
            var address = await _addressRepository.Get(addressId);

            var meterReadTask = _meterReadRepository.Get(meterReadId);

            MeterRead meterRead = null;
            Usage usage = null;

            if (!address.LastFinishedMeterReadProcess.HasValue)
            {
                meterRead = await meterReadTask;

                usage = meterRead.CreateUsage(0.00, meterState, DateTime.Now, messageMeterId, Guid.NewGuid());

                usage.CalculateUsage();

                _meterReadRepository.Update(meterRead);

                await _documentSession.SaveChangesAsync();

                return;
            }

            var lastMeterRead = await _meterReadRepository.Get(address.LastFinishedMeterReadProcess.Value);

            var lastUsageForMeter = lastMeterRead.Usages.First(i => i.MeterId == messageMeterId);

            meterRead = await meterReadTask;

            usage = meterRead.CreateUsage(lastUsageForMeter?.CurrentRead ?? 0.00, meterState, DateTime.Now, messageMeterId,
                Guid.NewGuid());

            usage.CalculateUsage();

            _meterReadRepository.Update(meterRead);

            await _documentSession.SaveChangesAsync();
        }
    }
}