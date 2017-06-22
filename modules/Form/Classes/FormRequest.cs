using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace lw.Forms.Classes
{
	
	[Serializable]
	/// <summary>
	/// Represents a form request object
	/// </summary>
	public class FormRequest
	{
		Dictionary<string, string> _data = new Dictionary<string, string>();
		Dictionary<string, string> _files = new Dictionary<string, string>();

		/// <summary>
		/// Returns the data in this request
		/// </summary>
		public Dictionary<string, string> Data
		{
			get
			{
				return _data;
			}
			set
			{
				_data = value;
			}
		}


		/// <summary>
		/// Parses a NameValueCollection into the internal Dictionary
		/// </summary>
		/// <param name="collection">The input collection</param>
		public void Parse(NameValueCollection collection)
		{
			foreach (string key in collection.Keys)
			{
				_data.Add(key, collection[key]);
			}
		}

		/// <summary>
		/// Adds data to the internal collection
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Add(string key, string value)
		{
			_data.Add(key, value);
		}


		/// <summary>
		/// Returns the value of the given key
		/// </summary>
		/// <param name="key">The key</param>
		/// <returns>The value of the key</returns>
		public string Get(string key)
		{
			string ret = null;
			if (_data.Keys.Contains(key))
				ret = _data[key];
			return ret;
		}

		/// <summary>
		/// Returns a mirror of the data in a NameValueCollection object
		/// </summary>
		public NameValueCollection EmailValues()
		{
			var ret = new NameValueCollection();
			foreach (string key in _data.Keys)
			{
				ret.Add(key, _data[key]);
			}
			return ret;
		}
	}
}
