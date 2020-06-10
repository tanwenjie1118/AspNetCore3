using Autofac;
using Hal.Core.Entities;
using Hal.Core.SqlSugar;
using Hal.Infrastructure.Configuration;
using Hal.Infrastructure.Extensions;
using Hal.Infrastructure.Helpers;
using Hal.Infrastructure.Singleton;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace Hal.AspDotNetCore3.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class SsoController : ControllerBase
    {
        private readonly ILogger<SsoController> _logger;
        private readonly ISqlSugarRepository sqlSugar;

        public SsoController(ILogger<SsoController> logger, ISqlSugarRepository sqlSugar)
        {
            _logger = logger;
            this.sqlSugar = sqlSugar;
        }

        [HttpPost("token")]
        public ActionResult Token(string account, string psw)
        {
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(psw))
            {
                return BadRequest();
            }

            if (sqlSugar.Get<User>(t => t.Account == account) is var user && user.IsNotNull())
            {
                if (!(MD5Helper.MD5Encrypt(psw) == user.Psw))
                {
                    throw new Exception("Login failed");
                }
            }

            // login succeed jwt encrypt
            var suopt = AutofacContainer.Container.Resolve<StartupOption>();
            TokenModelJwt tokenModel = new TokenModelJwt { Uid = user.Id, Role = user.Role };
            var jwtStr = JwtHelper.Encrypt(tokenModel, suopt.Jwt);
            return Ok(jwtStr);
        }
    }
}
