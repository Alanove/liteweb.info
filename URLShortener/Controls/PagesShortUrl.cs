using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using lw.Base;
using lw.URLShortener;
using lw.URLShortener.data;
using System.Data.SqlClient;
using lw.Pages;
using lw.Pages.data;


namespace lw.URLShortener.Controls
{
	public class PagesShortUrl : HtmlAnchor
	{
		#region internal variables
		bool _bound = false;
		bool _IncludePagesTypePath = false;
		string _innerText = "";
		string _format = "{0}";
		bool _hideText = false;
		string _path = null;
		string _linkExtension = "";
		int? ArticleId = null;
		string _hideFromFullUrl = "";

		string activeClass = "";

		#endregion

		public PagesShortUrl()
		{
		}

		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;

			object obj = DataBinder.Eval(this.NamingContainer, "DataItem");

			string a = DataBinder.Eval(obj, "PageId").ToString();

			///TODO: fix here This is very wrong as it connects to the database for every link.

			BuildLink(obj);
			
			base.DataBind();

			//if (MyPage.Editable)
			//{
			//	this.Attributes.Add("data-editable", "true");
			//	this.Attributes.Add("data-id", a);
			//	this.Attributes.Add("data-type", "news");
			//}
		}

		/// <summary>
		/// Builds the link and content for a news link.
		/// </summary>
		/// <param name="_pagesDetails"></param>
		public void BuildLink(Pages_View _pagesDetails)
		{
			string pageUrl = _pagesDetails.FullURL;
			if (!String.IsNullOrWhiteSpace(HideFromFullUrl))
				pageUrl = pageUrl.Replace(HideFromFullUrl, "");

			string url = WebTools.WebContext.Protocol + "://" + WebTools.WebContext.ServerName +
				"/" + pageUrl;

			RedirectsManager rMgr = new RedirectsManager();

			var _href = rMgr.AddRedirection(url);

			if (!string.IsNullOrWhiteSpace(_href))
				this.HRef = "http://sab.is/" + _href;

			this.Target = "_blank";
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
			Pages_View _articleDetails = null;

			if (thisArticle == null)
			{
				object obj = DataBinder.Eval(articleObject, "PageId");
				if (obj != null)
					ArticleId = (int)obj;

				if (ArticleId == null)
					return;

				_articleDetails = articleObject as Pages_View;

				if (_articleDetails == null)
				{
					_articleDetails = new Pages_View
					{
						PageId = ArticleId.Value,
						Title = DataBinder.Eval(articleObject, "Title").ToString(),
						PageFile = DataBinder.Eval(articleObject, "PageFile").ToString(),
//						TypeName = DataBinder.Eval(articleObject, "TypeName").ToString(),
//						TypeUniqueName = DataBinder.Eval(articleObject, "TypeUniqueName").ToString(),
						URL = DataBinder.Eval(articleObject, "URL").ToString()
					};
					DateTime? pageDate = DataBinder.Eval(articleObject, "PublishDate") as DateTime?;
					if (pageDate != null)
						_articleDetails.PublishDate = pageDate.Value;
				}

			}
			else
			{
				ArticleId = (int)thisArticle["NewsId"];

				_articleDetails = new Pages_View
				{
					PageId = ArticleId.Value,
					Title = thisArticle["Title"].ToString(),
					PageFile = thisArticle["PageFile"].ToString(),
					PublishDate = (DateTime)thisArticle["PublishDate"],
					//TypeName = thisArticle["TypeName"].ToString(),
					//TypeUniqueName = thisArticle["TypeUniqueName"].ToString(),
					URL = thisArticle["URL"].ToString()
				};
			}
			BuildLink(_articleDetails);
		}


		/// <summary>
		/// returns the string to be removed from the fullurl of the pages
		/// </summary>
		public string HideFromFullUrl
		{
			get { return _hideFromFullUrl; }
			set { _hideFromFullUrl = value; }
		}
	}
}