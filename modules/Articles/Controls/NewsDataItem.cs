using System;
using lw.Articles.LINQ;
using lw.DataControls;
using lw.Utils;
using lw.WebTools;

namespace lw.Articles.Controls
{
	/// <summary>
	/// News Details Container (DataContainer)
	/// Automatically reads NewsId or UniqueName from query string or page routing
	/// NewsId or UniqueName can be overriden for static pages just put NewsId="" or UniqueName="" inside the tag
	/// </summary>
	public class NewsDataItem : DataProvider
	{
		#region variables
		LINQ.NewsManager newsManager;

		int?_newsId = null;
		string _newsTitle = null;
		NewsDetailView _newsDetails = null;
		bool _overridePageTitle = true;

		public bool Bound = false;

		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		public NewsDataItem()
		{
			newsManager = new LINQ.NewsManager();
		}

		public override void DataBind()
		{
			if (!this.Visible)
				return;
			
			if (NewsDetails == null)
			{
				//If the article is not found or it's status is set to no display
				//this.Page.Response.StatusCode = 404;
				//this.Page.Response.StatusDescription = "404 Not Found";

				return;
			}

			if (Bound)
				return;
			Bound = true;

			

			DataItem = NewsDetails;

			if(MyPage.Editable)
				MyPage.RegisterLoadScript("NewsId", "document.__MyPageId = " + NewsDetails.NewsId.ToString() + ";");
			
			base.DataBind();
			if (OverridePageTitle)
			{
				Config cfg = new Config();

				if(MyPage != null)
				{
					if (!String.IsNullOrEmpty(NewsDetails.Title))
						MyPage.CustomTitle = string.Format("{0} - {1}",
							StringUtils.StripOutHtmlTags(NewsDetails.Title),
							cfg.GetKey("SiteName"));

					if (!String.IsNullOrEmpty(NewsDetails.Header))
						MyPage.Description = string.Format("{0}",
							StringUtils.StripOutHtmlTags(NewsDetails.Header));
			
				}
			}

			
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			base.Render(writer);
			
		}

		protected override void OnUnload(EventArgs e)
		{
			this.Page.Unload += new EventHandler(Page_Unload);
			base.OnUnload(e);
		}

		void Page_Unload(object sender, EventArgs e)
		{
			if (NewsId != null)
			{
				NewsManager nMgr = new NewsManager();
				nMgr.IncrementArticleViews(NewsId.Value);
			}
		}

		#region Properties

		/// <summary>
		/// Returns or sets NewsId 
		/// Priority: Tag Value, Route, QueryString
		/// </summary>
		public int? NewsId
		{
			get 
			{
				if (_newsId == null)
				{
					object obj = null;
					if (MyPage.RouteData != null)
						obj = Page.RouteData.Values["NewsId"];
					
					if (obj != null)
					{
						string _temp = obj.ToString().Trim();
						try
						{
							_newsId = Int32.Parse(_temp);
						}
						catch
						{
						}
					}
					if (_newsId == null)
					{
						obj = MyPage.GetQueryValue("NewsId");
						if (obj != null && !String.IsNullOrEmpty(obj.ToString()))
						{
							string _temp = obj.ToString().Trim();
							try
							{
								_newsId = Int32.Parse(_temp);
							}
							catch
							{
							}
						}
					}
				}
				return _newsId;
			}
			set { _newsId = value; }
		}

		/// <summary>
		/// Returns or sets NewsTitle 
		/// Priority: Tag Value, Route, QueryString
		/// </summary>
		public string NewsTitle
		{
			get
			{
				if (_newsTitle == null)
				{
                    _newsTitle = MyPage.GetQueryValue("NewsTitle");
                    if(String.IsNullOrEmpty(_newsTitle))
                    {
                        _newsTitle = MyPage.GetQueryValue("Title");
                    }
                    if (!String.IsNullOrEmpty(_newsTitle))
                    {
                        //_newsTitle = _newsTitle.Replace("-", " ");
                    }
				}
				return _newsTitle; 
			}
			set { _newsTitle = value; }
		}

		/// <summary>
		/// Gets or Sets the NewsDetails Data Container associated with the tag
		/// this object is projected to DataItem
		/// </summary>
		public NewsDetailView NewsDetails
		{
			get 
			{
				if (_newsDetails == null)
				{
					if (NewsId != null)
						_newsDetails = newsManager.GetNewsDetails(NewsId.Value);
					else if (!String.IsNullOrWhiteSpace(NewsTitle))
						_newsDetails = newsManager.GetNewsDetails(NewsTitle);
					if (_newsDetails != null)
					{
						_newsId = _newsDetails.NewsId;
						_newsTitle = _newsDetails.Title;

						if (_newsDetails.Status == (byte)NewsStatus.NoDisplay && !MyPage.Editable && WebContext.Request["preview"] != "true")
							_newsDetails = null;
					} //Page is linked to NewsType
					else
					{
						string newsType = MyPage.GetQueryValue("NewsType");

						NewsTypesManager ntMgr = new NewsTypesManager();


					}
				}
				return _newsDetails;
			}
			set { 
				_newsDetails = value;
				NewsId = value.NewsId;
				NewsTitle = value.Title;
			}
		}

		/// <summary>
		/// Flag to either override Page title from the news title or to leave it untouched
		/// </summary>
		public bool OverridePageTitle
		{
			get { return _overridePageTitle; }
			set { _overridePageTitle = value; }
		}
		#endregion
	}
}
