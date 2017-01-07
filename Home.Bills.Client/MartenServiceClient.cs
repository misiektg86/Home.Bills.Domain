using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Home.Bills.DataAccess.Dto;

namespace Home.Bills.Client
{
    internal class MartenServiceClient : IServiceClient
    {
        public Task<IEnumerable<Usage>> GetUsagesForAddress(Guid addressId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Meter>> GetMetersForAtAddress(Guid addressId)
        {
            throw new NotImplementedException();
        }

        public Task<Address> GetAddressDetails(Guid addressId)
        {
            throw new NotImplementedException();
        }

        public Task<Meter> GetMeter(Guid meterId)
        {
            throw new NotImplementedException();
        }
    }
}