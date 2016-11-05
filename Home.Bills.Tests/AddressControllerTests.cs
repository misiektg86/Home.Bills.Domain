using System;
using System.Linq;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Controllers;
using Home.Bills.Domain.AddressAggregate.DataProviders;
using Home.Bills.Domain.AddressAggregate.Entities;
using Home.Bills.Models;
using NSubstitute;
using Xunit;
using Xunit.Sdk;

namespace Home.Bills.Tests
{

    public class AddressControllerTests
    {
        [Fact]
        public async Task ShouldAddNewAddressIfNotExists()
        {
            var addressRepository = Substitute.For<IRepository<Address, Guid>>();
            var addressDataProvider = Substitute.For<IAddressDataProvider>();

            addressDataProvider.AddressExists(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(false);

            AddressController addressController = new AddressController(addressRepository, addressDataProvider);

            await
                addressController.Post(new Dtos.Address()
                {
                    City = "test",
                    HomeNumber = "test",
                    Street = "test",
                    StreetNumber = "test"
                });

            addressRepository.Received(1).Add(entity: Arg.Any<Address>());
        }

        [Fact]
        public async Task ShouldNotAddNewAddressIfAleradyExists()
        {
            var addressRepository = Substitute.For<IRepository<Address, Guid>>();
            var addressDataProvider = Substitute.For<IAddressDataProvider>();

            addressDataProvider.AddressExists(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(true);

            AddressController addressController = new AddressController(addressRepository, addressDataProvider);

            await
                addressController.Post(new Dtos.Address()
                {
                    City = "test",
                    HomeNumber = "test",
                    Street = "test",
                    StreetNumber = "test"
                });

            addressRepository.DidNotReceive().Add(entity: Arg.Any<Address>());
        }

        [Fact]
        public async Task ShouldDeleteAddressIfExists()
        {
            var addressRepository = Substitute.For<IRepository<Address, Guid>>();
            var addressDataProvider = Substitute.For<IAddressDataProvider>();

            var addressEntity = Address.Create("test", "test", "test", "test");

            addressRepository.Get(addressEntity.Id).Returns(addressEntity);

            AddressController addressController = new AddressController(addressRepository, addressDataProvider);

            await addressController.Delete(addressEntity.Id);

            addressRepository.Received(1).Delete(addressEntity.Id);
        }

        [Fact]
        public async Task ShouldNotDeleteAddressIfNotExists()
        {
            var addressRepository = Substitute.For<IRepository<Address, Guid>>();
            var addressDataProvider = Substitute.For<IAddressDataProvider>();

            Guid entityId = Guid.NewGuid();

            addressRepository.Get(entityId).Returns(default(Address));

            AddressController addressController = new AddressController(addressRepository, addressDataProvider);

            await addressController.Delete(entityId);

            addressRepository.Received(0).Delete(entityId);
        }

        [Fact]
        public async Task ShouldProvidedReadCreateUsage()
        {
            var addressRepository = Substitute.For<IRepository<Address, Guid>>();
            var addressDataProvider = Substitute.For<IAddressDataProvider>();

            var addressEntity = Address.Create("test", "test", "test", "test");

            addressEntity.AddMeter("123", 10);

            addressRepository.Get(addressEntity.Id).Returns(addressEntity);

            AddressController addressController = new AddressController(addressRepository, addressDataProvider);

            await
                addressController.ProvideRead(new MeterRead()
                {
                    AddressId = addressEntity.Id,
                    MeterSerialNumber = "123",
                    Read = 30,
                    ReadDate = DateTime.Now
                });


            await addressRepository.Received(1).Get(addressEntity.Id);

            Assert.Equal(20, addressEntity.GetUsages().First().Value);
        }
    }
}
