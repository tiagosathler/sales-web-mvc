using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;

namespace SalesWebMVC.Controllers
{
	[AutoValidateAntiforgeryToken]
	public class SellersController : Controller
	{
		private readonly SellerService _sellerService;
		private readonly DepartmentService _departmentService;

		public SellersController(SellerService sellerService, DepartmentService departmentService)
		{
			_sellerService = sellerService;
			_departmentService = departmentService;
		}

		public IActionResult Index()
		{
			List<Seller> sellers = _sellerService.FindAll();
			return View(sellers);
		}

		public IActionResult Create()
		{
			List<Department> departments = _departmentService.FindAll();
			SellerFormViewModel viewModel = new(departments);
			return View(viewModel);
		}

		[HttpPost]
		public IActionResult Create(Seller seller)
		{
			_sellerService.Insert(seller);
			return RedirectToAction(nameof(Index));
		}
	}
}