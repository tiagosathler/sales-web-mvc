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

		public async Task<IActionResult> Index()
		{
			List<Seller> sellers = await _sellerService.FindAllAsync();
			return View(sellers);
		}

		public async Task<IActionResult> Create()
		{
			List<Department> departments = await _departmentService.FindAllAsync();
			SellerFormViewModel sellerFormViewModel = new(departments);
			return View(sellerFormViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Create(Seller seller)
		{
			if (!ModelState.IsValid)
			{
				List<Department> departments = await _departmentService.FindAllAsync();
				SellerFormViewModel sellerFormViewModel = new(departments, seller);
				return View(sellerFormViewModel);
			}

			await _sellerService.InsertAsync(seller);
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not provided" });
			}
			Seller? seller = await _sellerService.FindByIdAsync(id);

			if (seller == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not found" });
			}
			return View(seller);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			await _sellerService.RemoveAsync(id);
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not provided" });
			}
			Seller? seller = await _sellerService.FindByIdAsync(id);

			if (seller == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not found" });
			}
			return View(seller);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not provided" });
			}
			Seller? seller = await _sellerService.FindByIdAsync(id);

			if (seller == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not found" });
			}

			List<Department> departments = await _departmentService.FindAllAsync();
			SellerFormViewModel sellerFormViewModel = new(departments, seller);

			return View(sellerFormViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, Seller seller)
		{
			if (!ModelState.IsValid)
			{
				List<Department> departments = await _departmentService.FindAllAsync();
				SellerFormViewModel sellerFormViewModel = new(departments, seller);
				return View(sellerFormViewModel);
			}
			if (id != seller.Id)
			{
				return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
			}

			try
			{
				await _sellerService.UpdateAsync(seller);
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