using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace lw.Utils
{

	/// <summary>
	/// Evaluates a string against a dictionary
	/// Usage: DictionnaryEvaluator d = new DictionnaryEvaluator(data);
	///	string str = new MatchEvaluator(d.LookUp);
	/// </summary>
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
