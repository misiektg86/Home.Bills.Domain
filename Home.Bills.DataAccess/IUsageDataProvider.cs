using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Home.Bills.DataAccess
{
    public interface IUsageDataProvider
    {
        Task<IEnumerable<Dto.Usage>> GetUsages(Guid addressId);

        Task<Dto.Usage> GetLastUsage(Guid address);
    }
}