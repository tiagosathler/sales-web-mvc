﻿using Microsoft.EntityFrameworkCore;
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

		public List<Seller> FindAll()
		{
			return _context.Seller.ToList();
		}

		public void Insert(Seller seller)
		{
			_context.Add(seller);
			_context.SaveChanges();
		}

		public Seller? FindById(int? id)
		{
			return _context.Seller.Include(s => s.Department).FirstOrDefault(s => s.Id == id);
		}

		public void Remove(int id)
		{
			Seller? foundSeller = _context.Seller.Find(id);
			if (foundSeller != null)
			{
				_context.Seller.Remove(foundSeller);
				_context.SaveChanges();
			}
		}

		public void Update(Seller seller)
		{
			if (!_context.Seller.Contains(seller))
			{
				throw new NotFoundException("Id not found");
			}

			try
			{
				_context.Update(seller);
				_context.SaveChanges();
			}
			catch (DbUpdateConcurrencyException e)
			{
				throw new DbConcurrencyException(e.Message);
			}
		}
	}
}