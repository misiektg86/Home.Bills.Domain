using System;
using Home.Bills.Domain.AddressAggregate;
using Home.Bills.Domain.AddressAggregate.Entities;
using Home.Bills.Domain.AddressAggregate.Exceptions;
using MassTransit;
using Xunit;

namespace Home.Bills.Domain.Tests
{
    public class AddressTests
    {
        private AddressFactory _addressFactory;

        public AddressTests()
        {
            _addressFactory = new AddressFactory(NSubstitute.Substitute.For<IBus>());
        }

        [Fact]
        public void ShouldAddNewMeter()
        {
            var address = CreateAddress();

            var meterGuid = Guid.NewGuid();

            address.AssignMeter(meterGuid);

            Assert.Contains(address.GetMeters(), i => i == meterGuid);
        }

        [Fact]
        public void ShouldThrowExceptionIfMeterExists()
        {
            var address = CreateAddress();

            var meterGuid = Guid.NewGuid();

            address.AssignMeter(meterGuid);

            Assert.Throws<InvalidOperationException>(() => address.AssignMeter(meterGuid));
        }

        //[Fact]
        //public void ShouldExchangeMeter()
        //{
        //    var address = CreateAddress();

        //    var meterGuid = Guid.NewGuid();

        //    var newMeterGuid = Guid.NewGuid();

        //    address.AssignMeter(meterGuid);

        //    address.ExchangeMeter(meterGuid, newMeterGuid, 10.000);

        //    Assert.DoesNotContain(address.GetMeters(), i => i == meterGuid);

        //    Assert.Contains(address.GetMeters(), i => i == newMeterGuid);
        //}

        //[Fact]
        //public void ShouldTrhowExceptionIfMeterDoesntExist()
        //{
        //    var address = CreateAddress();

        //    Assert.Throws<InvalidOperationException>(() => address.ExchangeMeter(Guid.NewGuid(), Guid.NewGuid(), 10.000));
        //}

        //[Fact] TODO
        //public void SohuldThrowExceptionIfNewReadLowerThanPrevious()
        //{
        //    var address = CreateAddress();

        //    address.AddMeter("1234", 14.000);

        //    Assert.Throws<InvalidOperationException>(() => address.ProvideRead(Guid.Empty, 12.00, "1234", DateTime.Now));
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

        //[Fact] TODO
        //public void ShouldNewReadUpdateMeterState()
        //{
        //    var address = CreateAddress();

        //    address.AddMeter("1234", 10.000);

        //    address.ProvideRead(Guid.Empty, 12.00, "1234", DateTime.Now);

        //    Assert.Equal(12.00, address.GetMeters().Single(i => i.Id == "1234").State);
        //}

        //[Fact] TODO
        //public void ShouldProvideReadCreateNewUsage()
        //{
        //    var address = CreateAddress();

        //    address.AddMeter("1234", 10.000);

        //    var usage = address.ProvideRead(Guid.Empty, 12.00, "1234", DateTime.Now);

        //    Assert.Equal(2.00, usage.Value);
        //}

        //[Fact] TODO
        //public void ShouldNotBeAbleToModifyMeterBesideAgregate()
        //{
        //    var address = CreateAddress();

        //    address.AddMeter("1234", 10.000);

        //    address.ProvideRead(Guid.Empty, 12.00, "1234", DateTime.Now);

        //    var meter = address.GetMeters().First();

        //    meter.State = 400.00;

        //    Assert.NotEqual(address.GetMeters().First().State, meter.State);
        //}

        //[Fact] TODO
        //public void ShouldThrowExceptionIfNoMeterFoundForProvidedRead()
        //{
        //    var address = CreateAddress();

        //    Assert.Throws<MeterNotFoundException>(() => address.ProvideRead(Guid.Empty, 12.00, "1234", DateTime.Now));
        //}

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