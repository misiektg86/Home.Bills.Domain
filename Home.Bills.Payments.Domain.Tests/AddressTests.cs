using System;
using System.Linq;
using Home.Bills.Infrastructure;
using Home.Bills.Payments.Domain.AddressAggregate;
using MassTransit;
using NSubstitute;
using Xunit;

namespace Home.Bills.Payments.Domain.Tests
{
    public class AddressTests : IClassFixture<MartenDatabaseFixture>
    {
        private readonly MartenDatabaseFixture _martenDatabaseFixture;

        private AddressFactory _addressFactory;

        public AddressTests(MartenDatabaseFixture martenDatabaseFixture)
        {
            _martenDatabaseFixture = martenDatabaseFixture;

            _addressFactory = new AddressFactory(Substitute.For<IBus>());
        }

        [Fact]
        public void ShouldBeAbleToApplyTariff()
        {
            var address = CreateAddress();

            string meterSerialNumber = "1234";

            address.ApplyTariff(meterSerialNumber, 10m);

            Assert.Equal(10m, address.GetTariff(meterSerialNumber));
        }

        [Fact]
        public void ShouldApplyNewTariffWhenMeterExists()
        {
            var address = CreateAddress();

            string meterSerialNumber = "1234";

            address.ApplyTariff(meterSerialNumber, 10m);

            Assert.Equal(10m, address.GetTariff(meterSerialNumber));

            address.ApplyTariff(meterSerialNumber, 20m);

            Assert.Equal(20m, address.GetTariff(meterSerialNumber));
        }

        [Fact]
        public void ShouldThrowExceptionIfTariffNotAssignedAndUsageRegistered()
        {
            var address = CreateAddress();

            string meterSerialNumber = "1234";

            Assert.Throws<TariffNotAssignedException>(() => address.RegisterUsage(meterSerialNumber, 10.00));
        }

        [Fact]
        public void ShouldAddUsage()
        {
            var address = CreateAddress();

            string meterSerialNumber = "1234";

            address.ApplyTariff(meterSerialNumber, 10m);

            address.RegisterUsage(meterSerialNumber, 10.00);

            var usage = address.GetUsages().FirstOrDefault(i => i.MeterSerialNumber == meterSerialNumber);

            Assert.NotNull(usage);

            Assert.Equal(100m, usage.AmountToPay);
        }

        [Fact]
        public void ShouldThrowExceptionIfMonthAccepted()
        {
            var address = CreateAddress();

            string meterSerialNumber = "1234";

            address.ApplyTariff(meterSerialNumber, 10m);

            address.RegisterUsage(meterSerialNumber, 10.00);

            address.AcceptUsageForMonth();

            Assert.Throws<BillForMonthCurrentlyAcceptedException>(() => address.AcceptUsageForMonth());
        }

        [Fact]
        public void ShouldAcceptMonthIfNotAccepted()
        {
            var address = CreateAddress();

            string meterSerialNumber = "1234";

            address.ApplyTariff(meterSerialNumber, 10m);

            address.RegisterUsage(meterSerialNumber, 10.00);

            address.AcceptUsageForMonth();
        }

        [Fact]
        public void ShouldNewUsageOpenPaymentForAMonth()
        {
            var address = CreateAddress();

            string meterSerialNumber = "1234";

            address.ApplyTariff(meterSerialNumber, 10m);

            address.RegisterUsage(meterSerialNumber, 10.00);

            address.AcceptUsageForMonth();

            address.RegisterUsage(meterSerialNumber, 20.00);

            address.AcceptUsageForMonth();
        }

        [Fact]
        public void ShouldPersistAddressAggregate()
        {
            var session = _martenDatabaseFixture.DocumentStore.OpenSession();
            var addressRepository = new GenericMartenRepository<Address>(session, Substitute.For<IBus>());

            var address = CreateAddress();

            Guid addressId = address.Id;

            string meterSerialNumber = "1234";

            address.ApplyTariff(meterSerialNumber, 10m);

            address.RegisterUsage(meterSerialNumber, 10.00);

            address.AcceptUsageForMonth();

            addressRepository.Add(address);

            session.SaveChanges();

            session.Dispose();

            session = _martenDatabaseFixture.DocumentStore.OpenSession();

            addressRepository = new GenericMartenRepository<Address>(session, Substitute.For<IBus>());

            address = addressRepository.Get(addressId).Result;

            Assert.NotNull(address);

            var usage = address.GetUsages().FirstOrDefault();

            Assert.NotNull(usage);
            Assert.Equal(10, usage.Value);
            Assert.Equal(100m, usage.AmountToPay);
        }

        private Address CreateAddress()
        {
            Address address = _addressFactory.Create(new AddressFactoryInput() { AddressId = Guid.NewGuid(), SquareMeters = 50.00 });
            return address;
        }
    }
}