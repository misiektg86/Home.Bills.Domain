using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Home.Bills.Domain.AddressAggregate;
using Home.Bills.Domain.AddressAggregate.Entities;
using Home.Bills.Infrastructure;
using Marten;
using MediatR;
using Xunit;

namespace Home.Bills.Domain.Tests.Integration
{
    public class MartenGenericRepositoryTests : IClassFixture<MartenDatabaseFixture>
    {
        private readonly MartenDatabaseFixture _fixture;
        private IDocumentSession _session;
        private AddressFactory _addressFactory;

        public MartenGenericRepositoryTests(MartenDatabaseFixture fixture)
        {
            _fixture = fixture;
            _session = fixture.DocumentStore.OpenSession();

            _addressFactory = new AddressFactory(NSubstitute.Substitute.For<IMediator>());
        }

        [Fact]
        public async Task ShouldAddDocumentToRepository()
        {
            var id = await InsertAddress();

            var repository = CreateGenericMartenRepository();

            var loadedAddress = await repository.Get(id);

            Assert.Equal(id, loadedAddress.Id);
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
            var repository = new GenericMartenRepository<Address>(_session, new Mediator(type => new object(), type => new List<object>()));
            return repository;
        }

        [Fact]
        public async Task ShouldRemoveDocumentFromRepository()
        {
            var address = await InsertAddress();

            var repo = CreateGenericMartenRepository();

            repo.Delete(address);

            await _session.SaveChangesAsync();

            var removedAddress = await repo.Get(address);

            Assert.Null(removedAddress);
        }

        [Fact]
        public async Task ShouldUpdateDocumentInRepository()
        {
            var address = await InsertAddress();

            var repo = CreateGenericMartenRepository();

            var loadedAddress = await repo.Get(address);

            loadedAddress.AddMeter("1234", 25.00);

            repo.Update(loadedAddress);

            await _session.SaveChangesAsync();

            _session.Dispose();

            _session = _fixture.DocumentStore.OpenSession();

            repo = CreateGenericMartenRepository();

            loadedAddress = await repo.Get(address);

            Assert.True(loadedAddress.GetMeters().Any(i => i.Id == "1234"));
        }

        [Fact]
        public async Task ShouldCreateNewUsage()
        {
            var address = await InsertAddress();

            var repo = CreateGenericMartenRepository();

            var loadedAddress = await repo.Get(address);

            loadedAddress.AddMeter("1234", 25.00);

            loadedAddress.ProvideRead(30.00, "1234", DateTime.Now);

            repo.Update(loadedAddress);

            await _session.SaveChangesAsync();

            _session.Dispose();

            _session = _fixture.DocumentStore.OpenSession();

            repo = CreateGenericMartenRepository();

            loadedAddress = await repo.Get(address);

            Assert.True(loadedAddress.GetMeters().Any(i => i.Id == "1234"));

            Assert.NotEmpty(loadedAddress.GetUsages());
        }
    }
}