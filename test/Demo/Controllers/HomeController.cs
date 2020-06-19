using System.Threading;
using System.Threading.Tasks;
using ActiveLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Demo.Models;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
	        _logger.LogInformation("Index was visited.");
	        return View();
        }

        public IActionResult Privacy()
        {
	        _logger.LogInformation("Privacy was visited.");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string requestId)
        {
            return View(new ErrorViewModel { RequestId = requestId });
        }

        public async Task<IActionResult> Logs([FromServices] LogService logService, CancellationToken cancellationToken = default)
        {
			var logs = await logService.GetAllLogsAsync("sqlite", cancellationToken);
	        return View(logs.Data);
        }
    }
}
