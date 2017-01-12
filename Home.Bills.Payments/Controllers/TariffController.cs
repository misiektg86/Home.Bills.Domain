using System;
using Frameworks.Light.Ddd;
using Home.Bills.Payments.Domain.TariffAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Home.Bills.Payments.Controllers
{
    [Route("api/[controller]")]
    public class TariffController : Controller
    {
        private readonly IAggregateFactory<Tariff, TariffFactoryInput, Guid> _tariffFactory;
        private readonly IRepository<Tariff, Guid> _tariffRepository;

        public TariffController(IAggregateFactory<Tariff,TariffFactoryInput,Guid> tariffFactory, IRepository<Tariff,Guid> tariffRepository )
        {
            _tariffFactory = tariffFactory;
            _tariffRepository = tariffRepository;
        }
    }
}