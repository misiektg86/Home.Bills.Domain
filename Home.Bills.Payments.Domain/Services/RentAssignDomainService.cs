using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Payments.Domain.AddressAggregate;
using Home.Bills.Payments.Domain.RentAggregate;

namespace Home.Bills.Payments.Domain.Services
{
    public class RentAssignDomainService
    {
        private readonly IRepository<Address, Guid> _addressRepository;
        private readonly IRepository<Rent, Guid> _rentRepository;

        public RentAssignDomainService(IRepository<Address, Guid> addressRepository, IRepository<Rent, Guid> rentRepository)
        {
            _addressRepository = addressRepository;
            _rentRepository = rentRepository;
        }

        public async Task AssignRent(Guid addressId, Guid rentId)
        {
            var address = await _addressRepository.Get(addressId);

            if (address == null)
            {
                throw new CannotAssignRentToAddressException(addressId.ToString(), new AddressNotFoundException(addressId.ToString()));
            }

            var rent = await _rentRepository.Get(rentId);

            if (rent == null)
            {
                throw new CannotAssignRentToAddressException(addressId.ToString(), new RentNotFoundException(rentId.ToString()));
            }

            if (rent.HasExpired())
            {
                throw new CannotAssignRentToAddressException(addressId.ToString(), new RentHasExpiredException(rentId.ToString()));
            }

            address.ApplyRent(rent.Id);

            _addressRepository.Update(address);
        }
    }
}