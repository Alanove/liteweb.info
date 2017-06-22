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
using lw.Base;

namespace lw.Searcher
{
	public class SearcherManager : LINQManager
	{
		public SearcherManager():base(cte.lib)
		{
		}

		/// <summary>
		/// Returns the search results IDS seperated by comma
		/// </summary>
		/// <param name="SearchResults">Search Results Table get this from search site</param>
		/// <param name="TableType">Table Name, ex: News, PhotoAlbums, Members...</param>
		/// <returns>Returns the ids</returns>
		public static string GetIds(DataTable SearchResults, string TableName)
		{
			StringBuilder ret = new StringBuilder();
			string sep = "";

			foreach (DataRow row in SearchResults.Rows)
			{
				if (row["TableType"].ToString() != TableName)
					continue;

				ret.Append(sep);
				ret.Append(row["Id"].ToString());
				sep = ",";
			}

			return ret.ToString();
		}

		#region Searcher

		/// <summary>
		/// Starts searching
		/// </summary>
		/// <param name="@KeyWord">KeyWord to search for</param>
		/// <returns>list of answers</returns>
        public DataTable searchSite(string searchItem)
        {
			StringBuilder sql = new StringBuilder("[dbo].[SearchSite][(N'" + searchItem + "')]");
            
            DataSet ds = DBUtils.GetDataSet(sql.ToString(), cte.lib);

            return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0];
        }

		/// <summary>
		/// Serch and caches a specific query in the current page.
		/// </summary>
		/// <param name="@KeyWord">KeyWord to search for</param>
		/// <param name="Page">The related page</param>
		/// <returns>list of answers</returns>
		public DataTable searchSite(string searchItem, CustomPage Page)
		{
			if (Page.PageContext["Search_" + searchItem] != null)
				return Page.PageContext["Search_" + searchItem] as DataTable;

			DataTable ret = searchSite(searchItem);

			Page.PageContext["Search_" + searchItem] = ret;

			return ret;
		}
		#endregion
	}
}