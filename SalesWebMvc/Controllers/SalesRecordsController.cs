using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
            List<SalesRecord> salesRecords = await _salesRecordsService
                .FindByDateAsync(minDate, maxDate);

            DateTime? defaultMinDate = minDate ?? SettingDefaultMinDate(salesRecords);

            SettingViewDataWithDates(defaultMinDate, maxDate);

            return View(salesRecords);
        }

        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            List<IGrouping<Department?, SalesRecord>> salesRecordsGroupedByDepartments = await _salesRecordsService
                .FindByDateGroupingByDepartmentsAsync(minDate, maxDate);

            DateTime? defaultMinDate = minDate ?? SettingDefaultMinDate(salesRecordsGroupedByDepartments);

            SettingViewDataWithDates(defaultMinDate, maxDate);

            return View(salesRecordsGroupedByDepartments);
        }

        private void SettingViewDataWithDates(DateTime? minDate, DateTime? maxDate)
        {
            DateTime minDateParsed = minDate ?? new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0, DateTimeKind.Local);
            DateTime maxDateParsed = maxDate ?? DateTime.Now;

            ViewData["minDate"] = minDateParsed.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDateParsed.ToString("yyyy-MM-dd");
        }

        private static DateTime? SettingDefaultMinDate(List<SalesRecord> list)
        {
            return !list.IsNullOrEmpty() ? list[^1].Date : null;
        }

        private static DateTime? SettingDefaultMinDate(List<IGrouping<Department?, SalesRecord>> list)
        {
            if (list.IsNullOrEmpty())
            {
                return null;
            }

            DateTime defaultMinDate = DateTime.MaxValue;

            foreach (IGrouping<Department?, SalesRecord> salesRecordsByDeparment in list)
            {
                if (salesRecordsByDeparment.IsNullOrEmpty())
                {
                    continue;
                }

                DateTime foundMinDate = salesRecordsByDeparment
                    .Where(salesRecord => salesRecord.Date <= defaultMinDate)
                    .Select(salesRecord => salesRecord.Date)
                    .OrderBy(d => d)
                    .FirstOrDefault(defaultMinDate);

                if (foundMinDate < defaultMinDate)
                {
                    defaultMinDate = foundMinDate;
                }
            }

            return defaultMinDate != DateTime.MaxValue ? defaultMinDate : null;
        }
    }
}