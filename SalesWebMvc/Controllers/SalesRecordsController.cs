using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Services;

namespace SalesWebMVC.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordsService _salesRecordsService;

        public SalesRecordsController(SalesRecordsService salesRecordsService)
        {
            _salesRecordsService = salesRecordsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            List<SalesRecord> salesRecords = await _salesRecordsService.FindByDateAsync(minDate, maxDate);

            DateTime minDateParsed = minDate ?? new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0, DateTimeKind.Local);
            DateTime maxDateParsed = maxDate ?? DateTime.Now;

            ViewData["minDate"] = minDateParsed.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDateParsed.ToString("yyyy-MM-dd");

            return View(salesRecords);
        }

        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            List<IGrouping<Department?, SalesRecord>> salesRecords = await _salesRecordsService.FindByDateGroupingAsync(minDate, maxDate);

            DateTime minDateParsed = minDate ?? new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0, DateTimeKind.Local);
            DateTime maxDateParsed = maxDate ?? DateTime.Now;

            ViewData["minDate"] = minDateParsed.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDateParsed.ToString("yyyy-MM-dd");

            return View(salesRecords);
        }
    }
}