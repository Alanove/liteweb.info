using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using lw.CTE;
using lw.WebTools;
using lw.Base;
using lw.DataControls;
using lw.Utils;

namespace lw.Pages.Controls
{

	/// <summary>
	/// Created a link to the associated news
	/// Must be included inside a DataContainer having NewsItem as DataItem
	/// </summary>
	[PersistChildren(false)]
	public class PageLink : HtmlAnchor
	{
		#region internal variables
		bool _bound = false;
		string _format = "{0}";
		string _path = "";
		string _anchor = "";
		string _editDataType = "news"; // Should Be Pages

		bool _showOnlyInCMSMode = false;

		string activeClass = "";

		#endregion

		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;

			if (this.Controls.Count == 0)
				this.InnerHtml = StringUtils.AddSup(StringUtils.ReplaceNbSup(Title));

			this.Title = Title;
			this.HRef = FullURL;

			if (!String.IsNullOrWhiteSpace(Path))
			{
				this.HRef = Path + "/" + this.HRef;
			}
			this.HRef = WebContext.Root + "/" + this.HRef + Anchor;

			if (this.URL.Contains("://"))
			{
				this.HRef = this.URL;
				this.Target = "_blank";
			}
			else if (this.URL.Contains("mailto:"))
			{
				this.HRef = this.URL;
			}

			string pageFile = ControlUtils.GetBoundedDataField(this.NamingContainer, "PageFile").ToString();

			if (!ForceURL && !String.IsNullOrWhiteSpace(pageFile))
			{
				this.HRef = String.Format("{0}/{3}/Page_{1}/{2}", WebContext.Root, PageId, pageFile, lw.CTE.Folders.PagesFolder);
			}

			if (ShowOnlyInCMSMode)
				this.Visible = false;

			base.DataBind();

			if (MyPage.Editable && ImEditable)
			{
				this.Attributes.Add("data-editable", "true");
				this.Attributes.Add("data-id", PageId.ToString());
				this.Attributes.Add("data-type", EditDataType);
				if (ShowOnlyInCMSMode)
					this.Visible = true;
			}
			this.Attributes["class"] = this.Attributes["class"] + " " + ControlUtils.GetBoundedDataField(this.NamingContainer, "URL");
			this.Attributes.Add("title", string.Format(Format, Title));
		}

		CustomPage myPage = null;
		CustomPage MyPage
		{
			get
			{
				if (myPage == null)
				{
					myPage = this.Page as CustomPage;
				}
				return myPage;
			}

		}



		#region Properties

		int? _pageId;
		public int? PageId
		{
			get
			{
				if (_pageId == null)
					_pageId = (int)ControlUtils.GetBoundedDataField(this.NamingContainer, "PageId");
				return _pageId;
			}
			set
			{
				_pageId = value;
			}
		}

		string _title = null;
		public string Title
		{
			get
			{
				if (_title == null)
					_title = ControlUtils.GetBoundedDataField(this.NamingContainer, "Title").ToString();
				return _title;
			}
			set
			{
				_title = value;
			}
		}


		string _fullURL = null;
		public string FullURL
		{
			get
			{
				if (_fullURL == null)
					_fullURL = ControlUtils.GetBoundedDataField(this.NamingContainer, "FullURL").ToString();

				if (!String.IsNullOrWhiteSpace(RemovePath))
				{
					_fullURL = _fullURL.Replace(RemovePath + "/", "");
				}

				return _fullURL;
			}
			set
			{
				_fullURL = value;
			}
		}

		string _url = null;
		public string URL
		{
			get
			{
				if (_url == null)
					_url = ControlUtils.GetBoundedDataField(this.NamingContainer, "URL").ToString();

				return _url;
			}
			set
			{
				_url = value;
			}
		}



		/// <summary>
		/// Text format inside the tag
		/// </summary>
		public string Format
		{
			get
			{
				return _format;
			}
			set
			{
				_format = value;
			}
		}

		/// <summary>
		/// static string to be added before the generated dynamic link
		/// </summary>
		public string Path
		{
			get { return _path; }
			set { _path = value; }
		}

		/// <summary>
		/// static string to be added after the generated dynamic link
		/// </summary>
		public string Anchor
		{
			get { return _anchor; }
			set { _anchor = value; }
		}


		/// <summary>
		/// this class is added to the tag if the news item is currently displayed in the same page.
		/// </summary>
		public string ActiveClass
		{
			get { return activeClass; }
			set { activeClass = value; }
		}


		bool _imEditable = false;
		/// <summary>
		/// Indicates if this link should contain the editing parameters
		/// </summary>
		public bool ImEditable
		{
			get
			{
				return _imEditable;
			}
			set
			{
				_imEditable = value;
			}
		}
		/// <summary>
		/// Show page link only in cms mode
		/// </summary>
		public bool ShowOnlyInCMSMode
		{
			get
			{
				return _showOnlyInCMSMode;
			}
			set
			{
				_showOnlyInCMSMode = value;
			}
		}


		private string _removePath = "";

		/// <summary>
		/// Removes a certain path from the link
		/// ex: mainmenu/parent/page (with RemovePath=mainmenu) becomes: parent/page
		/// Usefull when you have categories of menus and when you don't want to show the top parent menu
		/// </summary>
		public string RemovePath
		{
			get { return _removePath; }
			set { _removePath = value; }
		}

		bool _forceURL = false;
		/// <summary>
		/// Forces the link to open the page URL when the page file is present.
		/// </summary>
		public bool ForceURL
		{
			get
			{
				return _forceURL;
			}
			set
			{
				_forceURL = value;
			}
		}


		/// <summary>
		/// Get PageLink Type when CMS Mode to Edit
		/// </summary>
		public string EditDataType
		{
			get { return _editDataType; }
			set { _editDataType = value; }
		}



		#endregion

	}
}
