using System.Collections.Specialized;

namespace lw.Utils
{
	/// <summary>
	/// Defines utils that used from Collections.
	/// </summary>
	public class CollectionUtils
	{
		/// <summary>
		/// Transforms an HybridDictionary into a NameValueCollection
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static NameValueCollection HybridToNameValueCollection(HybridDictionary data)
		{
			NameValueCollection ret = new NameValueCollection();

			foreach (string key in data.Keys)
			{
				ret.Add(key, data[key].ToString());
			}

			return ret;
		}
	}
}
