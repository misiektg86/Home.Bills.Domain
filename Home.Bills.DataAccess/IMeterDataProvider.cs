using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Home.Bills.DataAccess.Dto;

namespace Home.Bills.DataAccess
{
    public interface IMeterDataProvider
    {
        Task<Meter> GetMeter(Guid meterId);
        Task<IEnumerable<Meter>> GetAllMeters(Guid addressId);

        Task<IEnumerable<Meter>> GetAllMeters();
    }
}