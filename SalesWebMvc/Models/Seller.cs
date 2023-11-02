using System.ComponentModel.DataAnnotations;

namespace SalesWebMVC.Models
{
	public class Seller
	{
		public int Id { get; set; } = default!;

		[Required(ErrorMessage = "{0} required")]
		[StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1}")]
		public string Name { get; set; } = null!;

		[Required(ErrorMessage = "{0} required")]
		[EmailAddress(ErrorMessage = "Enter a valid email address")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; } = null!;

		[Required(ErrorMessage = "{0} required")]
		[Display(Name = "Birth Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime BirthDate { get; set; } = default!;

		[Required(ErrorMessage = "{0} required")]
		[Range(100.0, 50000.0, ErrorMessage = "{0} must be from {1} to {2}")]
		[Display(Name = "Base Salary")]
		[DisplayFormat(DataFormatString = "{0:F2}")]
		public double BaseSalary { get; set; } = default!;

		public Department? Department { get; set; }

		public int DepartmentId { get; set; }

		public ICollection<SalesRecord> Sales { get; }

		public Seller()
		{
			Sales = new HashSet<SalesRecord>();
		}

		public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
			: this()
		{
			Id = id;
			Name = name;
			Email = email;
			BirthDate = birthDate;
			BaseSalary = baseSalary;
			Department = department;
		}

		public void AddSales(SalesRecord salesRecord)
		{
			Sales.Add(salesRecord);
		}

		public void RemoveSales(SalesRecord salesRecord)
		{
			Sales.Remove(salesRecord);
		}

		public double TotalSales(DateTime initial, DateTime final)

		{
			return Sales
				.Where(salesRecord => salesRecord.Date >= initial && salesRecord.Date <= final)
				.Sum(salesRecord => salesRecord.Amount);
		}

		public override bool Equals(object? obj)
		{
			return obj is Seller seller &&
				   Id == seller.Id;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}
	}
}