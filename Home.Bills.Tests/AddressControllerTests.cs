using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Controllers;
using Home.Bills.DataAccess;
using Home.Bills.Domain.AddressAggregate;
using NSubstitute;
using Xunit;
using Address = Home.Bills.Domain.AddressAggregate.Address;
using Home.Bills.Models;
using MassTransit;

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

            AddressController addressController = new AddressController(addressRepository, addressDataProvider, new AddressFactory(Substitute.For<IBus>()));

            await
                addressController.Post(new Models.Address()
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

            AddressController addressController = new AddressController(addressRepository, addressDataProvider, new AddressFactory(Substitute.For<IBus>()));

            await
                addressController.Post(new Models.Address()
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

            var addressFactory = new AddressFactory(Substitute.For<IBus>());

            var addressEntity = addressFactory.Create(new AddressFactoryInput() {City = "test",Street = "test", HomeNumber = "test", StreetNumber = "test", Id = Guid.NewGuid()});

            addressRepository.Get(addressEntity.Id).Returns(addressEntity);

            AddressController addressController = new AddressController(addressRepository, addressDataProvider, addressFactory);

            await addressController.Delete(addressEntity.Id);

            addressRepository.Received(1).Delete(addressEntity.Id);
        }

        [Fact]
        public async Task ShouldNotDeleteAddressIfNotExists()
        {
            var addressRepository = Substitute.For<IRepository<Address, Guid>>();
            var addressDataProvider = Substitute.For<IAddressDataProvider>();
            var addressFactory = new AddressFactory(Substitute.For<IBus>());

            Guid entityId = Guid.NewGuid();

            addressRepository.Get(entityId).Returns(default(Address));

            AddressController addressController = new AddressController(addressRepository, addressDataProvider, new AddressFactory(Substitute.For<IBus>()));

            await addressController.Delete(entityId);

            addressRepository.Received(0).Delete(entityId);
        }


        [Fact]
        public async Task ShouldCheckInPersons()
        {
            var addressRepository = Substitute.For<IRepository<Address, Guid>>();
            var addressDataProvider = Substitute.For<IAddressDataProvider>();
            var addressFactory = new AddressFactory(Substitute.For<IBus>());

            var addressEntity = addressFactory.Create(new AddressFactoryInput() { City = "test", Street = "test", HomeNumber = "test", StreetNumber = "test", Id = Guid.NewGuid() });

            addressRepository.Get(addressEntity.Id).Returns(addressEntity);

            AddressController addressController = new AddressController(addressRepository, addressDataProvider, addressFactory);

            await
                addressController.CheckInPerson(new CheckIn()
                {
                    AddressId = addressEntity.Id,
                    Persons = 5
                });


            await addressRepository.Received(1).Get(addressEntity.Id);

            Assert.Equal(5, addressEntity.CheckedInPersons);
        }

        [Fact]
        public async Task ShouldCheckOutPersons()
        {
            var addressRepository = Substitute.For<IRepository<Address, Guid>>();
            var addressDataProvider = Substitute.For<IAddressDataProvider>();
            var addressFactory = new AddressFactory(Substitute.For<IBus>());

            var addressEntity = addressFactory.Create(new AddressFactoryInput() { City = "test", Street = "test", HomeNumber = "test", StreetNumber = "test", Id = Guid.NewGuid() });

            addressEntity.CheckInPersons(6);

            addressRepository.Get(addressEntity.Id).Returns(addressEntity);

            AddressController addressController = new AddressController(addressRepository, addressDataProvider, addressFactory);

            await
                addressController.CheckOutPerson(new CheckOut()
                {
                    AddressId = addressEntity.Id,
                    Persons = 5
                });


            await addressRepository.Received(1).Get(addressEntity.Id);

            Assert.Equal(1, addressEntity.CheckedInPersons);
        }
    }
}
