using Microsoft.AspNetCore.Mvc;

namespace Musical_Theatre.Controllers
{
	public class HomeController : Controller
	{
		public HomeController()
		{
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}