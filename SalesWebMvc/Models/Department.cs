namespace SalesWebMVC.Models
{
    public class Department
    {
        public int Id { get; set; }
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