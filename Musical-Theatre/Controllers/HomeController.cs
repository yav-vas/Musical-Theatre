using Microsoft.AspNetCore.Mvc;
using Musical_Theatre.Models;

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

		public IActionResult Error(string errorMessage)
		{
			ErrorViewModel errorModel = new ErrorViewModel(errorMessage);
			return View(errorModel);
		}
	}
}