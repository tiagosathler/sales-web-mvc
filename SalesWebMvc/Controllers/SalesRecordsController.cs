using Microsoft.AspNetCore.Mvc;

namespace SalesWebMVC.Controllers
{
	[AutoValidateAntiforgeryToken]
	public class SalesRecordsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult SimpleSearch()
		{
			return View();
		}

		public IActionResult GroupingSearch()
		{
			return View();
		}
	}
}