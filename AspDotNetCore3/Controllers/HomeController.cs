using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspDotNetCore3.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("index")]
        public string Index()
        {
            return "This is Index Page";
        }

        [HttpGet("signal")]
        public IActionResult SignalR()
        {
            return View("SignalRMock");
        }
    }
}
