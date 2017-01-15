using System.Collections.Generic;
using System.Threading.Tasks;
using Home.Bills.Payments.DataAccess.Dtos;

namespace Home.Bills.Payments.DataAccess
{
    public interface IRegistratorDataProvider
    {
        Task<IEnumerable<Registrator>> GetRegistrators();
    }
}