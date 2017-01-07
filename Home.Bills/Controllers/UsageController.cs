using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Home.Bills.DataAccess;
using Home.Bills.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Home.Bills.Controllers
{
    [Route("api/[controller]")]
    public class UsageController : Controller
    {
        private readonly IUsageDataProvider _usageDataProvider;

        public UsageController( IUsageDataProvider usageDataProvider)
        {
            _usageDataProvider = usageDataProvider;
        }

        [HttpGet("GetLastUsage/{addressId}", Name = "GetLastUsage")]
        public async Task<IEnumerable<Home.Bills.DataAccess.Dto.Usage>> GetLastUsage(Guid addressId)
        {
            return await _usageDataProvider.GetLastUsages(addressId);
        }

        [HttpGet("{addressId}")]
        public async Task<IEnumerable<Home.Bills.DataAccess.Dto.Usage>> Get(Guid addressId)
        {
            return await _usageDataProvider.GetUsages(addressId);
        }
    }
}