using System;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;

using lw.Data;
using lw.WebTools;
using lw.CTE;
using System.Data;
using lw.Widgets.data;
using lw.Utils;
using lw.Widgets;

namespace lw.Widgets
{
	public class WidgetsManager : LINQManager
	{
		public WidgetsManager()
			: base(cte.lib)
		{
		}

		///Widgets related functionalities
		#region Widgets

		/// <summary>
		/// Overloading AddWidget: Adds the widgets to the table using only status, type and title
		/// </summary>
		/// <param name="Status">Status of the widget (1=>true, 0=>false) with DB type bit/tinyint</param>
		/// <param name="Type">Type of widget, we get it from the enum "DefaultWidgetsTypes" under Media project => enum.cs</param>
		/// <param name="Title">Title of the widget</param>
		/// <returns></returns>
		public data.Widget AddWidget(bool Status, int Type, string Title)
		{
			return AddWidget(null, Status, Type, Title, WebContext.Profile.UserId, WebContext.Profile.UserId, null);
		}

		/// <summary>
		/// Adds the widgets to the table having all needed parameters
		/// </summary>
		/// <param name="PageId">The Id of the Page to which the widget will be related</param>
		/// <param name="Status">Status of the widget (1=>true, 0=>false) with DB type bit/tinyint</param>
		/// <param name="Type">Type of widget, we get it from the enum "DefaultWidgetsTypes" under Media project => enum.cs</param>
		/// <param name="Title">Title of the widget</param>
		/// <param name="CreatedBy">CreatorId of the creator of the widget</param>
		/// <param name="ModifiedBy">CreatorId of the modifier of the widget</param>
		/// <returns></returns>
		public data.Widget AddWidget(int? PageId, bool? Status, int Type, string Title, int CreatedBy, int ModifiedBy, DateTime? ExpiratinDate)
		{
			byte status = Status.GetValueOrDefault() ? (byte)1 : (byte)0;
			int pageid = PageId.GetValueOrDefault();

			data.Widget widget = new data.Widget
			{
				PageId = pageid,
				Status = status,
				Type = Type,
				Title = Title,
				DateAdded = DateTime.Now,
				DateModified = DateTime.Now,
				ExpirationDate = ExpiratinDate,
				CreatedBy = CreatedBy,
				ModifiedBy = ModifiedBy
			};

			WidgetsData.Widgets.InsertOnSubmit(widget);
			WidgetsData.SubmitChanges();

			return widget;
		}

		/// <summary>
		/// Returns data record based on the widget id
		/// </summary>
		/// <param name="WidgetId">Id of the required widget</param>
		/// <returns>Single record if found, null if no data found</returns>
		public data.Widget GetWidget(int WidgetId)
		{
			var query = from w in WidgetsData.Widgets
						where w.Id == WidgetId
						select w;
			if (query.Count() > 0)
				return query.Single();
			return null;
		}

		public data.Widget GetWidget(int PageId, int Type)
		{
			var query = from w in WidgetsData.Widgets
						where w.PageId == PageId
						&& w.Type == Type
						select w;
			if (query.Count() > 0)
				return query.Single();
			return null;
		}

		/// <summary>
		/// Returns all widgets having the same type
		/// </summary>
		/// <param name="WidgetType">Int of the type of widget required, we get it from the enum "DefaultWidgetsTypes" under Media project => enum.cs</param>
		/// <returns>List of found records, null if no data found</returns>
		public IQueryable<data.Widget> GetWidgetsByType(int WidgetType)
		{
			var query = from w in WidgetsData.Widgets
						where w.Type == WidgetType
						select w;
			if (query.Count() > 0)
				return query;
			return null;
		}

		/// <summary>
		/// Returns all widgets having the same page
		/// </summary>
		/// <param name="WidgetType">Int of the type of widget required, we get it from the enum "DefaultWidgetsTypes" under Media project => enum.cs</param>
		/// <returns>List of found records, null if no data found</returns>
		public IQueryable<data.Widget> GetWidgetsByPage(int PageId)
		{
			var query = from w in WidgetsData.Widgets
						where w.PageId == PageId
						select w;
			if (query.Count() > 0)
				return query;
			return null;
		}

		/// <summary>
		/// Updates the widget based on the given params, params are nullable,
		/// if all were null no update shall occur.
		/// </summary>
		/// <param name="WidgetId">WidgetId, used to get the required widget</param>
		/// <param name="PageId">Id of the page to wich the widget is attached</param>
		/// <param name="Status">Status of the widget, 1/0 => true/false</param>
		/// <param name="Type">Type of the widget, we get it from the enum "DefaultWidgetsTypes" under Media project => enum.cs</param>
		/// <param name="Title">Title of the widget</param>
		/// <returns>true if success and false if widget not found</returns>
		public bool UpdateWidget(int WidgetId, int? PageId, bool? Status, int? Type, string Title)
		{
			data.Widget thisWidget = GetWidget(WidgetId);

			if (thisWidget == null)
				return false;

			byte status = Status.GetValueOrDefault() ? (byte)1 : (byte)0;
			int pageid = PageId.GetValueOrDefault();

			thisWidget.PageId = pageid;
			thisWidget.Status = status;
			thisWidget.Type = Type;
			thisWidget.Title = Title;
			thisWidget.ModifiedBy = WebContext.Profile.UserId;
			thisWidget.DateModified = DateTime.Now;

			WidgetsData.SubmitChanges();
			return true;
		}

		/// <summary>
		/// Deletes the record of the specified widget
		/// </summary>
		/// <param name="WidgetId">Id of the widget to be delted</param>
		/// <returns>Returns true if success and false on error</returns>
		public bool DeleteWidget(int WidgetId)
		{
			MediaManager mMgr = new MediaManager();
			data.Widget thisWidget = GetWidget(WidgetId);
			string sql = "Select MediaId From MediaWidgets Where WidgetId = {0}";

			if (thisWidget == null)
				return false;

			int widgetId = thisWidget.Id;

			var d = DBUtils.GetDataSet(string.Format(sql, widgetId), cte.lib).Tables[0].Rows;

			int[] MediaIds = new int[d.Count];
			for (int i = 0; i < d.Count; i++)
			{
				MediaIds[i] = Int32.Parse(d[i]["MediaId"].ToString());
			}
			mMgr.DeleteMediaFromWidget(MediaIds, widgetId);

			WidgetsData.Widgets.DeleteOnSubmit(thisWidget);
			WidgetsData.SubmitChanges();

			return true;
		}

		#endregion

		///General Variables
		#region Variables

		/// <summary>
		/// Widgets data context
		/// </summary>
		public data.WidgetsDataContext WidgetsData
		{
			get
			{
				if (_dataContext == null)
					_dataContext = new data.WidgetsDataContext(this.Connection);
				return (data.WidgetsDataContext)_dataContext;
			}
		}

		#endregion
	}
}