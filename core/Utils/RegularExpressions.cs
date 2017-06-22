
namespace lw.Utils
{
	/// <summary>
	/// Contains regular expressions to be used in different parts of the project and validations
	/// </summary>
	public class RegularExpressions
	{

		/// <summary>
		/// Validates a correct email address
		/// </summary>
		public const string Email = "^[^@%<>?.]([\\.]*[^@%<>?.])*@[a-z]([\\.\\-_]{0,1}[a-z0-9])*\\.[a-z]{2,}$";

		/// <summary>
		/// Validates a strong password
		/// </summary>
		public const string Password = "((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,20})";

		/// <summary>
		/// Validates and integer
		/// </summary>
		public const string Integer = "\\-?\\d+";

		/// <summary>
		/// Validates a decimal entry
		/// </summary>
		public const string Decimal = "\\-?\\d+(\\.\\d*)?";

		/// <summary>
		/// Validates an image
		/// </summary>
		public const string Image = "[^\\\"<>/]+\\.(jpg|bmp|jpeg|gif|png|tif)$";

		/// <summary>
		/// Vaidates and HTML tag
		/// </summary>
		public const string HTMLTags = "<[^>]+>";
	}
}