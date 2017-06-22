using System;
using System.Collections;
using System.Data;
using System.Text;
using lw.Data;
using lw.Networking;
using lw.WebTools;
using lw.Events;

namespace lw.Calendar
{
	public class CalendarNetwork : NetworkRelations
	{
		string _lib = "";
		public CalendarNetwork()
		{
			this._lib = lw.Events.cte.lib;
		}
		public DataTable GetRelatedNetworks(int CalendarId, string Condition)
		{
			return GetRelatedNetworks(lw.Events.cte.NetworkRelationTable, lw.Events.cte.NetworkRelateToField, CalendarId, Condition);
		}
		public DataTable GetRelatedNetworks(int CalendarId)
		{
			return GetRelatedNetworks(lw.Events.cte.NetworkRelationTable, lw.Events.cte.NetworkRelateToField, CalendarId);
		}
		public DataTable GetRelatedCalendars(int NetworkId)
		{
			return GetRelatedItems(lw.Events.cte.NetworkRelationTable, NetworkId);
		}
		public void UpdateCalendarRelations(int CalendarId, ArrayList NetworkIds)
		{
			UpdateItemRelations(lw.Events.cte.NetworkRelationTable, lw.Events.cte.NetworkRelateToField, CalendarId, NetworkIds);
		}
		public string GetRelationQueryByMember(int MemberId)
		{
			return GetRelationQueryByMember(lw.Events.cte.NetworkRelationTable, lw.Events.cte.NetworkRelateToField, MemberId);
		}

		public DataTable GetCalendarsByNetwork(int MemberId, DateTime? Date, string condition)
		{
			if (String.Compare(WebContext.Profile.dbUserName, Config.GetFromWebConfig("Admin"), true) == 0)
			{
				CalendarManager paMgr = new CalendarManager();
                return paMgr.GetDataEvents(condition).Table;
			}
			else
			{
				StringBuilder cond = new StringBuilder();

				cond.Append(" and " + GetRelationQueryByMember(MemberId));
				if (Date != null)
					cond.Append(string.Format(" and DateAdded>='{0}'", Date));


				string sql = string.Format("select * from CalendarsFullView where 1=1" + (condition != null ? condition : "") + " {0}",
					cond.ToString());

				return DBUtils.GetDataSet(sql, _lib).Tables[0];
			}
		}
		public void UpdateCalendarPreferedNetworks(int CalendarId, int networkId, bool prefered)
		{
			string sqlFalse = string.Format("update  " + lw.Events.cte.NetworkRelationTable + " set prefered='{0}' where Id={1}",
				false, CalendarId);
			DBUtils.ExecuteQuery(sqlFalse, lw.Events.cte.lib);

			string sql = string.Format("update {3} set prefered='{0}' where Id={1} and networkId={2}",
				   prefered, CalendarId, networkId, lw.Events.cte.NetworkRelationTable);
			DBUtils.ExecuteQuery(sql, lw.Events.cte.lib);
		}
		public string GetCalendarPreferedNetwork(int CalendarId)
		{
			string sql = string.Format("select NetworkId from {0} where Id = {1} and Prefered = {2}", lw.Events.cte.NetworkRelationTable, CalendarId, 1);
			var dbMemberId = DBUtils.GetDataSet(sql, lw.Events.cte.lib);

			if (dbMemberId.Tables[0].Rows.Count != 0)
			{
				return dbMemberId.Tables[0].Rows[0]["NetworkId"].ToString();
			}
			else return null;
		}
	}
}
