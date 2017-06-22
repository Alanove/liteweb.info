using System;
using System.Data;

using lw.Data;
using lw.Utils;

namespace lw.Events
{
	public class CalendarMgr : DirectorBase
	{
		public CalendarMgr()
			: base(cte.lib)
		{
		}

		public DataRow GetEventDetails(int EventId)
		{
			DataView events = GetEvents(string.Format("Id={0}", EventId));
			return events.Count > 0 ? events[0].Row : null;
		}

		public DataView GetEvents(int Category)
		{
			string cond = string.Format("CategoryId&{0}={0}", Math.Pow(2, Category));
			return GetEvents(cond);
		}
		public DataView GetEvents(string cond)
		{
			string sql = "select * from CalendarView";
			if (!StringUtils.IsNullOrWhiteSpace(cond))
				sql += " where " + cond;
			return DBUtils.GetDataSet(sql, cte.lib).Tables[0].DefaultView;
		}

		public DataView GetCategories(string cond)
		{
			string sql = "Select * from CalendarCategories";
			return new DataView(DBUtils.GetDataSet(sql, cte.lib).Tables[0]);
		}

	}
}
