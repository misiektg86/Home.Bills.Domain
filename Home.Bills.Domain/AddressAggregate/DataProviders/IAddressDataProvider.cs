using System.Collections.Generic;
using System.Threading.Tasks;
using Home.Bills.Domain.AddressAggregate.ValueObjects;

namespace Home.Bills.Domain.AddressAggregate.DataProviders
{
    public interface IAddressDataProvider
    {
        Task<bool> AddressExists(string city, string street, string streetNumber, string homeNumber);
        Task<IEnumerable<AddressInformation>> GetAddresses();
    }
}