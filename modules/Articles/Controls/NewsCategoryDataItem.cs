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
	public class NewsCategoryDataItem : DataProvider
	{
		#region variables
		LINQ.NewsTypesManager newsManager;

		int?_categoryId = null;
		string _categoryName = null;
		NewsType _categoryDetails = null;
		bool _overridePageTitle = true;

		public bool Bound = false;

		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		public NewsCategoryDataItem()
		{
			newsManager = new LINQ.NewsTypesManager();
		}

		public override void DataBind()
		{
			if (!this.Visible)
				return;

			if (CategoryDetails == null)
			{
				//If the article is not found or it's status is set to no display
				//this.Page.Response.StatusCode = 404;
				//this.Page.Response.StatusDescription = "404 Not Found";

				return;
			}

			if (Bound)
				return;
			Bound = true;

			DataItem = CategoryDetails;

			if (OverridePageTitle)
			{
				Config cfg = new Config();

				lw.Base.CustomPage page = this.Page as lw.Base.CustomPage;
				if (page != null)
				{

					if (!String.IsNullOrEmpty(CategoryDetails.Name))
						page.CustomTitle = string.Format("{0} - {1}",
							StringUtils.StripOutHtmlTags(CategoryDetails.Name),
							cfg.GetKey("SiteName"));
				}

			}
			base.DataBind();
		}

		protected override void OnUnload(EventArgs e)
		{
			this.Page.Unload += new EventHandler(Page_Unload);
			base.OnUnload(e);
		}

		void Page_Unload(object sender, EventArgs e)
		{
		}

		#region Properties


		/// <summary>
		/// Returns or sets NewsId 
		/// Priority: Tag Value, Route, QueryString
		/// </summary>
		public int? CategoryId
		{
			get 
			{
				if (_categoryId == null)
				{
					string obj = (this.Page as lw.Base.CustomPage).GetQueryValue("NewsCategoryId");
					
					
					if (!string.IsNullOrWhiteSpace(obj))
					{
						string _temp = obj.ToString().Trim();
						try
						{
							_categoryId = Int32.Parse(_temp);
						}
						catch
						{
						}
					}
				}
				return _categoryId;
			}
			set { _categoryId = value; }
		}

		/// <summary>
		/// Returns or sets NewsTitle 
		/// Priority: Tag Value, Route, QueryString
		/// </summary>
		public string CategoryName
		{
			get
			{
				if (_categoryName == null)
				{
					_categoryName = (this.Page as lw.Base.CustomPage).GetQueryValue("NewsType");

					
				}
				return _categoryName; 
			}
			set { _categoryName = value; }
		}

		/// <summary>
		/// Gets or Sets the NewsDetails Data Container associated with the tag
		/// this object is projected to DataItem
		/// </summary>
		public NewsType CategoryDetails
		{
			get 
			{
				if (_categoryDetails == null)
				{
					if (CategoryId != null)
						_categoryDetails = newsManager.GetType(CategoryId.Value);

					else if (!String.IsNullOrWhiteSpace(CategoryName))
						_categoryDetails = newsManager.GetType(CategoryName);

					

					if (_categoryDetails != null)
					{
						_categoryId = _categoryDetails.TypeId;
						_categoryName = _categoryDetails.UniqueName;

						if (_categoryDetails.Status == (byte)NewsStatus.NoDisplay)
							_categoryDetails = null;
					}
					
				}
				return _categoryDetails;
			}
			set 
			{
				CategoryId = value.TypeId;
				_categoryDetails = value;
				CategoryName = value.Name;
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
