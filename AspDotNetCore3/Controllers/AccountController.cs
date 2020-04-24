using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace AspDotNetCore3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet("login")]
        public ActionResult Login()
        {
            _logger.LogInformation("=========================Start Login =======================");
            var di = new ClaimsIdentity("cookie");
            di.AddClaim(new Claim(ClaimTypes.Name, "hal"));
            di.AddClaim(new Claim(ClaimTypes.Email, "hal_tan@163.com"));
            di.AddClaim(new Claim(ClaimTypes.MobilePhone, "1113333111"));
            di.AddClaim(new Claim(ClaimTypes.NameIdentifier, "xx1133-111x1gg"));
            di.AddClaim(new Claim(ClaimTypes.Role, "admin"));
            var cp = new ClaimsPrincipal(di);
            HttpContext.SignInAsync(cp).Wait();
            _logger.LogInformation("=========================Finish Login =======================");
            return Redirect("/Home/Index");
        }
    }
}
