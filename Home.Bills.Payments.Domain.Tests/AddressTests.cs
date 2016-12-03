using System;
using Home.Bills.Payments.Domain.AddressAggregate;
using Xunit;

namespace Home.Bills.Payments.Domain.Tests
{
    public class AddressTests
    {
        [Fact]
        public void ShouldBeAbleToApplyTariff()
        {
            Address address = Address.Create(Guid.NewGuid());

            string meterSerialNumber = "1234";

            address.ApplyTariff(meterSerialNumber, 10m);

            Assert.Equal(10m, address.GetTariff(meterSerialNumber));
        }

        [Fact]
        public void ShouldApplyNewTariffWhenMeterExists()
        {
            Address address = Address.Create(Guid.NewGuid());

            string meterSerialNumber = "1234";

            address.ApplyTariff(meterSerialNumber, 10m);

            Assert.Equal(10m, address.GetTariff(meterSerialNumber));

            address.ApplyTariff(meterSerialNumber, 20m);

            Assert.Equal(20m, address.GetTariff(meterSerialNumber));
        }
    }
}