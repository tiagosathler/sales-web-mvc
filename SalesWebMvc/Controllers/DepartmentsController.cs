using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;
using System.Diagnostics;

namespace SalesWebMVC.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class DepartmentsController : Controller
    {
        private const string ID_NOT_FOUND = "Id not found";
        private const string ID_NOT_PROVIDER = "Id not provider";
        private const string ID_MISMATCH = "Id mismatch";

        private readonly DepartmentService _departmentService;

        public DepartmentsController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            List<Department> departments = await _departmentService.FindAllAsync();
            return View(departments);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToError(ID_NOT_PROVIDER);
            }
            try
            {
                Department department = await _departmentService.FindByIdAsync(id.Value);
                return View(department);
            }
            catch (NotFoundException)
            {
                return Redirect(ID_NOT_FOUND);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name")] Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }

            try
            {
                await _departmentService.InsertAsync(department);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToError(e.Message);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await RedirectToDeparmentViewOrError(id);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Department department)
        {
            if (id != department.Id)
            {
                return RedirectToError(ID_MISMATCH);
            }

            if (!ModelState.IsValid)
            {
                return View(department);
            }

            try
            {
                await _departmentService.UpdateAsync(department);
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

        public async Task<IActionResult> Delete(int? id)
        {
            return await RedirectToDeparmentViewOrError(id);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _departmentService.RemoveAsync(id);
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

        private IActionResult RedirectToError(string message)
        {
            return RedirectToAction(nameof(Error), new { message });
        }

        private async Task<IActionResult> RedirectToDeparmentViewOrError(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToError(ID_NOT_PROVIDER);
            }

            try
            {
                Department department = await _departmentService.FindByIdAsync(id.Value);
                return View(department);
            }
            catch (NotFoundException)
            {
                return RedirectToError(ID_NOT_FOUND);
            }
        }
    }
}