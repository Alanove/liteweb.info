using System;
using System.Data;
using System.Web.UI;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Widgets;
using lw.Utils;
using lw.WebTools;
using lw.CTE;
using System.Text;
using lw.Pages;
using lw.Base;

namespace lw.Widgets.Controls
{
	public class WidgetsDataSource : CustomDataSource
	{
		bool _bound = false;
		string _condition = "";
		string _top = "";
		bool _status = true;
		string _orderBy = "DateAdded DESC";
		DefaultWidgetsTypes _type = DefaultWidgetsTypes.NULL;
		int? _pageId = null;
		CustomPage _page;

		MediaManager mMgr;

		public WidgetsDataSource()
		{
			this.DataLibrary = cte.lib;
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			_page = this.Page as CustomPage;

			BuildQuery();

			base.DataBind();
		}

		public void BuildQuery()
		{
			if(Type != DefaultWidgetsTypes.NULL)
				this.SelectCommand = string.Format("Select * From Widgets Where Type = {0} and PageId = {2} Order By {1}", (int)Type, OrderBy, PageId);
			else
			this.SelectCommand = string.Format("Select * From Widgets Where PageId = {1} Order By {0}", OrderBy, PageId);
		}

		public string Condition
		{
			get { return _condition; }
			set { _condition = value; }
		}

		public string Top
		{
			get { return _top; }
			set { _top = value; }
		}
		
		public bool Status
		{
			get { return _status; }
			set { _status = value; }
		}

		public string OrderBy
		{
			get { return _orderBy; }
			set { _orderBy = value; }
		}

		public DefaultWidgetsTypes Type
		{
			get
			{ return _type; }
			set
			{ _type = value; }
		}

		public int? PageId
		{
			get {
				return _pageId = (int)ControlUtils.GetBoundedDataField(this.Parent.NamingContainer, "PageId");
			}
			set { _pageId = value; }
		}
	}
}