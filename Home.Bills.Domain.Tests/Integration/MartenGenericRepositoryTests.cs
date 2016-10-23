using System;
using System.Linq;
using System.Threading.Tasks;
using Home.Bills.Domain.AddressAggregate.Entities;
using Home.Bills.Infrastructure;
using Xunit;

namespace Home.Bills.Domain.Tests.Integration
{
    public class MartenGenericRepositoryTests : IClassFixture<MartenDatabaseFixture>
    {
        private readonly MartenDatabaseFixture _fixture;

        public MartenGenericRepositoryTests(MartenDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldAddDocumentToRepository()
        {
            var id = await InsertAddress();

            var repository = CreateGenericMartenRepository();

            await repository.StartAsync();

            var loadedAddress = await repository.Get(id);

            Assert.Equal(id, loadedAddress.Id);

            repository.Dispose();
        }

        private async Task<Guid> InsertAddress()
        {
            var repository = CreateGenericMartenRepository();

            var repo = repository.StartAsync();

            var address = Address.Create("test street", "test city", "2b", "2");

            var id = address.Id;

            await repo;

            repository.Add(address);

            await repository.CommitAsync();

            repository.Dispose();

            return id;
        }

        private GenericMartenRepository<Address> CreateGenericMartenRepository()
        {
            var repository = new GenericMartenRepository<Address>(_fixture.DocumentStore);
            return repository;
        }

        [Fact]
        public async Task ShouldRemoveDocumentFromRepository()
        {
            var address = await InsertAddress();

            var repo = CreateGenericMartenRepository();

            await repo.StartAsync();

            repo.Delete(address);

            await repo.CommitAsync();

            await repo.StartAsync();

            var removedAddress = await repo.Get(address);

            Assert.Null(removedAddress);

            repo.Dispose();
        }

        [Fact]
        public async Task ShouldUpdateDocumentInRepository()
        {
            var address = await InsertAddress();

            var repo = CreateGenericMartenRepository();

            await repo.StartAsync();

            var loadedAddress = await repo.Get(address);

            loadedAddress.AddMeter("1234", 25.00);

            repo.Update(loadedAddress);

            await repo.CommitAsync();

            await repo.StartAsync();

            loadedAddress = await repo.Get(address);

            Assert.True(loadedAddress.GetMeters().Any(i => i.Id == "1234"));
        }

        [Fact]
        public async Task ShouldCommitUnitOfWork()
        {
            
        }
    }
}