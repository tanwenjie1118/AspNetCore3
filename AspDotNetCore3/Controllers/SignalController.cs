using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspDotNetCore3.Controllers
{
    [Authorize]
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
            return View("SignalRMock");
        }
    }
}
