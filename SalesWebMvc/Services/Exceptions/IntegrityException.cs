namespace SalesWebMVC.Services.Exceptions
{
	public class IntegrityException : ApplicationException
	{
		public IntegrityException() : base()
		{
		}

		public IntegrityException(string? message) : base(message)
		{
		}

		public IntegrityException(string? message, Exception? innerException) : base(message, innerException)
		{
		}
	}
}