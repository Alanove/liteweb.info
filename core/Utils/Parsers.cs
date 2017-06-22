using System;

namespace lw.Utils
{
	/// <summary>
	/// A class to contain the different parsers.
	/// This is an old class, you can now use TryParse with almost all kind of .Net's built in classes
	/// </summary>
	public class Parsers
	{
		/// <summary>
		/// Parses a string into <see cref="Short"/>
		/// </summary>
		/// <param name="param">Entry parameter</param>
		/// <returns>The parsed string or null if fail</returns>
		public static short? Short(string param)
		{
			if (!String.IsNullOrWhiteSpace(param))
				return short.Parse(param);

			return null;
		}

		/// <summary>
		/// Parses a string into <see cref="Int"/>
		/// </summary>
		/// <param name="param">Entry parameter</param>
		/// <returns>The parsed string or null if fail</returns>
		public static int? Int(string param)
		{
			if (!String.IsNullOrWhiteSpace(param))
				return Int32.Parse(param);

			return null;
		}

		/// <summary>
		/// Parses a string into <see cref="Byte"/>
		/// </summary>
		/// <param name="param">Entry parameter</param>
		/// <returns>The parsed string or null if fail</returns>
		public static byte? Byte(string param)
		{
			if (!String.IsNullOrWhiteSpace(param))
				return byte.Parse(param);

			return null;
		}

		/// <summary>
		/// Parses a string into <see cref="DateTime"/>
		/// </summary>
		/// <param name="param">Entry parameter</param>
		/// <returns>The parsed string or null if fail</returns>
		public static DateTime? Date(string param)
		{
			if (!String.IsNullOrWhiteSpace(param))
				return DateTime.Parse(param);

			return null;
		}
	}
}
