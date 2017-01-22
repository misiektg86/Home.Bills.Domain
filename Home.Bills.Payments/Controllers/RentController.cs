using System;
using System.Linq;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Payments.Domain.RentAggregate;
using Home.Bills.Payments.Domain.Services;
using Home.Bills.Payments.Models;
using Microsoft.AspNetCore.Mvc;
using RentItem = Home.Bills.Payments.Models.RentItem;

namespace Home.Bills.Payments.Controllers
{
    [Route("api/[controller]")]
    public class RentController : Controller
    {
        private readonly IRepository<Rent, Guid> _rentRepository;
        private readonly IAggregateFactory<Rent, RentFactoryInput, Guid> _rentAggregateFactory;
        private readonly RentAssignDomainService _rentAssignDomainService;

        public RentController(IRepository<Rent, Guid> rentRepository,
            IAggregateFactory<Rent, RentFactoryInput, Guid> rentAggregateFactory, RentAssignDomainService rentAssignDomainService)
        {
            _rentRepository = rentRepository;
            _rentAggregateFactory = rentAggregateFactory;
            _rentAssignDomainService = rentAssignDomainService;
        }

        [HttpGet("{rentId}", Name = "GetRent")]
        public async Task<ActionResult> Get(Guid rentId)
        {
            return new ObjectResult(await _rentRepository.Get(rentId));
        }

        [HttpPost("CreateRent", Name = "CreateRent")]
        public ActionResult CreateRent([FromBody] CreateRent rent)
        {
            var entity =
                _rentAggregateFactory.Create(new RentFactoryInput()
                {
                    RentItems = rent.RentItems.Select(ToEntity).ToArray(),
                    RentId = rent.RentId,
                    ValidTo = rent.ValidTo,
                });

            _rentRepository.Add(entity);

            return CreatedAtRoute("GetRent", new { RentId = entity.Id }, entity);
        }

        [HttpPut("AssignToAddress/{addressId}/{rentId}")]
        public async Task<ActionResult> AssignToAddress(Guid addressId, Guid rentId)
        {
            await _rentAssignDomainService.AssignRent(addressId, rentId).ConfigureAwait(false);

            return StatusCode(204);
        }

        private Domain.RentAggregate.RentItem ToEntity(RentItem model)
        {
            return new Domain.RentAggregate.RentItem(model.Description, model.AmountPerUnit, model.ItemPosition, (Domain.RentAggregate.RentUnit)(int)model.RentUnit);
        }
    }
}