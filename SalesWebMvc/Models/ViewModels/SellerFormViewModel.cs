namespace SalesWebMVC.Models.ViewModels
{
	public class SellerFormViewModel
	{
		public Seller Seller { get; set; } = default!;
		public ICollection<Department> Departments { get; }

		public SellerFormViewModel(ICollection<Department> departments)
		{
			Departments = departments;
		}

		public SellerFormViewModel(ICollection<Department> departments, Seller seller)
			: this(departments)
		{
			Seller = seller;
		}
	}
}