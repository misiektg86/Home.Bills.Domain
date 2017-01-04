using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Home.Bills.DataAccess;
using Home.Bills.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Home.Bills.Controllers
{
    [Route("api/[controller]")]
    public class UsageController : Controller
    {
        private readonly UsageDomainService _usageDomainService;
        private readonly IUsageDataProvider _usageDataProvider;

        public UsageController(UsageDomainService usageDomainService, IUsageDataProvider usageDataProvider)
        {
            _usageDomainService = usageDomainService;
            _usageDataProvider = usageDataProvider;
        }

        [HttpGet("{addressId}", Name = "GetLastUsage")]
        public async Task<IEnumerable<Home.Bills.DataAccess.Dto.Usage>> GetLastUsage(Guid addressId)
        {
            return await _usageDataProvider.GetLastUsages(addressId);
        }
    }
}

namespace Home.Bills.Models
{
    public class Usage
    {
        public Guid AddressId { get; set; }

        public Guid UsageId { get; set; }

        public Guid PreviousUsageId { get; set; }

        public string MeterSerialNumber { get; set; }

        public Guid MeterId { get; set; }

        public DateTime ReadDateTime { get; set; }
    }
}