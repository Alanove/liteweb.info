using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using lw.CTE;
using lw.Data;
using lw.Utils;
using lw.WebTools;
using System.Text.RegularExpressions;
using System.Text;


namespace lw.HashTags
{
	/// <summary>
	/// A class to manage the hash tags in the system inherited from LINQManager
	/// </summary>
	public class HashTagsManager : LINQManager
	{
		/// <summary>
		/// HashTagsManager constructor inherited from LINQManager
		/// cte.lib: the name of the Database library associated with this DLL
		/// </summary>
		public HashTagsManager()
			: base(cte.lib)
		{
		}

		/// <summary>
		/// Automatically parses the input string and insert the corresponding tag to db with the relation
		/// </summary>
		/// <param name="RelationId">The ID of thre related Item (might be PageId, AlbumId, ImageId) make sure that you change the type depending on your relation</param>
		/// <param name="TagType">The type of the Tag relation.</param>
		/// <param name="InputString">The input string to be parsed.</param>
		public void UpdateTags(int RelationId, HashTagTypes TagType, string InputString)
		{
			Regex r = new Regex(lw.CTE.RegularExpressions.HashTagMatcher, RegexOptions.IgnoreCase);

			StringBuilder sb = new StringBuilder();

			string sep = "";

			foreach (var tag in r.Matches(InputString))
			{
				sb.Append(sep);
				string temp = tag.ToString().Substring(1);
				if (!string.IsNullOrWhiteSpace(temp))
				{
					sb.Append(temp);
					sep = ",";
				}
			};

			HashTagsData.HashTags_Update(StringUtils.SQLEncode(sb.ToString()), RelationId, (short)TagType);
		}

		/// <summary>
		/// Inject tge tag links into the string, the tags inside the string will then be clickable.
		/// </summary>
		/// <param name="inputStr">The Input String</param>
		/// <param name="tagsPath">The absolute path for tags in the website, ex: /tags. The tag link will be added after</param>
		/// <returns>The modified string</returns>
		public static string InjectTagLinks(string inputStr, string tagsPath)
		{
			Regex r = new Regex(lw.CTE.RegularExpressions.HashTagMatcher, RegexOptions.IgnoreCase);

			return r.Replace(inputStr, delegate(Match m)
			{
				if (m.Length == 1)
					return m.Value;

				return string.Format("<a href=\"{0}/{1}\">{2}</a>", tagsPath, m.Value.Substring(1), m.Value);
			});
		}

		/// <summary>
		/// Inject tge tag links into the string, the tags inside the string will then be clickable.
		/// </summary>
		/// <param name="inputStr">The Input String</param>
		/// <returns>The modified string</returns>
		public static string InjectTagLinks(string inputStr)
		{
			string hashTagsAbsPath = lw.WebTools.WebUtils.GetFromWebConfig(lw.CTE.parameters.HashTagsAbsPath);

			return InjectTagLinks(inputStr, hashTagsAbsPath);
		}
		#region Variables

		/// <summary>
		/// HashTags DataContext, link to LINQ file
		/// </summary>
		public HashTagsDataContext HashTagsData
		{
			get
			{
				if (_dataContext == null)
					_dataContext = new HashTagsDataContext(Connection);
				return (HashTagsDataContext)_dataContext;
			}
		}

		#endregion
	}
}
