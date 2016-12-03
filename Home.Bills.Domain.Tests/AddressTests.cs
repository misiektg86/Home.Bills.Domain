using System;
using System.Linq;
using Home.Bills.Domain.AddressAggregate;
using Home.Bills.Domain.AddressAggregate.Entities;
using Home.Bills.Domain.AddressAggregate.Exceptions;
using MediatR;
using Xunit;

namespace Home.Bills.Domain.Tests
{
    public class AddressTests
    {
        private AddressFactory _addressFactory;

        public AddressTests()
        {
            _addressFactory = new AddressFactory(NSubstitute.Substitute.For<IMediator>());
        }

        [Fact]
        public void ShouldAddNewMeter()
        {
            var address = CreateAddress();

            address.AddMeter("1234", 10.000);

            Assert.Contains(address.GetMeters(), i => i.Id == "1234");
        }

        [Fact]
        public void ShouldThrowExceptionIfMeterExists()
        {
            var address = CreateAddress();

            address.AddMeter("1234", 10.000);

            Assert.Throws<InvalidOperationException>(() => address.AddMeter("1234", 10.000));
        }

        [Fact]
        public void ShouldExchangeMeter()
        {
            var address = CreateAddress();

            address.AddMeter("1234", 10.000);

            address.ExchangeMeter("1234", "4321", 10.000);

            Assert.DoesNotContain(address.GetMeters(), i => i.Id == "1234");

            Assert.Contains(address.GetMeters(), i => i.Id == "4321");
        }

        [Fact]
        public void ShouldTrhowExceptionIfMeterDoesntExist()
        {
            var address = CreateAddress();

            Assert.Throws<InvalidOperationException>(() => address.ExchangeMeter("1234", "4321", 10.000));
        }

        [Fact]
        public void SohuldThrowExceptionIfNewReadLowerThanPrevious()
        {
            var address = CreateAddress();

            address.AddMeter("1234", 14.000);

            Assert.Throws<InvalidOperationException>(() => address.ProvideRead(12.00, "1234", DateTime.Now));
        }

        private  Address CreateAddress()
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

        [Fact]
        public void ShouldNewReadUpdateMeterState()
        {
            var address = CreateAddress();

            address.AddMeter("1234", 10.000);

            address.ProvideRead(12.00, "1234", DateTime.Now);

            Assert.Equal(12.00, address.GetMeters().Single(i => i.Id == "1234").State);
        }

        [Fact]
        public void ShouldProvideReadCreateNewUsage()
        {
            var address = CreateAddress();

            address.AddMeter("1234", 10.000);

            address.ProvideRead(12.00, "1234", DateTime.Now);

            Assert.Equal(2.00, address.GetUsages().Last().Value);
        }

        [Fact]
        public void ShouldNotBeAbleToModifyMeterBesideAgregate()
        {
            var address = CreateAddress();

            address.AddMeter("1234", 10.000);

            address.ProvideRead(12.00, "1234", DateTime.Now);

            var meter = address.GetMeters().First();

            meter.State = 400.00;

            Assert.NotEqual(address.GetMeters().First().State, meter.State);
        }

        [Fact]
        public void ShouldThrowExceptionIfNoMeterFoundForProvidedRead()
        {
            var address = CreateAddress();

            Assert.Throws<MeterNotFoundException>(() => address.ProvideRead(12.00, "1234", DateTime.Now));
        }

        [Fact]
        public void ShouldCheckInPersons()
        {
            var address = CreateAddress();

            Assert.Equal(0, address.CheckedInPersons);

            address.CheckInPersons(4);

            Assert.Equal(4, address.CheckedInPersons);
        }

        [Fact]
        public void ShouldCheckOutPersons()
        {
            var address = CreateAddress();

            address.CheckInPersons(4);

            address.CheckOutPersons(2);

            Assert.Equal(2, address.CheckedInPersons);
        }

        [Fact]
        public void ShouldThrowExceptionIfCheckingOutMorePersonsThatCheckedIn()
        {
            var address = CreateAddress();

            address.CheckInPersons(4);

            Assert.Throws<CannotCheckOutMorePersonsThanCheckedInException>(() => address.CheckOutPersons(5));
        }
    }
}