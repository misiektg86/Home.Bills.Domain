using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Home.Bills.DataAccess;
using Home.Bills.DataAccess.Dto;
using Marten;

namespace Home.Bills.Client
{
    internal class MartenServiceClient : IServiceClient, IDisposable
    {
        private readonly IUsageDataProvider _usageDataProvider;
        private readonly IMeterDataProvider _meterDataProvider;
        private readonly IAddressDataProvider _addressDataProvider;
        private readonly IMeterReadDataProvider _meterReadDataProvider;
        private readonly IDocumentSession _documentSession;

        public MartenServiceClient(IUsageDataProvider usageDataProvider, IMeterDataProvider meterDataProvider, IAddressDataProvider addressDataProvider, IMeterReadDataProvider meterReadDataProvider, IDocumentSession documentSession)
        {
            _usageDataProvider = usageDataProvider;
            _meterDataProvider = meterDataProvider;
            _addressDataProvider = addressDataProvider;
            _meterReadDataProvider = meterReadDataProvider;
            _documentSession = documentSession;
        }

        public Task<IEnumerable<Usage>> GetUsagesForAddress(Guid addressId)
        {
            return _usageDataProvider.GetUsages(addressId);
        }

        public Task<MeterRead> GetMeterRead(Guid meterReadId)
        {
            return _meterReadDataProvider.GetMeterRead(meterReadId);
        }

        public Task<IEnumerable<Meter>> GetMetersForAtAddress(Guid addressId)
        {
            return _meterDataProvider.GetAllMeters(addressId);
        }

        public Task<Address> GetAddressDetails(Guid addressId)
        {
            return _addressDataProvider.GetAddress(addressId);
        }

        public Task<Meter> GetMeter(Guid meterId)
        {
            return _meterDataProvider.GetMeter(meterId);
        }

        public void Dispose()
        {
            _documentSession?.Dispose();
        }
    }
}