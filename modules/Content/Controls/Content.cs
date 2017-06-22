using System;
using System.IO;
using lw.Base;
using lw.WebTools;


namespace lw.Content.Controls
{
	/// <summary>
	/// Renders the page content (text)
	/// </summary>
	public class Content : Message
	{
		bool _editable = false;
		string pageName;
		bool overridePageProperties = false;
		bool _bound = false;
		string _param = "page";
		bool _createPage = true;

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			CustomPage thisPage = this.Page as CustomPage;
			if (thisPage == null)
				thisPage = new CustomPage();
			
			ContentManager pagesMgr = new ContentManager();
			PagesDS.PagesRow page = pagesMgr.GetPage(PageName, thisPage.Language);

			if (page == null && CreatePage)
			{
				pagesMgr.AddPage(pageName, pageName, "", false, ContentManager.Content(pageName), thisPage.Language);
				page = pagesMgr.GetPage(pageName, thisPage.Language);
			}
			if (page == null)
				return;

			string str = page.Content;

			str = str.Replace("<sup>&amp;reg;</sup>", "®");
			str = str.Replace("<sup>®</sup>", "®");
			str = str.Replace("&amp;reg;", "&reg;");
			str = str.Replace("&reg;", "<sup>&reg;</sup>");
			str = str.Replace("®", "<sup>&reg;</sup>");


			this.Text = str;

			if (OverridePageProperties)
			{
				if (thisPage != null)
				{
					Config cfg = new Config();
					thisPage.Title = page.Title;
					thisPage.AddDescription(page.Description);
					thisPage.AddKeywords(page.Keywords);
				}
			}

			base.DataBind();
		}
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (Editable)
			{
				//<div class=Eheader><a href=""javascript:lw_Editor.edit('{0}', '{1}')"">Edit</a></div>					

				writer.Write(string.Format(@"<div contenteditable=true class=""editingarea"">
<div id=""Edit_{0}"">", this.UniqueID.Replace(":", "_"), PageName));

				base.Render(writer);

				writer.Write(string.Format("</div>",
					this.UniqueID.Replace(":", "_"), this.PageName));
				writer.Write("</div>");
			}
			else
				base.Render(writer);
		}
		#region properties

		public string Param
		{
			get
			{
				return _param;
			}
			set 
			{
				_param = value;
			}
		}

		public string PageName
		{
			get
			{
				if (String.IsNullOrEmpty(pageName))
				{
					CustomPage page = this.Page as CustomPage;
					if (page != null)
					{
						pageName = page.GetQueryValue(Param);
					}
				}
				return pageName;
			}
			set
			{
				pageName = value;
			}
		}
		public bool Editable
		{
			get
			{
				CustomPage page = this.Page as CustomPage;
				this._editable = page != null ? page.Editable : false;
				return _editable;
			}
		}
		public bool OverridePageProperties
		{
			get { return overridePageProperties; }
			set { overridePageProperties = value; }
		}

		public bool CreatePage
		{
			get
			{
				return _createPage;
			}
			set
			{
				_createPage = value;
			}
		}

		#endregion
	}
}
