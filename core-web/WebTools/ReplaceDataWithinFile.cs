using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.IO;

namespace lw.WebTools
{
	public class ReplaceDataWithinFile
	{

		public static string ReplaceData(string sourceFile, string destinationDirectory, string destinationFile, NameValueCollection data)
		{
			Config cfg = new Config();
			if (data == null)
				data = new NameValueCollection();

			string body = "";
			
			if (!Directory.Exists(destinationDirectory))
			{
				Directory.CreateDirectory(destinationDirectory);
			}

			string tempFile = Path.Combine(destinationDirectory, destinationFile);

			StreamWriter sr = new StreamWriter(tempFile);

			if (sourceFile != "")
			{
				System.IO.StreamReader s = new System.IO.StreamReader(sourceFile);
				body = s.ReadToEnd();
				if (data != null)
				{
					foreach (string str in data.Keys)
					{
						string p = "\\{{1}(?<id>\\w+)\\}{1}";
						System.Text.RegularExpressions.Regex r =
							new System.Text.RegularExpressions.Regex(p);

						DictionnaryEvaluator d = new DictionnaryEvaluator(data);

						body = r.Replace(body, new MatchEvaluator(d.LookUp));
					}
				}
				s.Close();
			}

			sr.Write(body);
			sr.Close();
			sr.Dispose();

			return tempFile;
		}
	}

	/// <summary>
	/// Evaluates a string against a dictionary
	/// Usage: DictionnaryEvaluator d = new DictionnaryEvaluator(data);
	///	string str = new MatchEvaluator(d.LookUp);
	/// </summary>
	/// 

	public class DictionnaryEvaluator
	{
		NameValueCollection _dic;
		public DictionnaryEvaluator(NameValueCollection dic)
		{
			this._dic = dic;
		}
		public string LookUp(Match m)
		{
			if (_dic[m.Groups["id"].Value] != null)
				return _dic[m.Value.Replace("{", "").Replace("}", "")].ToString();
			return "";
		}
	}
}
