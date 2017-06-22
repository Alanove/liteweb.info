using System;
using System.Data;
using lw.Base;
using lw.DataControls;
using lw.Utils;
using lw.WebTools;

namespace lw.Articles.Controls
{
	/// <summary>
	/// News Type Details Container (DataContainer)
	/// Automatically reads TypeId or NewsType from query string or page routing
	/// TypeId or NewsType can be overriden for static pages just put TypeId="" or NewsType="" inside the tag
	/// </summary>
	public class NewsTypeDataItem : DataProvider
	{
		#region variables
		NewsManager newsManager;

		int?_newsTypeId = null;
		string _newsType = null;
		DataRow _typeDetails = null;
		bool _overridePageTitle = true;

		bool _bound = false;

		CustomPage _page;

		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		public NewsTypeDataItem()
		{
			newsManager = new NewsManager();
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			_page = this.Page as CustomPage;

			if (TypeDetails == null)
			{
				//If the type is not found or it's status is set to no display
				//this.Page.Response.StatusCode = 404;
				//this.Page.Response.StatusDescription = "404 Not Found";

				return;
			}

			DataItem = TypeDetails;

			if (OverridePageTitle)
			{
				Config cfg = new Config();

				_page.Title = string.Format("{0} - {1}", 
					StringUtils.StripOutHtmlTags(TypeDetails["Name"].ToString()),
					cfg.GetKey("SiteName"));
			}

			base.DataBind();
		}

		#region Properties


		/// <summary>
		/// Returns or sets TypeId 
		/// Priority: Tag Value, Route, QueryString
		/// </summary>
		public int? TypeId
		{
			get 
			{
				if (_newsTypeId == null)
				{
					string obj = _page.GetQueryValue("NewsTypeId");
					if (!String.IsNullOrWhiteSpace(obj))
						_newsTypeId = Int32.Parse(obj);
				}
				return _newsTypeId;
			}
			set { _newsTypeId = value; }
		}

		/// <summary>
		/// Returns or sets NewsType (Name) 
		/// Priority: Tag Value, Route, QueryString
		/// </summary>
		public string NewsType
		{
			get
			{
				if (_newsType == null)
				{
					_newsType = _page.GetQueryValue("NewsType");
				}
				return _newsType; 
			}
			set { _newsType = value; }
		}

		/// <summary>
		/// Gets or Sets the TypeDetails Data Container associated with the tag
		/// this object is projected to DataItem
		/// </summary>
		public DataRow TypeDetails
		{
			get 
			{
				if (_typeDetails == null)
				{
					if (TypeId != null)
						_typeDetails = newsManager.GetNewsTypeView(TypeId.Value);
					else
						_typeDetails = newsManager.GetNewsTypeView(NewsType);
				}
				return _typeDetails;
			}
			set { _typeDetails = value; }
		}

		/// <summary>
		/// Flag to either override Page title from the title or to leave it untouched
		/// </summary>
		public bool OverridePageTitle
		{
			get { return _overridePageTitle; }
			set { _overridePageTitle = value; }
		}
		#endregion
	}
}
