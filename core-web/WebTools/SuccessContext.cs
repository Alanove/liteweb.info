using System;
using System.Collections.Specialized;
using System.Text;
using lw.CTE;

namespace lw.WebTools
{
    public class SuccessContext
    {
		static string CollectionKey = Context.Success;
        public static NameValueCollection Collection
        {
            get
            {
				NameValueCollection col;
				if (WebContext.ExecutionContext[CollectionKey] != null)
					col = (NameValueCollection)WebContext.ExecutionContext[CollectionKey];
				else
				{
					col = new NameValueCollection();
					WebContext.ExecutionContext[CollectionKey] = col;
				}
				return col;
			}
		}
		public static void Add(string key, string value)
		{
			Collection.Add(key, value);
		}
		public static string Get(string key)
		{
			return Collection[key];
		}
		public static string GetAll(string format, string seperator)
		{
			return GetAll(format, seperator, "");
		}
		public static string GetAll(string format, string seperator, string keyFilter)
		{
			StringBuilder sb = new StringBuilder();
			string sep = "";
			foreach (string key in Collection)
			{
				sb.Append(sep);
				if (keyFilter != "")
				{
					if (key.IndexOf(keyFilter) != 0)
						continue;
				}
				sb.Append(Collection[key]);
				sep = seperator;
			}
			string str = sb.ToString();
			if (String.IsNullOrEmpty(str))
				return str;
			return string.Format(format, str);
		}
		public static void Clear()
		{
			Collection.Clear();
		}
		public static int Count
		{
			get
			{
				return Collection.Count;
			}
		}
	}
}
