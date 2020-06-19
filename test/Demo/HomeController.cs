using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Demo
{
	public class HomeController : Controller
	{
		[Route("")]
		public IActionResult Index()
		{
			return Ok(new { Message = "This is an object property. "});
		}
	}
}
