using Core.Entities;
using Core.SqlSugar;
using Infrastructure.Extensions;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace AspDotNetCore3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ISqlSugarRepository sqlSugar;

        public AccountController(ILogger<AccountController> logger, ISqlSugarRepository sqlSugar)
        {
            _logger = logger;
            this.sqlSugar = sqlSugar;
        }

        [HttpGet("login")]
        public ActionResult Login(string account, string psw)
        {
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(psw))
            {
                return BadRequest();
            }

            if (sqlSugar.Get<User>(t => t.Account == account) is var user && user.IsNotNull())
            {
                if (!(MD5Helper.MD5Encrypt(psw) == user.Psw))
                {
                    throw new Exception("Log in failed");
                }
            }

            var di = new ClaimsIdentity("cookie");
            di.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            di.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            di.AddClaim(new Claim(ClaimTypes.MobilePhone, user.MobilePhone));
            di.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.NameIdentifier));
            di.AddClaim(new Claim(ClaimTypes.Role, user.Role));
            var cp = new ClaimsPrincipal(di);

            var properties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTime.Now.AddMinutes(1)
            };

            HttpContext.SignInAsync(cp, properties).Wait();
            
            var ticket = new AuthenticationTicket(cp, properties, "myScheme");
            
            AuthenticateResult.Success(ticket);

            return Content("Log in succeed");
        }


        [HttpGet("logout")]
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Content("Log out succeed");
        }
    }
}
