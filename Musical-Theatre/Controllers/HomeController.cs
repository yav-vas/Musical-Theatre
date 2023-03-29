using Microsoft.AspNetCore.Mvc;
using Musical_Theatre.Models;
using System.Diagnostics;

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

		public IActionResult Privacy()
		{
			return View();
		}
	}
}