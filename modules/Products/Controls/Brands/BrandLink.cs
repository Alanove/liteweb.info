using System.Web.UI;
using System.Web.UI.HtmlControls;
using lw.CTE.Enum;
using lw.Utils;
using lw.WebTools;


namespace lw.Products.Controls
{
	public class BrandLink : HtmlAnchor
	{
		bool _bound = false;
		LinkType _type = LinkType.Title;
		string _innerText = "";
		int _brandId = -1;

		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;

			object obj = ControlUtils.GetBoundedDataField(this.NamingContainer, "BrandId");

			if (obj != null && obj.ToString() != "")
				_brandId = (int)obj;

			object title = ControlUtils.GetBoundedDataField(this.NamingContainer, "Title");

			switch (Type)
			{
				case LinkType.Image:
					object image = ControlUtils.GetBoundedDataField(this.NamingContainer, "Image");
					this.Controls.Add(new BrandImage(image, title));
					break;
				case LinkType.Title:
					this.InnerHtml = title.ToString();
					break;
				default:
					this.InnerHtml = _innerText;
					break;
			}
			this.HRef = string.Format("~/shop/by-brand/{0}/{1}.aspx",
				StringUtils.ToURL(title, "-"),
				_brandId);

			base.DataBind();
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
	}
}
