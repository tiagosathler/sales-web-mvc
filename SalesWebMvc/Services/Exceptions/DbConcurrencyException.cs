namespace SalesWebMVC.Services.Exceptions
{
	public class DbConcurrencyException : ApplicationException
	{
		public DbConcurrencyException() : base()
		{
		}

		public DbConcurrencyException(string? message) : base(message)
		{
		}

		public DbConcurrencyException(string? message, Exception? innerException) : base(message, innerException)
		{
		}
	}
}