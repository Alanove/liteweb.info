using System.Web.UI;

using lw.WebTools;

namespace lw.Products.Controls
{
	public class CategoryImage : System.Web.UI.HtmlControls.HtmlImage
	{
		bool _bound = false;
		string type = "Thumb";
		string _Src = "";
		object _Image, _Title;

		public CategoryImage()
		{ }

		public CategoryImage(object image, object title)
		{
			_Image = image;
			_Title = title;
		}	

		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;

			object obj = ControlUtils.GetBoundedDataField(this.NamingContainer, "Image");

			if (obj != null && obj.ToString() != "")
			{
				this._Src = string.Format("{0}/{1}/{2}",
					lw.CTE.Folders.CategoriesImages, this.Type, obj);
			}

			obj = ControlUtils.GetBoundedDataField(this.NamingContainer, "Title");
			if (obj != null)
				this.Alt = obj.ToString();

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

		public string Type
		{
			get
			{
				return type;
			}
			set
			{
				type = value;
			}
		}
	}
}