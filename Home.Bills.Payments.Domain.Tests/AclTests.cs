using System;
using System.Threading.Tasks;
using Autofac;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.Messages;
using Home.Bills.Payments.Domain.AddressAggregate;
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

            await Task.Delay(1000);

            var addressRepository = _infrastructureFixture.AutofacContainer.Resolve<IRepository<Address, Guid>>();

            var address = await addressRepository.Get(addressId);

            Assert.NotNull(address);
        }
    }
}