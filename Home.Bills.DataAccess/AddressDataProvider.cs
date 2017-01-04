using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Home.Bills.Domain.AddressAggregate;
using Home.Bills.Domain.AddressAggregate.DataProviders;
using Home.Bills.Domain.AddressAggregate.Events;
using Home.Bills.Domain.AddressAggregate.Exceptions;
using Home.Bills.Domain.AddressAggregate.ValueObjects;
using Marten;

namespace Home.Bills.DataAccess
{
    public class AddressDataProvider : IAddressDataProvider
    {
        private readonly IDocumentSession _documentSession;

        public AddressDataProvider(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public Task<bool> AddressExists(string city, string street, string streetNumber, string homeNumber)
        {
            return
                _documentSession.Query<Address>()
                    .AnyAsync(i => i.Information.City.Equals(city, StringComparison.InvariantCultureIgnoreCase) &&
                    i.Information.Street.Equals(street, StringComparison.InvariantCultureIgnoreCase) &&
                    i.Information.StreetNumber.Equals(streetNumber, StringComparison.InvariantCultureIgnoreCase) &&
                    i.Information.HomeNumber.Equals(homeNumber, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<IEnumerable<AddressInformation>> GetAddresses()
        {
            var list =
                await
                    _documentSession.Query<Address>().ToListAsync();

            return list.Select(i => i.Information).ToList();
        }
    }
}