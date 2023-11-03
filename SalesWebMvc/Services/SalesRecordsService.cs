using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;

namespace SalesWebMVC.Services
{
    public class SalesRecordsService

    {
        private readonly SalesWebMVCContext _context;

        public SalesRecordsService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            return await CreateQuery(minDate, maxDate)
                .ToListAsync();
        }

        public async Task<List<IGrouping<Department?, SalesRecord>>> FindByDateGroupingByDepartmentsAsync(DateTime? minDate, DateTime? maxDate)
        {
            return await CreateQuery(minDate, maxDate)
                .GroupBy(salesRecord => salesRecord.Seller.Department)
                .ToListAsync();
        }

        private IQueryable<SalesRecord> CreateQuery(DateTime? minDate, DateTime? maxDate)
        {
            IQueryable<SalesRecord> queryableSalesRecord = from salesRecord in _context.SalesRecord select salesRecord;

            if (minDate.HasValue)
            {
                queryableSalesRecord = queryableSalesRecord.Where(salesRecord => salesRecord.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                queryableSalesRecord = queryableSalesRecord.Where(salesRecord => salesRecord.Date <= maxDate.Value);
            }

            return queryableSalesRecord
                .Include(salesRecord => salesRecord.Seller)
                .Include(salesRecord => salesRecord.Seller.Department)
                .OrderByDescending(salesRecord => salesRecord.Date);
        }
    }
}