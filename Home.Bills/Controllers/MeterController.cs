using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.DataAccess;
using Home.Bills.Domain.MeterAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Home.Bills.Controllers
{
    [Route("api/[controller]")]
    public class MeterController : Controller
    {
        private readonly IRepository<Meter, Guid> _meterRepository;
        private readonly IAggregateFactory<Meter, MeterFactoryInput, Guid> _meterFactory;
        private readonly IMeterDataProvider _meterDataProvider;

        public MeterController(IRepository<Meter, Guid> meterRepository, IAggregateFactory<Meter, MeterFactoryInput, Guid> meterFactory, IMeterDataProvider meterDataProvider)
        {
            _meterRepository = meterRepository;
            _meterFactory = meterFactory;
            _meterDataProvider = meterDataProvider;
        }

        [HttpGet("{meterId}", Name = "GetMeter")]
        public IActionResult GetMeter(Guid meterId)
        {
            return new ObjectResult(_meterDataProvider.GetMeter(meterId));
        }

        [HttpGet(Name = "GetMeters")]
        public IActionResult GetMeter()
        {
            return new ObjectResult(_meterDataProvider.GetAllMeters());
        }

        [HttpPost]
        public IActionResult Post([FromBody]Models.MeterCreate meter)
        {
            _meterRepository.Add(_meterFactory.Create(new MeterFactoryInput() { AddressId = meter.AddressId, MeterId = meter.MeterId, SerialNumber = meter.SerialNumber, State = meter.State }));

            return CreatedAtRoute("GetMeter", new { meterId = meter.MeterId }, meter);
        }

        [HttpPut("MountMeterAtAddress", Name = "MountMeterAtAddress")]
        public async Task<IActionResult> MountMeterAtAddress([FromBody]Models.MeterMount meter)
        {
            var meterEntity = await _meterRepository.Get(meter.MeterId);

            if (meterEntity == null)
            {
                return NotFound(meter.MeterId);
            }

            meterEntity.MountAtAddress(meter.AddressId);

            _meterRepository.Update(meterEntity);

            return NoContent();
        }

        [HttpPut("UnmountMeterAtAddress", Name = "UnmountMeterAtAddress")]
        public async Task<IActionResult> UnmountMeterAtAddress([FromBody]Models.MeterMount meter)
        {
            var meterEntity = await _meterRepository.Get(meter.MeterId);

            if (meterEntity == null)
            {
                return NotFound(meter.MeterId);
            }

            meterEntity.UnmountAtAddress(meter.AddressId);

            _meterRepository.Update(meterEntity);

            return NoContent();
        }

        [HttpPut("UpdateMeterState", Name = "UpdateMeterState")]
        public async Task<IActionResult> UpdateMeterState([FromBody]Models.MeterState meter)
        {
            var meterEntity = await _meterRepository.Get(meter.MeterId);

            if (meterEntity == null)
            {
                return NotFound(meter.MeterId);
            }

            meterEntity.UpdateState(meter.State);

            _meterRepository.Update(meterEntity);

            return NoContent();
        }

        [HttpPut("CorrectMeterState", Name = "CorrectMeterState")]
        public async Task<IActionResult> CorrectMeterState([FromBody]Models.MeterState meter)
        {
            var meterEntity = await _meterRepository.Get(meter.MeterId);

            if (meterEntity == null)
            {
                return NotFound(meter.MeterId);
            }

            meterEntity.CorrectState(meter.State);

            _meterRepository.Update(meterEntity);

            return NoContent();
        }

        [HttpGet("GetAllMeters/{addressId}", Name = "GetAllMeters")]
        public async Task<IActionResult> GetAllMeters(Guid addressId)
        {
            return new ObjectResult(await _meterDataProvider.GetAllMeters(addressId));
        }
    }
}