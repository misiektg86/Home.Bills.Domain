//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Frameworks.Light.Ddd;
//using Home.Bills.Controllers;
//using Home.Bills.DataAccess;
//using Home.Bills.Domain.AddressAggregate;
//using Home.Bills.Domain.Services;
//using Home.Bills.Models;
//using MediatR;
//using NSubstitute;
//using Xunit;
//using Address = Home.Bills.Domain.AddressAggregate.Entities.Address;

//namespace Home.Bills.Tests
//{
//    public class UsageControllerTests
//    {

//        [Fact]
//        public async Task ShouldProvidedReadCreateUsage()
//        {
//            var addressRepository = Substitute.For<IRepository<Address, Guid>>();
//            var usageRepository = Substitute.For<IRepository<Domain.UsageAggregate.Usage, Guid>>();
//            var addressFactory = new AddressFactory(Substitute.For<IMediator>());

//            var addressEntity = addressFactory.Create(new AddressFactoryInput() { City = "test", Street = "test", HomeNumber = "test", StreetNumber = "test", Id = Guid.NewGuid() });

//            addressEntity.AddMeter("123", 10);

//            addressRepository.Get(addressEntity.Id).Returns(addressEntity);

//            UsageController usageController = new UsageController(new UsageDomainService(usageRepository, addressRepository), Substitute.For<IUsageDataProvider>());
//            var usageId = Guid.NewGuid();
//            await
//                usageController.Post(new Usage()
//                {
//                    AddressId = addressEntity.Id,
//                    MeterSerialNumber = "123",
//                    Value = 30,
//                    ReadDateTime = DateTime.Now,
//                    UsageId = usageId
//                });


//            usageRepository.Received(1).Add(Arg.Any<Domain.UsageAggregate.Usage>());
//        }
//    }
//}