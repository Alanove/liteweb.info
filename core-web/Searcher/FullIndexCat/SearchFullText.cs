using System;
using System.Data;
using System.IO;
using System.Web;
using lw.CTE;
using lw.Data;
using lw.Utils;
using lw.WebTools;
using System.Text;

namespace lw.Searcher
{
	public class SearchFullText
	{
		string _lib = "";
		public SearchFullText()
		{
			this._lib = cte.lib;
		}

		public DataSet GetSearchResults(string SearchQuery)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("DECLARE @RC int DECLARE @KeyWord varchar(250) ");
			if(!string.IsNullOrWhiteSpace(SearchQuery)){
				sb.Append("select @KeyWord = '" + SearchQuery + "'");
			}
			sb.Append(" EXECUTE @RC = [dbo].[SearchSite] @KeyWord");
			var a = DBUtils.GetDataSet(SearchQuery, cte.lib);
			return a;
		}
	}
}
