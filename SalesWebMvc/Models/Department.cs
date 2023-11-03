using System.ComponentModel.DataAnnotations;

namespace SalesWebMVC.Models
{
    public class Department
    {
        public int Id { get; set; } = default!;

        [Required(ErrorMessage = "{0} required")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1}")]
        public string Name { get; set; } = null!;

        public ICollection<Seller> Sellers { get; }

        public Department()
        {
            Sellers = new HashSet<Seller>();
        }

        public Department(int id, string name)
            : this()
        {
            Id = id;
            Name = name;
        }

        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sellers
                .Sum(seller => seller.TotalSales(initial, final));
        }

        public override bool Equals(object? obj)
        {
            return obj is Department department &&
                   Id == department.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}