using System.Web.UI;

using lw.WebTools;

namespace lw.Products.Controls
{
	public class BrandImage : System.Web.UI.HtmlControls.HtmlImage
	{
		bool _bound = false;
		string _Src = "";
		object _Image = null, _Title = null;

		public BrandImage(object image, object title)
		{
			_Image = image;
			_Title = title;
		}

		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;

			if (_Image == null)
				_Image = ControlUtils.GetBoundedDataField(this.NamingContainer, "Image");

			if (_Image != null && _Image.ToString() != "")
			{
				this._Src = string.Format("{0}/{1}",
					lw.CTE.Folders.BrandsImages, _Image);
			}

			if (_Title == null)
				_Title = ControlUtils.GetBoundedDataField(this.NamingContainer, "Title");
			if (_Title != null)
				this.Alt = _Title.ToString();

			this.Attributes["Title"] = _Title.ToString();

			base.DataBind();
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (!_bound)
				this.DataBind();
			if (this._Src != "")
			{
				this.Src = WebContext.Root + this._Src;
				base.Render(writer);
			}
		}
	}
	

}
