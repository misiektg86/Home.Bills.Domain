using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Payments.DataAccess.Dtos;
using Home.Bills.Payments.Domain.TariffAggregate;
using Microsoft.AspNetCore.Mvc;
using Tariff = Home.Bills.Payments.Domain.TariffAggregate.Tariff;

namespace Home.Bills.Payments.Controllers
{
    [Route("api/[controller]")]
    public class TariffController : Controller
    {
        private readonly IAggregateFactory<Tariff, TariffFactoryInput, Guid> _tariffFactory;
        private readonly IRepository<Tariff, Guid> _tariffRepository;
        private readonly ITariffDataProvider _tariffDataProvider;

        public TariffController(IAggregateFactory<Tariff, TariffFactoryInput, Guid> tariffFactory, IRepository<Tariff, Guid> tariffRepository, ITariffDataProvider tariffDataProvider)
        {
            _tariffFactory = tariffFactory;
            _tariffRepository = tariffRepository;
            _tariffDataProvider = tariffDataProvider;
        }

        [HttpGet("{tariffId}",Name = "GetTariff")]
        public async Task<IActionResult> Get(Guid tariffId)
        {
            return new ObjectResult(await _tariffDataProvider.GetTariff(tariffId));
        }

        [HttpPost]
        public IActionResult CreateTariff([FromBody]CreateTariff command)
        {
            var tariff =
                _tariffFactory.Create(new TariffFactoryInput()
                {
                    Created = DateTime.Now,
                    Description = command.Description,
                    TariffId = command.TariffId,
                    TariffValue = command.TariffPrice,
                    ValidTo = command.ValidTo
                });

            _tariffRepository.Add(tariff);

            return CreatedAtRoute("GetTariff", new { TariffId = tariff.Id }, tariff);
        }
    }

    public class CreateTariff
    {
        public Guid TariffId { get; set; }
        public Guid AddressId { get; set; }
        public decimal TariffPrice { get; set; }
        public string Description { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}