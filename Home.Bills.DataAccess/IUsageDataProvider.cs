using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Home.Bills.DataAccess
{
    public interface IUsageDataProvider
    {
        Task<IEnumerable<Dto.Usage>> GetLastUsages(Guid addressId);

        Task<IEnumerable<Dto.Usage>> GetUsages(Guid meterReadId);
    }
}