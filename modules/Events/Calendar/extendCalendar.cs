using System.Data;

namespace lw.Events
{
	public class CalendarAdp : CalendarDsTableAdapters.CalendarTableAdapter
	{
		public DataTable GetEvents(string cond)
		{
			if (!string.IsNullOrEmpty(cond))
				cond = " where " + cond;
			base.CommandCollection[0].CommandText += cond;
			return base.GetData();
		}	
	}
	public class CalendarCategoriesAdp : CalendarDsTableAdapters.CalendarCategoriesTableAdapter
	{
		public CalendarDs.CalendarCategoriesDataTable GetCategories(string cond)
		{
			if (!string.IsNullOrEmpty(cond))
				cond = " where " + cond;
			base.CommandCollection[0].CommandText += cond;
			return base.GetData();
		}
	}
}
