using System;
using System.Collections.Specialized;
using System.Text;

using lw.CTE;
using lw.Utils;

namespace lw.WebTools
{
    public class ErrorContext
    {
		static string CollectionKey = Context.Errors;
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
		public static void Add(string key, Exception ex)
		{
			Add("exception-message-" + key, ex.Message);
			Add("exception-source-" + key, ex.Source);
			Add("exception-trace-" + key, ex.StackTrace);
		}
		public static void Add(Exception ex)
		{
			Add("exception-message", ex.Message);
			Add("exception-source", ex.Source);
			Add("exception-trace", ex.StackTrace);
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
			if(!StringUtils.IsNullOrWhiteSpace(str))
				return string.Format(format, sb.ToString());
			return "";
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
