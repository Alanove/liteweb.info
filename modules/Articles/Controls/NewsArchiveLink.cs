using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using lw.WebTools;

namespace lw.Articles.Controls
{
	public class NewsArchiveLink : HtmlAnchor
	{
		bool _bound = false;
		NewsLinkType _type = NewsLinkType.Archive;
		bool _IncludeNewsTypePath = false;
		string _innerText = "";
		string _format = "{0}";
		bool _hideText = false;
		string _path = "";

		public NewsArchiveLink()
		{
		}

		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;

			

			int _itemId = -1;
			object obj = DataBinder.Eval(this.NamingContainer, "DataItem.NewsType");

			if (!String.IsNullOrEmpty(obj.ToString()))
				_itemId = (int)obj;
			else
				return;

			object TypeUniqueName = DataBinder.Eval(this.NamingContainer, "DataItem.TypeUniqueName");
			object NewsMonthName = DataBinder.Eval(this.NamingContainer, "DataItem.NewsMonthName");
			object NewsMonth = DataBinder.Eval(this.NamingContainer, "DataItem.NewsMonth");

			switch (Type)
			{
				case NewsLinkType.ArchiveByType:
					this.HRef = string.Format("{2}/{0}/archive/{1:yyyy}/{1:MM}.aspx",
						TypeUniqueName,
							DateTime.Parse(NewsMonth.ToString()),
							WebContext.Root
						);
					break;
				case NewsLinkType.Archive:
				default:
					if (IncludeNewsTypePath)
					{
						this.HRef = string.Format("{3}/{0}/{1}/archives/{2:yyyy}/{2:MM}.aspx",
							Path == "" ? "articles" : Path,
							TypeUniqueName,
							DateTime.Parse(NewsMonth.ToString()),
							WebContext.Root
						);
					}
					else
					{
						this.HRef = string.Format("{2}/{0}/archives/{1:yyyy}/{1:MM}.aspx",
							Path == "" ? "articles" : Path,
							DateTime.Parse(NewsMonth.ToString()),
							WebContext.Root
						);
					}
					break;
			}
			if (this.Controls.Count == 0)
				this.InnerHtml = string.Format(Format, NewsMonthName);

			base.DataBind();
		}

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
		public string Path
		{
			get
			{
				return _path;
			}
			set
			{
				_path = value;
			}
		}
		public bool IncludeNewsTypePath
		{
			get { return _IncludeNewsTypePath; }
			set { _IncludeNewsTypePath = value; }
		}
		public bool HideText
		{
			get { return _hideText; }
			set { _hideText = value; }
		}

	}
}
