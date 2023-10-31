using SalesWebMVC.Models.Enums;

namespace SalesWebMVC.Models
{
	public class SalesRecord

	{
		public int Id { get; set; } = default!;
		public DateTime Date { get; set; } = default!;
		public double Amount { get; set; } = default!;
		public SaleStatus Status { get; set; } = default!;
		public Seller Seller { get; set; } = default!;

		public SalesRecord()
		{ }

		public SalesRecord(int id, DateTime date, double amount, SaleStatus status, Seller seller)
		{
			Id = id;
			Date = date;
			Amount = amount;
			Status = status;
			Seller = seller;
		}

		public override bool Equals(object? obj)
		{
			return obj is SalesRecord record &&
				   Id == record.Id;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}
	}
}