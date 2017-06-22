using System;
using System.Collections;
using System.Data;
using System.Text;
using lw.Data;
using lw.Networking;
using lw.WebTools;

namespace lw.Articles
{
	public class NewsNetworks : NetworkRelations
	{
		string _lib = "";
		public NewsNetworks()
		{
			this._lib = cte.lib;
		}
		public DataTable GetRelatedNetworks(int NewsId, string Condition)
		{
			return GetRelatedNetworks(cte.NetworkRelationTable, cte.NetworkRelateToField, NewsId, Condition);
		}
		public DataTable GetRelatedNetworks(int NewsId)
		{
			return GetRelatedNetworks(cte.NetworkRelationTable, cte.NetworkRelateToField, NewsId);
		}
		public DataTable GetRelatedNews(int NetworkId)
		{
			return GetRelatedItems(cte.NetworkRelationTable, NetworkId);
		}
		public void UpdateNewsRelations(int NewsId, ArrayList NetworkIds)
		{
			UpdateItemRelations(cte.NetworkRelationTable, cte.NetworkRelateToField, NewsId, NetworkIds);
		}
		public string GetRelationQueryByMember(int MemberId)
		{
			return GetRelationQueryByMember(cte.NetworkRelationTable, cte.NetworkRelateToField, MemberId);
		}
		public DataTable GetNewsByNetwork(int MemberId, DateTime? Date, string condition)
		{
			if (String.Compare(WebContext.Profile.dbUserName, Config.GetFromWebConfig("Admin"), true) == 0)
			{
				NewsManager nMgr = new NewsManager();
				return nMgr.GetNewsView(condition).Table;
			}
			else
			{
				StringBuilder cond = new StringBuilder();

				cond.Append(" and " + GetRelationQueryByMember(MemberId));
				if (Date != null)
					cond.Append(string.Format(" and DateAdded>='{0}'", Date));


				string sql = string.Format("select * from NewsView where 1=1" + (condition != null ? " and " + condition : "") + " {0}",
					cond.ToString());

				DataSet ds = DBUtils.GetDataSet(sql, cte.lib);
				DataTable newsTable = ds.Tables[0];


				System.Collections.Specialized.NameValueCollection statustext = new System.Collections.Specialized.NameValueCollection();
				statustext.Add("0", "No Display");
				statustext.Add("2", "Archive");
				statustext.Add("3", "Main Page");

				System.Collections.Specialized.NameValueCollection languages = new System.Collections.Specialized.NameValueCollection();
				languages.Add("1", "Description");
				languages.Add("2", "Events");

				newsTable.Columns.Add("StatusText", typeof(string));
				newsTable.Columns.Add("LanguageText", typeof(string));

				foreach (DataRow n in newsTable.Rows)
				{
					n["StatusText"] = statustext[n["Status"].ToString()];
					n["LanguageText"] = languages[n["NewsLanguage"].ToString()];
				}

				newsTable.AcceptChanges();

				return newsTable.DefaultView.Table;
			}
		}
		public void UpdateNewsPreferedNetworks(int NewsId, int networkId, bool prefered)
		{
			string sqlFalse = string.Format("update  " + cte.NetworkRelationTable + " set prefered='{0}' where newsId={1}",
				false, NewsId);
			DBUtils.ExecuteQuery(sqlFalse, cte.lib);

			string sql = string.Format("update {3} set prefered='{0}' where NewsId={1} and networkId={2}",
				   prefered, NewsId, networkId, cte.NetworkRelationTable);
			DBUtils.ExecuteQuery(sql, cte.lib);
		}
		public string GetNewsPreferedNetwork(int NewsId)
		{
			string sql = string.Format("select NetworkId from {0} where NewsId = {1} and Prefered = {2}", cte.NetworkRelationTable, NewsId, 1);
			var dbMemberId = DBUtils.GetDataSet(sql, lw.Articles.cte.lib);

			if (dbMemberId.Tables[0].Rows.Count != 0)
			{
				return dbMemberId.Tables[0].Rows[0]["NetworkId"].ToString();
			}
			else return null;
		}
	}
}
