﻿using System;
using System.Net;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.DataAccess;
using Home.Bills.Domain.AddressAggregate;
using Home.Bills.Models;
using Microsoft.AspNetCore.Mvc;
using Address = Home.Bills.Domain.AddressAggregate.Address;

namespace Home.Bills.Controllers
{
    [Route("api/[controller]")]
    public class AddressController : Controller
    {
        private readonly IRepository<Address, Guid> _addressRepository;

        private readonly IAddressDataProvider _addressDataProvider;
        private readonly IAggregateFactory<Address, AddressFactoryInput, Guid> _addressFactory;
        private readonly IMeterReadDataProvider _meterReadDataProvider;

        public AddressController(IRepository<Address, Guid> addressRepository, IAddressDataProvider addressDataProvider, IAggregateFactory<Address, AddressFactoryInput, Guid> addressFactory, IMeterReadDataProvider meterReadDataProvider)
        {
            _addressRepository = addressRepository;
            _addressDataProvider = addressDataProvider;
            _addressFactory = addressFactory;
            _meterReadDataProvider = meterReadDataProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            return new ObjectResult(await _addressDataProvider.GetAddresses());
        }


        [HttpGet("{id}", Name = "GetAddress")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _addressDataProvider.GetAddress(id);

            if (item == null)
            {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Models.Address data)
        {
            if (await _addressDataProvider.AddressExists(data.City, data.Street, data.StreetNumber, data.HomeNumber))
            {
                return StatusCode((int)HttpStatusCode.Conflict);
            }

            var address = _addressFactory.Create(new AddressFactoryInput() { Street = data.Street, City = data.City, StreetNumber = data.StreetNumber, HomeNumber = data.HomeNumber, Id = Guid.NewGuid(), SquareMeters = data.SquareMeters });

            _addressRepository.Add(address);

            return CreatedAtRoute("GetAddress", new { id = address.Id }, address);
        }

        [HttpPut("BeginMeterReadProcess",Name = "BeginMeterReadProcess")]
        public async Task<IActionResult> BeginMeterReadProcess([FromBody] MeterRead meter)
        {
            var address = await _addressRepository.Get(meter.AddressId);

            if (address == null)
            {
                return NotFound(meter.AddressId);
            }

            address.BeginMeterReadProcess(meter.MeterReadId);

            _addressRepository.Update(address);

            return StatusCode(204);
        }

        [HttpPut("SetLastMeterRead/{addressId}/{meterReadId}", Name = "SetLastMeterRead")]
        public async Task<IActionResult> SetLastMeterRead(Guid addressId, Guid meterReadId)
        {
            var meterRead = await _meterReadDataProvider.GetMeterRead(meterReadId);

            if (meterRead == null)
            {
                return NotFound(meterReadId);
            }

            var address = await _addressRepository.Get(addressId);

            if (address == null)
            {
                return NotFound(addressId);
            }

            address.SetLastMeterRead(meterReadId);

            _addressRepository.Update(address);

            return new NoContentResult();
        }

        [HttpPut("FinishMeterReadProcess", Name = "FinishMeterReadProcess")]
        public async Task<IActionResult> FinishMeterReadProcess([FromBody] MeterRead meter)
        {
            var address = await _addressRepository.Get(meter.AddressId);

            if (address == null)
            {
                return NotFound(meter.AddressId);
            }

            address.FinishMeterReadProcess(meter.MeterReadId);

            _addressRepository.Update(address);

            return StatusCode(204);
        }

        [HttpPut("CancelMeterReadProcess", Name = "CancelMeterReadProcess")]
        public async Task<IActionResult> CancelMeterReadProcess([FromBody] MeterRead meter)
        {
            var address = await _addressRepository.Get(meter.AddressId);

            if (address == null)
            {
                return NotFound(meter.AddressId);
            }

            address.CancelMeterReadProcess(meter.MeterReadId);

            _addressRepository.Update(address);

            return StatusCode(204);
        }

        [HttpPut("AssignMeter")]
        public async Task<IActionResult> AssignMeter([FromBody] Meter meter)
        {
            var address = await _addressRepository.Get(meter.AddressId);

            if (address == null)
            {
                return NotFound(meter.AddressId);
            }

            address.AssignMeter(meter.MeterId);

            _addressRepository.Update(address);

            return StatusCode(204);
        }

        [HttpPut("CheckIn")]
        public async Task<IActionResult> CheckInPerson([FromBody] CheckIn checkIn)
        {
            var address = await _addressRepository.Get(checkIn.AddressId);

            if (address == null)
            {
                return NotFound(checkIn.AddressId);
            }

            address.CheckInPersons(checkIn.Persons);

            _addressRepository.Update(address);

            return StatusCode(204);
        }

        [HttpPut("CheckOut")]
        public async Task<IActionResult> CheckOutPerson([FromBody] CheckOut checkOut)
        {
            var address = await _addressRepository.Get(checkOut.AddressId);

            if (address == null)
            {
                return NotFound(checkOut.AddressId);
            }

            address.CheckOutPersons(checkOut.Persons);

            _addressRepository.Update(address);

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