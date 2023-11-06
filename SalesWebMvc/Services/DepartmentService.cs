using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Services.Exceptions;

namespace SalesWebMVC.Services
{
    public class DepartmentService
    {
        private readonly SalesWebMVCContext _context;

        public DepartmentService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> FindAllAsync()
        {
            return await _context.Department
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public async Task<Department> FindByIdAsync(int id)
        {
            return await _context.Department
                .FirstOrDefaultAsync(d => d.Id == id)
                ?? throw new NotFoundException("Id not found");
        }

        public async Task InsertAsync(Department department)
        {
            try
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task UpdateAsync(Department department)
        {
            bool containsDepartment = await _context.Department.ContainsAsync(department);

            if (!containsDepartment)
            {
                throw new NotFoundException("Id not found");
            }

            try
            {
                _context.Update(department);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task RemoveAsync(int id)
        {
            Department department = await _context.Department
                .Include(d => d.Sellers)
                .FirstOrDefaultAsync(d => d.Id == id)
                ?? throw new NotFoundException("Id not found");

            if (department.Sellers.Count != 0)
            {
                throw new IntegrityException("It is not possible to delete the department because there are sellers associated with it");
            }

            try
            {
                _context.Remove(department);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }
        }
    }
}