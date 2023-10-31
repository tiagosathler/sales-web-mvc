namespace SalesWebMVC.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public double BaseSalary { get; set; }
        public Department Department { get; set; } = default!;
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