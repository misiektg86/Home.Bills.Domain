using System;
using System.Threading.Tasks;
using Home.Bills.DataAccess.Dto;

namespace Home.Bills.DataAccess
{
    public interface IMeterReadDataProvider
    {
        Task<MeterRead> GetMeterRead(Guid meterReadId);
    }
}