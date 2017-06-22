using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using lw.CTE;
using lw.CTE.Enum;
using lw.WebTools;

namespace lw.Products.Controls
{
	public class CategoryLink : HtmlAnchor
	{
		bool _bound = false;
		LinkType _type = LinkType.Title;
		string _format = "{0}";
		string _innerText = "";
		int _categoryId = -1;
		string destinationFolder;

		public CategoryLink()
		{
			Config cfg = new Config();
			destinationFolder = cfg.GetKey(Settings.CategoriesFolder);
		}
		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;

			object obj = ControlUtils.GetBoundedDataField(this.NamingContainer, "CategoryId");

			if (obj != null && obj.ToString() != "")
				_categoryId = Int32.Parse(obj.ToString());

			object title = ControlUtils.GetBoundedDataField(this.NamingContainer, "Title");

			switch (Type)
			{
				case LinkType.Image:
					object image = ControlUtils.GetBoundedDataField(this.NamingContainer, "Image");
					this.Controls.Add(new CategoryImage(image, title));
					break;
				case LinkType.Title:
					this.InnerHtml = string.Format(Format, title);
					break;
				default:
					this.InnerHtml = _innerText;
					break;
			}
			string uniqueName = ControlUtils.GetBoundedDataField(this.NamingContainer, "Name").ToString();
			this.Attributes.Add("rel", uniqueName);

			this.HRef = string.Format("{2}/{0}/{1}", 
				DestinationFolder, 
				uniqueName,
				WebContext.Root
			);

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