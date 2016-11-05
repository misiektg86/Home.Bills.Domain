using System;
using System.Threading.Tasks;
using Home.Bills.Domain.AddressAggregate.Entities;
using Home.Bills.Infrastructure;
using Marten;
using Xunit;

namespace Home.Bills.Tests
{
    public class MartenDataProviderTests : IClassFixture<MartenDatabaseFixture>
    {
        private IDocumentSession _session;

        public MartenDataProviderTests(MartenDatabaseFixture databaseFixture)
        {
            _session = databaseFixture.DocumentStore.OpenSession();
        }

        [Fact]
        public async Task ShouldGetAllAddresses()
        {
            await InsertAddress();
            await InsertAddress();
            await InsertAddress();

            var dataProvider = new AddressDataProvider(_session);

            var data = await dataProvider.GetAddresses();

            Assert.NotEmpty(data);
        }

        private async Task<Guid> InsertAddress()
        {
            var repository = CreateGenericMartenRepository();

            var address = Address.Create("test street", "test city", "2b", "2");

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