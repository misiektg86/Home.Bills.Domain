using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Home.Bills.DataAccess.Dto;

namespace Home.Bills.Client
{
    public interface IServiceClient
    {
        Task<IEnumerable<Usage>> GetUsagesForAddress(Guid addressId);

        Task<IEnumerable<Meter>> GetMetersForAtAddress(Guid addressId);

        Task<Address> GetAddressDetails(Guid addressId);

        Task<Meter> GetMeter(Guid meterId);
    }
}
