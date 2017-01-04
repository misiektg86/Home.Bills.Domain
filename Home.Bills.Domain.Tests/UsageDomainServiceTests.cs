using System;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate;
using Home.Bills.Domain.AddressAggregate.Events;
using Home.Bills.Domain.MeterReadAggregate;
using Home.Bills.Domain.Tests.Integration;
using Marten;
using MassTransit;
using Xunit;

namespace Home.Bills.Domain.Tests
{
    public class UsageDomainServiceTests : IClassFixture<MartenDatabaseFixture>
    {
        private readonly MartenDatabaseFixture _martenDatabaseFixture;
        private AddressFactory _addressFactory;
        private GenericMartenRepository<Address> _addressRepository;
        //private GenericMartenRepository<Usage> _usageRepository;
        private IDocumentSession _documentSession;

        public UsageDomainServiceTests(MartenDatabaseFixture martenDatabaseFixture)
        {
            _martenDatabaseFixture = martenDatabaseFixture;
            _addressFactory = new AddressFactory(NSubstitute.Substitute.For<IBus>());
            _documentSession = martenDatabaseFixture.DocumentStore.OpenSession();
            _addressRepository = new GenericMartenRepository<Address>(_documentSession, NSubstitute.Substitute.For<IBus>());
          //  _usageRepository = new GenericMartenRepository<Usage>(_documentSession, NSubstitute.Substitute.For<IBus>());
        }

        //[Fact]
        //public async Task ShouldProvideReadCreateUsageAggregate()
        //{
        //    var address = CreateAddress();

        //    address.AddMeter("123", 0.00);

        //    _addressRepository.Add(address);

        //    _documentSession.SaveChanges();

        //    var usageDomainService = new UsageDomainService(_usageRepository, _addressRepository);

        //    await usageDomainService.CreateUsageFromMeterRead(Guid.NewGuid(), address.Id, 5.00, "123", DateTime.Now);

        //    _documentSession.SaveChanges();

        //    var usageDataProvider = new UsageDataProvider(_martenDatabaseFixture.DocumentStore.OpenSession());

        //    var usage = await usageDataProvider.GetLastUsage(address.Id);

        //    Assert.NotNull(usage);

        //    Assert.Equal(5.00, usage.Value);
        //}

        private Address CreateAddress()
        {
            return
               _addressFactory.Create(new AddressFactoryInput()
               {
                   Street = "test street",
                   City = "test city",
                   StreetNumber = "2b",
                   HomeNumber = "2",
                   Id = Guid.NewGuid()
               });
        }
    }
}