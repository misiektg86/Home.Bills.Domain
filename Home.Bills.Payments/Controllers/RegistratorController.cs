using System;
using System.Net;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Payments.DataAccess;
using Home.Bills.Payments.Domain.RegistratorAgregate;
using Microsoft.AspNetCore.Mvc;

namespace Home.Bills.Payments.Controllers
{
    [Route("api/[controller]")]
    public class RegistratorController : Controller
    {
        private readonly IRepository<Registrator, Guid> _repository;
        private readonly IAggregateFactory<Registrator, FactoryInput, Guid> _factory;
        private readonly IRegistratorDataProvider _registratorDataProvider;

        public RegistratorController(IRepository<Registrator, Guid> repository, IAggregateFactory<Registrator, FactoryInput, Guid> factory, IRegistratorDataProvider registratorDataProvider)
        {
            _repository = repository;
            _factory = factory;
            _registratorDataProvider = registratorDataProvider;
        }

        [HttpGet("{registratorId}", Name = "GetRegistrator")]
        public IActionResult Get(Guid registratorId)
        {
            return new ObjectResult(_repository.Get(registratorId));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new ObjectResult(await _registratorDataProvider.GetRegistrators());
        }

        [HttpPost]
        public IActionResult CreateRegistrator([FromBody] Models.CreateRegistrator model)
        {
            var entity =
                _factory.Create(new FactoryInput()
                {
                    AddressId = model.AddressId,
                    Description = model.Description,
                    RegistratorId = model.RegistratorId,
                    TariffId = model.TariffId
                });

            _repository.Add(entity);

            return CreatedAtRoute("GetRegistrator", new { model.RegistratorId }, entity);
        }

        [HttpPut("ChangeTariff/{registratorId}/{tariffId}", Name = "ChangeTariff")]
        public async Task<IActionResult> ChangeTariff(Guid registratorId, Guid tariffId)
        {
            var registrator = await _repository.Get(registratorId);

            registrator.ApplyTariff(tariffId);

            _repository.Update(registrator);

            return StatusCode(204);
        }
    }
}