using System;
using System.Net;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.DataProviders;
using Home.Bills.Domain.AddressAggregate.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Home.Bills.Controllers
{
    [Route("api/[controller]")]
    public class AddressController : Controller
    {
        private readonly IRepository<Address, Guid> _addressRepository;

        private readonly IAddressDataProvider _addressDataProvider;

        public AddressController(IRepository<Address, Guid> addressRepository, IAddressDataProvider addressDataProvider)
        {
            _addressRepository = addressRepository;
            _addressDataProvider = addressDataProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            return new ObjectResult(await _addressDataProvider.GetAddresses());
        }


        [HttpGet("{id}", Name = "GetAddress")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _addressRepository.Get(id);

            if (item == null)
            {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Dtos.Address data)
        {
            if (await _addressDataProvider.AddressExists(data.City, data.Street, data.StreetNumber, data.HomeNumber))
            {
                return StatusCode((int)HttpStatusCode.Conflict);
            }

            var address = Address.Create(data.Street, data.City, data.StreetNumber, data.HomeNumber);

            _addressRepository.Add(address);

            return CreatedAtRoute("GetAddress", new { id = address.Id }, address);
        }

        [HttpPut("ProvideRead")]
        public async Task<IActionResult> ProvideRead([FromBody] Models.MeterRead meterRead)
        {
            var address = await _addressRepository.Get(meterRead.AddressId);

            if (address == null)
            {
                return NotFound(meterRead.AddressId);
            }

            address.ProvideRead(meterRead.Read, meterRead.MeterSerialNumber, meterRead.ReadDate);

            return StatusCode(204);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _addressRepository.Get(id) == null)
            {
                return NotFound(id);
            }

            _addressRepository.Delete(id);

            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}