using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.Messages;
using Home.Bills.Infrastructure;
using Home.Bills.Payments.Domain.AddressAggregate;
using Marten;
using MediatR;
using Xunit;

namespace Home.Bills.Payments.Domain.Tests
{
    public class AclTests : IClassFixture<InfrastructureFixture>
    {
        private readonly InfrastructureFixture _infrastructureFixture;

        public AclTests(InfrastructureFixture infrastructureFixture)
        {
            _infrastructureFixture = infrastructureFixture;
        }

        [Fact]
        public async Task ShouldIAddressCreatedMessageCreateAddress()
        {
            var addressId = Guid.NewGuid();

            await _infrastructureFixture.Bus.Publish<IAddressCreated>(new { Id = addressId, SquareMeters = 50.00 });

            await Task.Delay(500);

            var addressRepository = _infrastructureFixture.AutofacContainer.Resolve<IRepository<Address, Guid>>();

            var address = await addressRepository.Get(addressId);

            Assert.NotNull(address);
        }

        //[Fact] TODO
        //public async Task ShouldIUsageCreatedMessageRegisterUsageForAddress()
        //{
        //    var addressId = Guid.NewGuid();

        //    var meterSerialNumber = "1234";

        //    var address = _infrastructureFixture.AutofacContainer.Resolve<Frameworks.Light.Ddd.IAggregateFactory<Address, AddressFactoryInput, Guid>>().Create(new AddressFactoryInput() { AddressId = addressId, SquareMeters = 50.00 });

        //    var session = _infrastructureFixture.AutofacContainer.Resolve<IDocumentSession>();

        //    var addressRepository = _infrastructureFixture.AutofacContainer.Resolve<IRepository<Address, Guid>>();

        //    address.ApplyTariff(meterSerialNumber, 34m);

        //    addressRepository.Add(address);

        //    await session.SaveChangesAsync();

        //    await _infrastructureFixture.Bus.Publish<IUsageCreated>(new { AddressId = addressId, MeterSerialNumber = meterSerialNumber, Value = 3.00, ReadDateTime = DateTime.Now });

        //    await Task.Delay(1000);

        //    var addressAggregate = await new GenericMartenRepository<Address>(_infrastructureFixture.MartenDatabaseFixture.DocumentStore.OpenSession(), _infrastructureFixture.AutofacContainer.Resolve<IMediator>()).Get(addressId);

        //    var usages = addressAggregate.GetUsages();

        //    Assert.NotEmpty(usages);

        //    Assert.Equal(meterSerialNumber, usages.First().MeterSerialNumber);

        //    Assert.Equal(3.00, usages.First().Value);

        //    Assert.Equal(34m * 3m, usages.First().AmountToPay);
        //}
    }
}