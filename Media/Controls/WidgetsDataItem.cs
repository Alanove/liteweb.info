using System;
using lw.DataControls;
using lw.Utils;
using lw.WebTools;
using lw.Pages.Properties;
using System.Data;
using lw.Data;
using lw.Widgets;
using lw.Pages;

namespace lw.Widgets.Controls
{
	/// <summary>
	/// News Details Container (DataContainer)
	/// Automatically reads NewsId or UniqueName from query string or page routing
	/// NewsId or UniqueName can be overriden for static pages just put NewsId="" or UniqueName="" inside the tag
	/// </summary>
	public class WidgetsDataItem : DataProvider
	{
		#region variables
		WidgetsManager widgetsManager;

		int _widgetId = 0;
		data.Widget _widgetDetails = null;
		DefaultWidgetsTypes _widgetType = DefaultWidgetsTypes.NULL;
		int _pageId = 0;

		public bool Bound = false;

		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		public WidgetsDataItem()
		{
			widgetsManager = new WidgetsManager();
		}


		protected override void OnInit(EventArgs e)
		{

			base.OnInit(e);
		}

		public override void DataBind()
		{
			if (!this.Visible)
				return;

			if (WidgetDetails == null)
			{
				return;
			}

			if (Bound)
				return;
			Bound = true;

			DataItem = WidgetDetails;

			base.DataBind();
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			base.Render(writer);

		}

		protected override void OnUnload(EventArgs e)
		{
			base.OnUnload(e);
		}

		#region Properties

		/// <summary>
		/// Returns or sets WidgetId 
		/// Priority: Tag Value, Route, QueryString
		/// </summary>
		public int WidgetId
		{
			get
			{
				if (_widgetId == 0)
				{
					object obj = MyPage.GetQueryValue("WidgetId");
					if (obj != null && !String.IsNullOrEmpty(obj.ToString()))
					{
						string _temp = obj.ToString().Trim();
						try
						{
							_widgetId = Int32.Parse(_temp);
						}
						catch
						{
						}
					}
				}
				return _widgetId;
			}
			set { _widgetId = value; }
		}

		/// <summary>
		/// Gets or Sets the WidgetDetails Data Container associated with the tag
		/// this object is projected to DataItem
		/// </summary>
		public data.Widget WidgetDetails
		{
			get
			{
				if (_widgetDetails == null)
				{
					if (WidgetId != 0)
					{
						_widgetDetails = widgetsManager.GetWidget(WidgetId);
					}
					else
					{
						_widgetDetails = widgetsManager.GetWidget(PageId, (int)WidgetType);
					}
				}
				return _widgetDetails;
			}
			set
			{
				_widgetDetails = value;
				WidgetId = value.Id;
			}
		}

		public int PageId
		{
			get
			{
				if (_pageId == 0)
				{
					object obj = MyPage.GetQueryValue("PageId");
					if (obj != null && !String.IsNullOrEmpty(obj.ToString()))
					{
						string _temp = obj.ToString().Trim();
						try
						{
							_pageId = Int32.Parse(_temp);
						}
						catch
						{
						}
					}
					else
					{
						obj = MyPage.GetQueryValue("Id");
						if (obj != null && !String.IsNullOrEmpty(obj.ToString()))
						{
							string _temp = obj.ToString().Trim();
							try
							{
								_pageId = Int32.Parse(_temp);
							}
							catch
							{
							}
						}
						else
						{
							obj = MyPage.GetQueryValue("PageURL");
							if (obj != null && !String.IsNullOrWhiteSpace(obj.ToString()))
							{
								string _temp = obj.ToString().Trim();
								try
								{
									PagesManager pMgr = new PagesManager();
									var a = pMgr.GetPage(_temp);
									_pageId = a.PageId;
								}
								catch
								{
								}
							}
						}
					}
				}
				return _pageId;
			}
			set { _pageId = value; }
		}

		public DefaultWidgetsTypes WidgetType
		{
			get { return _widgetType; }
			set { _widgetType = value; }
		}
		#endregion
	}
}
