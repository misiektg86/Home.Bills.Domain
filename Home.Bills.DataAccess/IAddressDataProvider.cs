using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Home.Bills.DataAccess.Dto;
using Home.Bills.Domain.AddressAggregate.ValueObjects;

namespace Home.Bills.DataAccess
{
    public interface IAddressDataProvider
    {
        Task<bool> AddressExists(string city, string street, string streetNumber, string homeNumber);
        Task<IEnumerable<Address>> GetAddresses();
        Task<Address> GetAddress(Guid addressId);
    }
}