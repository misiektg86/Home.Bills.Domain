using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Home.Bills.Domain.AddressAggregate;
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

        public async Task<IEnumerable<Dto.Address>> GetAddresses()
        {
            var list =
                await
                    _documentSession.Query<Address>().ToListAsync();

            return list.Select(i => new Dto.Address()
            {
                AddressId = i.Id,
                CheckedInPersons = i.CheckedInPersons,
                City = i.Information.City,
                HomeNumber = i.Information.HomeNumber,
                LastFinishedMeterReadProcess = i.LastFinishedMeterReadProcess,
                MeterReadId = i.MeterReadId,
                Meters = i.Meters.ToList(),
                SquareMeters = i.Information.SquareMeters,
                Street = i.Information.Street,
                StreetNumber = i.Information.StreetNumber
            }).ToList();
        }

        public async Task<Dto.Address> GetAddress(Guid addressId)
        {
            var address =
                await
                    _documentSession.Query<Address>().FirstOrDefaultAsync(i => i.Id == addressId);

            return new Dto.Address()
            {
                AddressId = address.Id,
                CheckedInPersons = address.CheckedInPersons,
                City = address.Information.City,
                HomeNumber = address.Information.HomeNumber,
                LastFinishedMeterReadProcess = address.LastFinishedMeterReadProcess,
                MeterReadId = address.MeterReadId,
                Meters = address.Meters.ToList(),
                SquareMeters = address.Information.SquareMeters,
                Street = address.Information.Street,
                StreetNumber = address.Information.StreetNumber
            };
        }
    }
}