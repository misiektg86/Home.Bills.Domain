//using System;
//using System.Threading.Tasks;
//using Frameworks.Light.Ddd;
//using Home.Bills.Domain.AddressAggregate.Entities;
//using Home.Bills.Domain.AddressAggregate.Exceptions;
//using Home.Bills.Domain.UsageAggregate;

//namespace Home.Bills.Domain.Services
//{
//    public class UsageDomainService
//    {
//        private readonly IRepository<Usage, Guid> _usageRepository;

//        private readonly IRepository<Address, Guid> _addressRepository;

//        public UsageDomainService(IRepository<Usage, Guid> usageRepository, IRepository<Address, Guid> addressRepository)
//        {
//            _usageRepository = usageRepository;
//            _addressRepository = addressRepository;
//        }

//        public async Task CreateUsageFromMeterRead(Guid usageId, Guid addressId, double read, string meterSerialNumber, DateTime readDateTime)
//        {
//            var address = await _addressRepository.Get(addressId);

//            if (address == null)
//            {
//                throw new AddressNotFoundException(addressId.ToString());
//            }

//            var usage = address.ProvideRead(usageId, read, meterSerialNumber, readDateTime);

//            _addressRepository.Update(address);

//            _usageRepository.Add(usage);
//        }
//    }
//}