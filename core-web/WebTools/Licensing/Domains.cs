using System;
using System.Collections.Generic;


namespace lw.CTE
{
	public class Lisensing
	{
		public static List<String> Domains = new List<string>();

		public static bool HasLicense
		{
			get
			{
				string domain = "";
				if (Domains.Count > 0)
					return !String.IsNullOrEmpty(Domains.Find(delegate(string s) { return s == domain; }));
				return true;
			}
		}
	}

	class LicenseKeys
	{
		public string OriginalKey;
		public string Domain;
		public DateTime Expires;

		public LicenseKeys(string key)
		{

		}
	}
}
