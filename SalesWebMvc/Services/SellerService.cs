using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Services.Exceptions;

namespace SalesWebMVC.Services
{
	public class SellerService
	{
		private readonly SalesWebMVCContext _context;

		public SellerService(SalesWebMVCContext context)
		{
			_context = context;
		}

		public async Task<List<Seller>> FindAllAsync()
		{
			return await _context.Seller.ToListAsync();
		}

		public async Task InsertAsync(Seller seller)
		{
			_context.Add(seller);
			await _context.SaveChangesAsync();
		}

		public async Task<Seller?> FindByIdAsync(int? id)
		{
			return await _context.Seller.Include(s => s.Department).FirstOrDefaultAsync(s => s.Id == id);
		}

		public async Task RemoveAsync(int id)
		{
			try
			{
				Seller foundSeller = await _context.Seller.Include(s => s.Sales).FirstOrDefaultAsync(s => s.Id == id)
					?? throw new NotFoundException("Id not found");

				if (foundSeller.Sales.Count != 0)
				{
					throw new IntegrityException("Can't delete seller because he/she has sales");
				}

				_context.Seller.Remove(foundSeller);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException e)
			{
				throw new IntegrityException(e.Message);
			}
		}

		public async Task UpdateAsync(Seller seller)
		{
			bool containsSeller = await _context.Seller.ContainsAsync(seller);

			if (!containsSeller)
			{
				throw new NotFoundException("Id not found");
			}

			try
			{
				_context.Update(seller);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException e)
			{
				throw new DbConcurrencyException(e.Message);
			}
		}
	}
}