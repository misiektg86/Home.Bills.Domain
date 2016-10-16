using System;
using System.Linq;
using Home.Bills.Domain.AddressAggregate.Entities;
using Xunit;

namespace Home.Bills.Domain.Tests
{
    public class AddressTests
    {
        [Fact]
        public void ShouldAddNewMeter()
        {
            var address = Address.Create("strzelców bytomskich", "bytom", "141", "2");

            address.AddMeter("1234", 10.000);

            Assert.Contains(address.GetMeters(), i => i.Id == "1234");
        }

        [Fact]
        public void ShouldThrowExceptionIfMeterExists()
        {
            var address = Address.Create("strzelców bytomskich", "bytom", "141", "2");

            address.AddMeter("1234", 10.000);

            Assert.Throws<InvalidOperationException>(() => address.AddMeter("1234", 10.000));
        }

        [Fact]
        public void ShouldExchangeMeter()
        {
            var address = Address.Create("strzelców bytomskich", "bytom", "141", "2");

            address.AddMeter("1234", 10.000);

            address.ExchangeMeter("1234", "4321", 10.000);

            Assert.DoesNotContain(address.GetMeters(), i => i.Id == "1234");

            Assert.Contains(address.GetMeters(), i => i.Id == "4321");
        }

        [Fact]
        public void ShouldTrhowExceptionIfMeterDoesntExist()
        {
            var address = Address.Create("strzelców bytomskich", "bytom", "141", "2");

            Assert.Throws<InvalidOperationException>(() => address.ExchangeMeter("1234", "4321", 10.000));
        }

        [Fact]
        public void SohuldThrowExceptionIfNewReadLowerThanPrevious()
        {
            var address = Address.Create("strzelców bytomskich", "bytom", "141", "2");

            address.AddMeter("1234", 14.000);

            Assert.Throws<InvalidOperationException>(() => address.ProvideRead(12.00, "1234", DateTime.Now));
        }

        [Fact]
        public void ShouldNewReadUpdateMeterState()
        {
            var address = Address.Create("strzelców bytomskich", "bytom", "141", "2");

            address.AddMeter("1234", 10.000);

            address.ProvideRead(12.00, "1234", DateTime.Now);

            Assert.Equal(12.00, address.GetMeters().Single(i => i.Id == "1234").State);
        }

        [Fact]
        public void ShouldProvideReadCreateNewUsage()
        {
            var address = Address.Create("strzelców bytomskich", "bytom", "141", "2");

            address.AddMeter("1234", 10.000);

            address.ProvideRead(12.00, "1234", DateTime.Now);

            Assert.Equal(2.00, address.GetUsages().Last().Value);
        }

        [Fact]
        public void ShouldNotBeAbleToModifyMeterBesideAgregate()
        {
            var address = Address.Create("strzelców bytomskich", "bytom", "141", "2");

            address.AddMeter("1234", 10.000);

            address.ProvideRead(12.00, "1234", DateTime.Now);

            var meter = address.GetMeters().First();

            meter.State = 400.00;

            Assert.NotEqual(address.GetMeters().First().State, meter.State);
        }
    }
}