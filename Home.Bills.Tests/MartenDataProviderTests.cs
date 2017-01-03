using System;
using System.Linq;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.DataAccess;
using Home.Bills.Domain.AddressAggregate;
using Home.Bills.Domain.AddressAggregate.Entities;
using Marten;
using MassTransit;
using NSubstitute;
using Xunit;

namespace Home.Bills.Tests
{
    public class MartenDataProviderTests : IClassFixture<MartenDatabaseFixture>
    {
        private IDocumentSession _session;
        private MartenDatabaseFixture _databaseFixture;
        private AddressFactory _addressFactory;

        public MartenDataProviderTests(MartenDatabaseFixture databaseFixture)
        {
            _databaseFixture = databaseFixture;
            _session = _session = databaseFixture.DocumentStore.OpenSession();

            _addressFactory = new AddressFactory(Substitute.For<IBus>());
        }

        [Fact]
        public async Task ShouldGetAllAddresses()
        {
            await InsertAddress();
            await InsertAddress();
            await InsertAddress();

            _session.Dispose();

            _session = _databaseFixture.DocumentStore.OpenSession();

            var dataProvider = new AddressDataProvider(_session);

            var data = (await dataProvider.GetAddresses()).ToList();

            Assert.NotEmpty(data);

            var address = data.First();

            Assert.NotNull(address.City);
            Assert.NotNull(address.HomeNumber);
            Assert.NotNull(address.Street);
            Assert.NotNull(address.StreetNumber);
            Assert.NotNull(address.SquareMeters);
        }

        private async Task<Guid> InsertAddress()
        {
            var repository = CreateGenericMartenRepository();

            var address =
                _addressFactory.Create(new AddressFactoryInput()
                {
                    Street = "test street",
                    City = "test city",
                    StreetNumber = "2b",
                    HomeNumber = "2",
                    Id = Guid.NewGuid(),
                    SquareMeters = 50.00
                });

            var id = address.Id;

            repository.Add(address);

            await _session.SaveChangesAsync();

            return id;
        }

        private GenericMartenRepository<Address> CreateGenericMartenRepository()
        {
            var repository = new GenericMartenRepository<Address>(_session, Substitute.For<IBus>());
            return repository;
        }
    }
}