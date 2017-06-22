using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using lw.CTE;
using lw.CTE.Enum;
using lw.Utils;
using lw.WebTools;

namespace lw.Products.Controls
{
	public class CategoryImageLink : HtmlAnchor
	{
		bool _bound = false;
		LinkType _type = LinkType.Image;
		string _format = "{0}";
		string _innerText = "";
		int _categoryId = -1;
		string destinationFolder;

		public CategoryImageLink()
		{
			Config cfg = new Config();
			destinationFolder = Folders.CategoriesImages;
		}
		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;

			object obj = ControlUtils.GetBoundedDataField(this.NamingContainer, "CategoryId");

			if (!String.IsNullOrEmpty(obj.ToString()))
				_categoryId = Int32.Parse(obj.ToString());
			else
				return;

			CategoriesMgr cMgr = new CategoriesMgr();
			DataRow cat = cMgr.GetCategory(_categoryId);

			if (StringUtils.IsNullOrWhiteSpace(cat["Image"]))
				this.Visible = false;
			else
			{
				switch (Type)
				{
					case LinkType.Title:
						this.InnerHtml = string.Format(Format, cat["Title"]);
						this.Title = cat["Title"].ToString();
						this.HRef = string.Format("~/{0}/Large/{1}", DestinationFolder, cat["Image"]);
						break;
					case LinkType.Image:
					default:
						this.Controls.Add(new CategoryImage(cat["Image"], cat["Title"]));
						this.Title = cat["Title"].ToString();
						this.HRef = string.Format("~/{0}/Large/{1}", DestinationFolder, cat["Image"]);
						break;
				}
			}

			base.DataBind();
		}

		public string DestinationFolder
		{
			get { return destinationFolder; }
			set { destinationFolder = value; }
		}
		public LinkType Type
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
		public string InnerText
		{
			get
			{
				return _innerText;
			}
			set
			{
				_innerText = value;
			}
		}
		public string Format
		{
			get { return _format; }
			set { _format = value; }
		}
	}
}