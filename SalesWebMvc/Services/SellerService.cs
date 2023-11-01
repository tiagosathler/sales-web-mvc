using SalesWebMVC.Data;
using SalesWebMVC.Models;

namespace SalesWebMVC.Services
{
	public class SellerService
	{
		private readonly SalesWebMVCContext _context;

		public SellerService(SalesWebMVCContext context)
		{
			_context = context;
		}

		public List<Seller> FindAll()
		{
			return _context.Seller.ToList();
		}

		public void Insert(Seller seller)
		{
			_context.Add(seller);
			_context.SaveChanges();
		}
	}
}