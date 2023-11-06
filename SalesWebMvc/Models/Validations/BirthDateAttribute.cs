using System.ComponentModel.DataAnnotations;

namespace SalesWebMVC.Models.Validations
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public class BirthDateAttribute : ValidationAttribute
	{
		public DateTime BirthDateLimit { get; init; }

		public BirthDateAttribute(int legalAge)
		{
			BirthDateLimit = DateTime.Now.AddYears(-legalAge);
			ErrorMessage ??= $"The seller must be of legal age and born before {BirthDateLimit:dd/MM/yyyy}";
		}

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value is DateTime date && date <= BirthDateLimit)
			{
				return ValidationResult.Success;
			}
			return new ValidationResult(ErrorMessage);
		}
	}
}