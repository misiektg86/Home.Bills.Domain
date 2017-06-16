using System;
using System.Threading.Tasks;
using Home.Bills.Notification.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Home.Bills.Notifications.Controllers
{
    [Route("api/[controller]")]
    public class MailAccountController : Controller
    {
        private readonly SmtpAccountDataAccess _dataAccess;
        public MailAccountController(SmtpAccountDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        [HttpGet("/smtp/account/details", Name = "GetAccountDetails")]
        public async Task<IActionResult> GetAccountDetails()
        {
            return new ObjectResult(await _dataAccess.GetSmtpAccount());
        }

        [HttpPost("smtp/account/create", Name = "CreateSmtpAccount")]
        public async Task<IActionResult> CreateSmtpAccount([FromBody] Models.SmtpAccount account)
        {
            if (await _dataAccess.GetSmtpAccount() != null)
                return StatusCode(409);

            await _dataAccess.AddSmtpAccount(Convert(account));

            return StatusCode(201);
        }

        [HttpPut("smtp/account", Name = "EditSmtpAccount")]
        public async Task<IActionResult> EditSmtpAccount([FromBody] Models.SmtpAccount account)
        {
            if (await _dataAccess.GetSmtpAccount() == null)
                return StatusCode(409);

            await _dataAccess.EditSmtpAccount(Convert(account));

            return StatusCode(201);
        }

        private SmtpAccount Convert(Models.SmtpAccount source)
        {
            return new SmtpAccount
            {      
                Id = source.Id,
                EnableSSl = source.EnableSSl,
                FromAddress = source.FromAddress,
                Password = source.Password,
                Port = source.Port,
                Server = source.Server,
                UserName = source.UserName
            };
        }

        private Models.SmtpAccount Convert(SmtpAccount source)
        {
            return new Models.SmtpAccount
            {
                Id = source.Id,
                EnableSSl = source.EnableSSl,
                FromAddress = source.FromAddress,
                Password = "***",
                Port = source.Port,
                Server = source.Server,
                UserName = source.UserName
            };
        }
    }
}