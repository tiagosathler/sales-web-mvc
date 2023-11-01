using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;

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

		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			Seller? seller = _sellerService.FindById(id);

			if (seller == null)
			{
				return NotFound();
			}
			return View(seller);
		}

		public IActionResult Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			Seller? seller = _sellerService.FindById(id);

			if (seller == null)
			{
				return NotFound();
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
				return NotFound();
			}
			Seller? seller = _sellerService.FindById(id);

			if (seller == null)
			{
				return NotFound();
			}

			List<Department> departments = _departmentService.FindAll();
			SellerFormViewModel sellerFormViewModel = new(departments)
			{
				Seller = seller
			};

			return View(sellerFormViewModel);
		}

		[HttpPost]
		public IActionResult Edit(int id, Seller seller)
		{
			if (id != seller.Id)
			{
				return BadRequest();
			}
			try
			{
				_sellerService.Update(seller);
				return RedirectToAction(nameof(Index));
			}
			catch (NotFoundException)
			{
				return NotFound();
			}
			catch (DbConcurrencyException)
			{
				return BadRequest();
			}
		}
	}
}