//using System;
//using System.Net;
//using System.Threading.Tasks;
//using Home.Bills.DataAccess;
//using Home.Bills.Domain.Services;
//using Microsoft.AspNetCore.Mvc;

//namespace Home.Bills.Controllers
//{
//    [Route("api/[controller]")]
//    public class UsageController : Controller
//    {
//        private readonly UsageDomainService _usageDomainService;
//        private readonly IUsageDataProvider _usageDataProvider;

//        public UsageController(UsageDomainService usageDomainService, IUsageDataProvider usageDataProvider)
//        {
//            _usageDomainService = usageDomainService;
//            _usageDataProvider = usageDataProvider;
//        }

//        [HttpGet("{addressId}", Name = "GetLastUsage")]
//        public async Task<Home.Bills.DataAccess.Dto.Usage> GetLastUsage(Guid addressId)
//        {
//            return await _usageDataProvider.GetLastUsage(addressId);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Post([FromBody]Models.Usage data)
//        {
//            await _usageDomainService.CreateUsageFromMeterRead(data.UsageId, data.AddressId, data.Value, data.MeterSerialNumber,
//                 data.ReadDateTime);

//            return CreatedAtRoute("GetLastUsage", new { data.AddressId }, data);
//        }
//    }
//}

//namespace Home.Bills.Models
//{
//    public class Usage
//    {
//        public Guid AddressId { get; set; }

//        public Guid UsageId { get; set; }

//        public double Value { get; set; }

//        public string MeterSerialNumber { get; set; }

//        public DateTime ReadDateTime { get; set; }
//    }
//}