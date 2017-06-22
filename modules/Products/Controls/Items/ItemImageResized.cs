using System.Web.UI;
using System.Web.UI.WebControls;
using lw.ImageControls;
using lw.WebTools;

namespace lw.Products.Controls
{
	public class ItemImageResized : ImageResizer
	{
		bool _bound = false;
		int number = 1;
		string _Src = "";
		ImageType imageType = ImageType.Thumb;
		object _Image = null, _Title = null;
		bool _thumbImage = false;
		

		public ItemImageResized()
		{
		}
		public ItemImageResized(object image, object title, int width, int height)
		{
			_Image = image;
			_Title = title;
			this.Width = new Unit(width);
			this.Height = new Unit(height);
		}

		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;

			if (_Image == null)
			{
				if(ThumbImage)
					_Image = DataBinder.Eval(this.NamingContainer, "DataItem.ThumbImage");
				else
					_Image = DataBinder.Eval(this.NamingContainer, "DataItem.Image" + number.ToString());
			}
			if (_Image != null && _Image.ToString() != "")
			{
				this.Src = string.Format("{3}/{0}/{1}/{2}",
					lw.CTE.Folders.ProductsImages, this.Type, _Image,
					WebContext.Root);
			}

			if (_Title == null)
				_Title = DataBinder.Eval(this.NamingContainer, "DataItem.Title");
			if (_Title != null)
			{
				this.Attributes["Title"] = _Title.ToString();
			}
			base.DataBind();
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (!_bound)
				this.DataBind();
			base.Render(writer);
		}
		public int Number
		{
			get{return number;}
			set{number = value;}
		}
		public ImageType Type
		{
			get{return imageType;}
			set{imageType = value;}
		}
		public bool ThumbImage
		{
			get { return _thumbImage; }
			set { _thumbImage = value; }
		}
		
	}
	
}