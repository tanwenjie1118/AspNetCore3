using Hal.Infrastructure.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hal.AspDotNetCore3.Controllers
{
    [HiddenApi]
    [ApiController]
    [Route("[controller]")]
    public class SignalController : Controller
    {
        private readonly ILogger<SignalController> _logger;

        public SignalController(ILogger<SignalController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult SignalR()
        {
            return View();
        }
    }
}
