using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;
using System.Diagnostics;

namespace SalesWebMVC.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class SellersController : Controller
    {
        private const string ID_NOT_FOUND = "Id not found";
        private const string ID_NOT_PROVIDER = "Id not provider";
        private const string ID_MISMATCH = "Id mismatch";

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
                return await RedirectToSellerFormView(seller);
            }

            try
            {
                await _sellerService.InsertAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToError(e.Message);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            return await RedirectToViewSellerOrToError(id);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException)
            {
                return RedirectToError(ID_NOT_FOUND);
            }
            catch (IntegrityException e)
            {
                return RedirectToError(e.Message);
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            return await RedirectToViewSellerOrToError(id);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToError(ID_NOT_PROVIDER);
            }

            try
            {
                Seller seller = await _sellerService.FindByIdAsync(id.Value);
                return await RedirectToSellerFormView(seller);
            }
            catch (NotFoundException)
            {
                return RedirectToError(ID_NOT_FOUND);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                return await RedirectToSellerFormView(seller);
            }

            if (id != seller.Id)
            {
                return RedirectToError(ID_MISMATCH);
            }

            try
            {
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException)
            {
                return RedirectToError(ID_NOT_FOUND);
            }
            catch (IntegrityException e)
            {
                return RedirectToError(e.Message);
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

        private async Task<IActionResult> RedirectToViewSellerOrToError(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToError(ID_NOT_PROVIDER);
            }

            try
            {
                Seller seller = await _sellerService.FindByIdAsync(id.Value);
                return View(seller);
            }
            catch (NotFoundException)
            {
                return RedirectToError(ID_NOT_FOUND);
            }
        }

        private IActionResult RedirectToError(string message)
        {
            return RedirectToAction(nameof(Error), new { message });
        }

        private async Task<IActionResult> RedirectToSellerFormView(Seller seller)
        {
            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel sellerFormViewModel = new(departments, seller);
            return View(sellerFormViewModel);
        }
    }
}