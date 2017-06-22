using System;
using System.Text.RegularExpressions;


namespace lw.Utils
{

	/// <summary>
	/// Defines different validation classes
	/// </summary>
	public class Validation
	{
		/// <summary>
		/// Checkes if the entry is empty
		/// </summary>
		/// <param name="fieldValue">Entry value</param>
		/// <returns>True if empty, false if not</returns>
		public static bool IsEmpty(string fieldValue)
		{
			if (fieldValue == "" || fieldValue == null)
				return true;
			else
				return false;
		}
		/// <summary>
		/// Checks if the entry text is an integer value
		/// </summary>
		/// <param name="fieldValue">Entry value</param>
		/// <returns>True if integer, false if not</returns>
		public static bool IsInteger(string fieldValue)
		{
			Regex re = new Regex(RegularExpressions.Integer, RegexOptions.IgnoreCase);
			return re.IsMatch(fieldValue);
		}

		/// <summary>
		/// Checks if the entry text is an decimal value
		/// </summary>
		/// <param name="fieldValue">Entry value</param>
		/// <returns>True if decimal, false if not</returns>
		public static bool IsDecimal(string fieldValue)
		{
			Regex re = new Regex(RegularExpressions.Decimal, RegexOptions.IgnoreCase);
			return re.IsMatch(fieldValue);
		}

		/// <summary>
		/// Checks if the entry text is a date value
		/// </summary>
		/// <param name="fieldValue">Entry value</param>
		/// <returns>True if date, false if not</returns>
		public static bool IsDate(string fieldValue)
		{
			try
			{
				DateTime.Parse(fieldValue);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		/// <summary>
		/// Checks if the entry value is between minValue and maxValue
		/// </summary>
		/// <paramref name="minValue"/>
		/// <paramref name="maxValue"/>
		/// <paramref name="fieldValue"/>
		/// <returns>True if in range, false if not</returns>
		public static bool InRange(double minValue, double maxValue, string fieldValue)
		{
			int length = fieldValue.Length;
			if (length > maxValue && length < minValue)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Checks if the entry text is an email
		/// </summary>
		/// <param name="email"></param>
		/// <returns>True if email, false if not</returns>
		public static bool IsEmail(string email)
		{
			Regex re = new Regex(RegularExpressions.Email);
			return re.IsMatch(email);
		}

		/// <summary>
		/// Checks if the entry text is an image
		/// </summary>
		/// <param name="image"></param>
		/// <returns>True if image, false if not</returns>
		public static bool IsImage(string image)
		{
			Regex re = new Regex(RegularExpressions.Image);
			return re.IsMatch(image);
		}

		/// <summary>
		/// Validates a strong password
		/// must contains one digit from 0-9
		/// must contains one lowercase characters
		/// must contains one uppercase characters
		/// must contains one special symbols in the list "@#$%"
		/// match anything with previous condition checking
        /// length at least 6 characters and maximum of 20	
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		public static bool IsStrongPassword(string password)
		{
			Regex re = new Regex(RegularExpressions.Password);
			return re.IsMatch(password);
		}
	}
}
