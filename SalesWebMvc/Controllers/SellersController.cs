using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using System.Diagnostics;

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
			SellerFormViewModel sellerFormViewModel = new(departments);
			return View(sellerFormViewModel);
		}

		[HttpPost]
		public IActionResult Create(Seller seller)
		{
			if (!ModelState.IsValid)
			{
				List<Department> departments = _departmentService.FindAll();
				SellerFormViewModel sellerFormViewModel = new(departments, seller);
				return View(sellerFormViewModel);
			}

			_sellerService.Insert(seller);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not provided" });
			}
			Seller? seller = _sellerService.FindById(id);

			if (seller == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not found" });
			}
			return View(seller);
		}

		public IActionResult Details(int? id)
		{
			if (id == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not provided" });
			}
			Seller? seller = _sellerService.FindById(id);

			if (seller == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not found" });
			}
			return View(seller);
		}

		[HttpPost]
		public IActionResult Delete(int id)
		{
			_sellerService.Remove(id);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not provided" });
			}
			Seller? seller = _sellerService.FindById(id);

			if (seller == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not found" });
			}

			List<Department> departments = _departmentService.FindAll();
			SellerFormViewModel sellerFormViewModel = new(departments, seller);

			return View(sellerFormViewModel);
		}

		[HttpPost]
		public IActionResult Edit(int id, Seller seller)
		{
			if (!ModelState.IsValid)
			{
				List<Department> departments = _departmentService.FindAll();
				SellerFormViewModel sellerFormViewModel = new(departments, seller);
				return View(sellerFormViewModel);
			}
			if (id != seller.Id)
			{
				return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
			}

			try
			{
				_sellerService.Update(seller);
				return RedirectToAction(nameof(Index));
			}
			catch (ApplicationException e)
			{
				return RedirectToAction(nameof(Error), new { message = e.Message });
			}
		}

		public IActionResult Error(string message)
		{
			ErrorViewModel errorViewModel = new()
			{
				Message = message,
				RequestId = Activity.Current?.Id,
			};
			return View(errorViewModel);
		}
	}
}