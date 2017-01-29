using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Notifications.Domain.AddressAggregate;
using Home.Bills.Notifications.Models;
using Microsoft.AspNetCore.Mvc;

namespace Home.Bills.Notifications.Controllers
{
    [Route("api/[controller]")]
    public class AddressController : Controller
    {
        private readonly IRepository<Address, Guid> _addressRepository;
        private readonly IAggregateFactory<Address, AddressFactoryInput, Guid> _addressFactory;

        public AddressController(IRepository<Address, Guid> addressRepository, IAggregateFactory<Address,AddressFactoryInput,Guid> addressFactory )
        {
            _addressRepository = addressRepository;
            _addressFactory = addressFactory;
        }

        [HttpGet("get/{addressId}",Name = "GetAddress")]
        public async Task<IActionResult> GetAddress(Guid addressId)
        {
            return new ObjectResult(await _addressRepository.Get(addressId));
        }

        [HttpPost("create",Name = "CreateAddress")]
        public IActionResult CreateAddress([FromBody] CreateAddress address)
        {
            var aggregate =
                _addressFactory.Create(new AddressFactoryInput
                {
                    AddressId = address.AddressId,
                    FullAddressName = address.FullAddress
                });

            _addressRepository.Add(aggregate);

            return CreatedAtRoute("GetAddress", new {address.AddressId}, aggregate);
        }

        [HttpPut("modify/adminEmail/{addressId}")]
        public async Task<IActionResult> SetEmail(Guid addressId, [FromBody]string email)
        {
            var entity = await _addressRepository.Get(addressId);

            entity.SetBuildingAdministratorEmail(email);

            _addressRepository.Update(entity);

            return StatusCode(204);
        }

        [HttpPut("modify/addressowner/{addressId}")]
        public async Task<IActionResult> SetAddressOwnerEmail(Guid addressId, [FromBody]string email)
        {
            var entity = await _addressRepository.Get(addressId);

            entity.SetAddressOwnerEmail(email);

            _addressRepository.Update(entity);

            return StatusCode(204);
        }
    }
}