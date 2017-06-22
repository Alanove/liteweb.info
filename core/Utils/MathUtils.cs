
namespace lw.Utils
{
	/// <summary>
	/// Utils that are related to Math in general
	/// </summary>
	public class MathUtils
	{
		/// <summary>
		/// Returns the percentage of value in total
		/// </summary>
		/// <param name="value">The exact value</param>
		/// <param name="total">The total value</param>
		/// <returns>The percentage value</returns>
		public static double Percentage(object value, object total)
		{
			if (double.Parse(total.ToString()) == 0)
				return 0;
			return double.Parse(value.ToString()) * 100 / double.Parse(total.ToString());
		}
	}
}
