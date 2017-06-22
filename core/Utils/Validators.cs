
namespace lw.Utils
{
	public class RegularExpressions
	{
		public const string Email = "^[^@%<>?.]([\\.]*[^@%<>?.])*@[a-z]([\\.\\-_]{0,1}[a-z0-9])*\\.[a-z]{2,}$";

		public const string Password = "((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,20})";

		public const string Integer = "\\-?\\d+";
		public const string Decimal = "\\-?\\d+(\\.\\d*)?";

		public const string Image = "[^\\\"<>/]+\\.(jpg|bmp|jpeg|gif|png|tif)$";
	}
}