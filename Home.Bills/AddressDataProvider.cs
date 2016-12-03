﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Home.Bills.Domain.AddressAggregate.DataProviders;
using Home.Bills.Domain.AddressAggregate.Entities;
using Home.Bills.Domain.AddressAggregate.ValueObjects;
using Marten;
using Marten.Linq;

namespace Home.Bills
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
                    ((IMartenQueryable) _documentSession.Query<Address>()).ToListAsync<Address>(
                        CancellationToken.None);

            return list.Select(i => i.Information).ToList();
        }
    }
}