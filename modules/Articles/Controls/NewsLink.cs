using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using lw.Articles.LINQ;
using lw.CTE;
using lw.WebTools;
using lw.Base;

namespace lw.Articles.Controls
{

	/// <summary>
	/// Created a link to the associated news
	/// Must be included inside a DataContainer having NewsItem as DataItem
	/// </summary>
	public class NewsLink : HtmlAnchor
	{
		#region internal variables
		bool _bound = false;
		NewsLinkType _type = NewsLinkType.Auto;
		bool _IncludeNewsTypePath = false;
		string _innerText = "";
		string _format = "{0}";
		bool _hideText = false;
		string _path = null;
		string _linkExtension = "";
		int? ArticleId = null;

		string activeClass = "";

		#endregion

		public NewsLink()
		{
		}

		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;

			object obj = DataBinder.Eval(this.NamingContainer, "DataItem");//.NewsId");

			string a = DataBinder.Eval(obj, "NewsId").ToString();

			///TODO: fix here This is very wrong as it connects to the database for every link.
			//LINQ.NewsManager nMgr = new LINQ.NewsManager();
			//NewsDetailView _newsDetails = nMgr.GetNewsDetails(_itemId);

			BuildLink(obj);
			
			base.DataBind();

			if (MyPage.Editable)
			{
				this.Attributes.Add("data-editable", "true");
				this.Attributes.Add("data-id", a);
				this.Attributes.Add("data-type", "news");
			}
		}

		/// <summary>
		/// Builds the link and content for a news link.
		/// </summary>
		/// <param name="_newsDetails"></param>
		public void BuildLink(NewsDetailView _newsDetails)
		{
			string text = _newsDetails.Title;

			if (_newsDetails.Status == 0)
				this.Attributes["class"] = this.Attributes["class"] + " disabled";

			switch (Type)
			{
				case NewsLinkType.Auto:
					if (!String.IsNullOrEmpty(_newsDetails.NewsFile))
						goto case NewsLinkType.DirectFileLink;
					goto default;
				case NewsLinkType.DirectFileLink:
					if (string.IsNullOrEmpty(_newsDetails.NewsFile))
						goto default;
					this.HRef = string.Format("{2}/{0}/{1}",
						Folders.NewsFile,
						_newsDetails.NewsFile,
						WebContext.Root
					);
					this.Target = "_blank";
					break;
				case NewsLinkType.DownloadFile:
					if (string.IsNullOrEmpty(_newsDetails.NewsFile))
						goto default;
					this.HRef = string.Format("{1}/download-file.axd?file={0}&type=News",
						_newsDetails.NewsFile,
						WebContext.Root
					);
					break;
				case NewsLinkType.ArchiveByDate:
					text = string.Format("{0:MMMM}, {0:yyyy}", _newsDetails.NewsDate);
					this.HRef = string.Format("{3}/{0}/{1:yyyy}/{1:MM}{2}",
						Path == null ? "archives" : Path,
						_newsDetails.NewsDate,
						LinkExtension,
						WebContext.Root
					);
					break;
				case NewsLinkType.ArchiveByType:
					text = _newsDetails.TypeName;
					this.HRef = string.Format("{3}/{0}{1}{2}",
						Path == null ? "article-categories" : Path,
						_newsDetails.TypeUniqueName,
						LinkExtension,
						WebContext.Root
					);
					break;
				case NewsLinkType.ArticleID:
					if (IncludeNewsTypePath)
					{
						this.HRef = string.Format("{4}/{0}{1}/{2}{3}",
							Path == null ? "" : Path + "/",
							_newsDetails.TypeUniqueName,
							_newsDetails.NewsId,
							LinkExtension,
							WebContext.Root
						);
					}
					else
					{
						this.HRef = string.Format("{3}/{0}{1}{2}",
							Path == null ? "" : Path + "/",
							_newsDetails.NewsId,
							LinkExtension,
							WebContext.Root
						);
					}
					break;
				case NewsLinkType.Article:
				default:
					if (_newsDetails.UniqueName.IndexOf("http://") == 0)
					{
						this.HRef = _newsDetails.UniqueName;
						this.Target = "_blank";
					}
					else
					{
						if (IncludeNewsTypePath)
						{
							this.HRef = string.Format("{4}/{0}{1}/{2}{3}",
								Path == null ? "" : Path + "/",
								_newsDetails.TypeUniqueName,
								_newsDetails.UniqueName,
								LinkExtension,
								WebContext.Root
							);
						}
						else
						{
							this.HRef = string.Format("{3}/{0}{1}{2}",
								Path == null ? "" : Path + "/",
								_newsDetails.UniqueName,
								LinkExtension,
								WebContext.Root
							);
						}
					}
					break;
			}
			if (this.Controls.Count == 0)
				this.InnerHtml = string.Format(Format, text);
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

		/// <summary>
		/// Builds the link from an object
		/// </summary>
		/// <param name="_newsDetailsRow"></param>
		public void BuildLink(Object articleObject)
		{
			if (articleObject == null)
				return;

			SqlDataReader thisArticle = articleObject as SqlDataReader;
			NewsDetailView _articleDetails = null;

			if (thisArticle == null)
			{
				object obj = DataBinder.Eval(articleObject, "NewsId");
				if (obj != null)
					ArticleId = (int)obj;

				if (ArticleId == null)
					return;

				_articleDetails = articleObject as NewsDetailView;

				if (_articleDetails == null)
				{
					_articleDetails = new NewsDetailView
					{
						NewsId = ArticleId.Value,
						Title = DataBinder.Eval(articleObject, "Title").ToString(),
						NewsFile = DataBinder.Eval(articleObject, "NewsFile").ToString(),
						TypeName = DataBinder.Eval(articleObject, "TypeName").ToString(),
						TypeUniqueName = DataBinder.Eval(articleObject, "TypeUniqueName").ToString(),
						UniqueName = DataBinder.Eval(articleObject, "UniqueName").ToString()
					};
					DateTime? newsDate = DataBinder.Eval(articleObject, "NewsDate") as DateTime?;
					if (newsDate != null)
						_articleDetails.NewsDate = newsDate.Value;
				}

			}
			else
			{
				ArticleId = (int)thisArticle["NewsId"];

				_articleDetails = new NewsDetailView
				{
					NewsId = ArticleId.Value,
					Title = thisArticle["Title"].ToString(),
					NewsFile = thisArticle["NewsFile"].ToString(),
					NewsDate = (DateTime)thisArticle["NewsDate"],
					TypeName = thisArticle["TypeName"].ToString(),
					TypeUniqueName = thisArticle["TypeUniqueName"].ToString(),
					UniqueName = thisArticle["UniqueName"].ToString()
				};
			}
			BuildLink(_articleDetails);
		}


		#region Properties

		/// <summary>
		/// Type of link ,
		/// Auto,
		/// Article linked to the news UniqueName
		/// ArticleID id is added in the end
		/// DownloadFile link directly to the associated news file the file will be downloaded
		/// DirectFileLink link directly to the associated news file the file will open by default browswer behavior
		/// Archive ???
		/// ArchiveByDate ??
		/// ArchiveByType ??
		/// ArchiveByYear ???
		/// </summary>
		/// <seealso cref="lw.Tags.NewsLinkType"/>
		public NewsLinkType Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
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
		/// Flag to either include the news type in the path
		/// </summary>
		public bool IncludeNewsTypePath
		{
			get { return _IncludeNewsTypePath; }
			set { _IncludeNewsTypePath = value; }
		}

		/// <summary>
		/// Flag to either show or hide the text inside the tag
		/// </summary>
		public bool HideText
		{
			get { return _hideText; }
			set { _hideText = value; }
		}

		/// <summary>
		/// Extension to be added at the end of the link (default: .aspx)
		/// </summary>
		public string LinkExtension
		{
			get { return _linkExtension; }
			set { _linkExtension = value; }
		}

		/// <summary>
		/// this class is added to the tag if the news item is currently displayed in the same page.
		/// </summary>
		public string ActiveClass
		{
			get { return activeClass; }
			set { activeClass = value; }
		}

		#endregion

	}
}
