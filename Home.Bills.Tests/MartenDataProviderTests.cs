using System;
using System.Linq;
using System.Threading.Tasks;
using Home.Bills.Domain.AddressAggregate;
using Home.Bills.Domain.AddressAggregate.Entities;
using Home.Bills.Infrastructure;
using Marten;
using MediatR;
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

            _addressFactory = new AddressFactory(NSubstitute.Substitute.For<IMediator>());
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
                    Id = Guid.NewGuid()
                });

            var id = address.Id;

            repository.Add(address);

            await _session.SaveChangesAsync();

            return id;
        }

        private GenericMartenRepository<Address> CreateGenericMartenRepository()
        {
            var repository = new GenericMartenRepository<Address>(_session);
            return repository;
        }
    }
}