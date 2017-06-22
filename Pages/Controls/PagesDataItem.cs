using System;
using lw.DataControls;
using lw.Utils;
using lw.WebTools;
using lw.Pages.Properties;
using System.Data;
using lw.Data;
using System.Web;

namespace lw.Pages.Controls
{
    /// <summary>
    /// News Details Container (DataContainer)
    /// Automatically reads NewsId or UniqueName from query string or page routing
    /// NewsId or UniqueName can be overriden for static pages just put NewsId="" or UniqueName="" inside the tag
    /// </summary>
    public class PagesDataItem : DataProvider
    {
        #region variables
        PagesManager pagesManager;

        int? _pageId = null;
        string _pageURL = null;
        data.Pages_View _pageDetails = null;
        string _pageTitle = null;
        bool _overridePageTitle = true;

        public bool Bound = false;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public PagesDataItem()
        {
            pagesManager = new PagesManager();
        }


        protected override void OnInit(EventArgs e)
        {

            base.OnInit(e);
        }

        public override string MasterPageFile
        {
            get
            {
                if (PageDetails != null && !String.IsNullOrWhiteSpace(PageDetails.TemplateFileName))
                {
                    base.MasterPageFile = string.Format("{0}/{1}", lw.CTE.Settings.TemplatesFolder, PageDetails.TemplateFileName);
                }
                return base.MasterPageFile;
            }
            set
            {
                base.MasterPageFile = value;
            }
        }

        public override void DataBind()
        {
            if (!this.Visible)
                return;

            if (PageDetails == null)
            {
                //If the article is not found or it's status is set to no display
				//this.Page.Response.TrySkipIisCustomErrors = true;
                this.Page.Response.StatusCode = 404;
                this.Page.Response.StatusDescription = "404 Not Found";
				this.Page.Response.End();
				return;

				//throw new HttpException(404, "Page not found");

               // return;
            }

            if (Bound)
                return;
            Bound = true;




            DataItem = PageDetails;

			if (MyPage.Editable)
			{
				MyPage.RegisterLoadScript("PageId", "document.__MyPageId = " + PageDetails.PageId.ToString() + ";");
				MyPage.RegisterLoadScript("PageRoles", "document.__MyPageEditingRoles = " + (PageDetails.EditingRoles != null ? PageDetails.EditingRoles.ToString() : "0") + ";");
			}

            base.DataBind();
            if (OverridePageTitle)
            {
                Config cfg = new Config();

                if (MyPage != null)
                {
                    if (!String.IsNullOrEmpty(PageDetails.Title))
                        MyPage.CustomTitle = string.Format("{0} - {1}",
                            StringUtils.StripOutHtmlTags(PageDetails.Title),
                            cfg.GetKey("SiteName"));

                    if (!String.IsNullOrEmpty(PageDetails.SmallDescription))
                        MyPage.Description = string.Format("{0}",
                            StringUtils.StripOutHtmlTags(PageDetails.SmallDescription));

                    if (!String.IsNullOrEmpty(PageDetails.Image))
                        MyPage.Image = string.Format("{0}://{1}{2}/{3}/Page_{4}/{5}", WebContext.Protocol, WebContext.ServerName, WebContext.Root, lw.CTE.Folders.PagesFolder, PageDetails.PageId, PageDetails.Image.Replace(".", "-l."));

                    if (!String.IsNullOrEmpty(PageDetails.FullURL))
                        MyPage.Url = WebContext.Request.Url.AbsoluteUri;

                    if (PageDetails.IsSecure != null)
                        MyPage.isSecure = (bool)PageDetails.IsSecure;

                    if (!String.IsNullOrEmpty(PageDetails.Keywords))
                        MyPage.Keywords = string.Format("{0}",
                            StringUtils.StripOutHtmlTags(PageDetails.Keywords));
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

		bool affectPageViews = true;
		/// <summary>
		/// Tells the tag if it should affect the page views or not
		/// This should be used when having to PagesDataItem Tags on the same page
		/// </summary>
		public bool AffectPageViews
		{
			get { return affectPageViews; }
			set { affectPageViews = value; }
		}

		/// <summary>
		/// Runs when the page unloads
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        void Page_Unload(object sender, EventArgs e)
        {
			if (affectPageViews) 
            {
                pagesManager.IncrementPageViews(PageId.Value);
				PageId = null;
            }
        }

        #region Properties

        /// <summary>
        /// Returns or sets PageId 
        /// Priority: Tag Value, Route, QueryString
        /// </summary>
        public int? PageId
        {
            get
            {
                if (_pageId == null)
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
                    }
                }
                return _pageId;
            }
            set { _pageId = value; }
        }

        /// <summary>
        /// Returns or sets PageURL
        /// Priority: Tag Value, Route, QueryString
        /// </summary>
        public string PageURL
        {
            get
            {
                if (_pageURL == null)
                {
                    _pageURL = MyPage.GetQueryValue("PageURL");
                }
                return _pageURL;
            }
            set { _pageURL = value; }
        }

        /// <summary>
        /// Returns or sets Page Title
        /// Priority: Tag Value, Route, QueryString
        /// </summary>
        public string PageTitle
        {
            get
            {
                if (_pageTitle == null)
                {
                    _pageTitle = MyPage.GetQueryValue("PageTitle");
                }
                return _pageTitle;
            }
            set { _pageTitle = value; }
        }

        /// <summary>
        /// Gets or Sets the NewsDetails Data Container associated with the tag
        /// this object is projected to DataItem
        /// </summary>
        public data.Pages_View PageDetails
        {
            get
            {
                if (_pageDetails == null)
                {
					if (PageId != null)
					{
						_pageDetails = pagesManager.GetPageView(PageId.Value);
					}
					else if (!String.IsNullOrWhiteSpace(PageURL))
					{
						if (MyPage.PageContext["Page_" + PageURL] != null)
							_pageDetails = MyPage.PageContext["Page_" + PageURL] as data.Pages_View;
						else
							_pageDetails = pagesManager.GetPageView(PageURL);
					}
					else if (!String.IsNullOrWhiteSpace(PageTitle))
						_pageDetails = pagesManager.GetPageView(PageTitle);

                    if (_pageDetails != null)
                    {
                        _pageId = _pageDetails.PageId;
                        _pageURL = _pageDetails.URL;

                        if (_pageDetails.Status == (byte)PageStatus.Hidden && !MyPage.Editable && WebContext.Request["preview"] != "true")
                            _pageDetails = null;

                        //if (_pageDetails.Status == (byte)PageStatus.Deleted)
                        //	_pageDetails = null;

                        if (_pageDetails == null)
                        {
                            this.Page.Response.StatusCode = 404;
							this.Page.Response.StatusDescription = "404 Not Found";
							this.Page.Response.End();
                        }

						MyPage.PageContext["Page_" + PageURL] = _pageDetails;

                    } //Page is linked to NewsType
                }
                return _pageDetails;
            }
            set
            {
                _pageDetails = value;
                PageId = value.PageId;
                PageURL = value.URL;
            }
        }

        DataTable _pageProperties = null;
        public DataTable PageProperties
        {
            get
            {
                if (_pageProperties == null)
                {
                    string sql = "select * from PageDataProrpertiesView where PageId=" + PageId.ToString();
                    _pageProperties = DBUtils.GetDataSet(sql, cte.lib).Tables[0];
                }
                return _pageProperties;
            }
            set
            {
                _pageProperties = value;
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

        bool _getComments = false;
        /// <summary>
        /// Defines if the sql source should be fetched with comments.
        /// </summary>
        public bool GetComments
        {
            get
            {
                return _getComments;
            }
            set
            {
                _getComments = value;
            }
        }
        #endregion
    }
}
