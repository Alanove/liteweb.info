using System.Drawing;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using lw.ImageControls;
using lw.WebTools;
using lw.DataControls;

namespace lw.Products.Controls
{
	public class ItemImage:HtmlImage
	{
		bool _bound = false;
		int number = 1;
		string _Src = "";
		ImageType imageType = ImageType.Thumb;
		object _Image = null, _Title = null;
		bool _thumbImage = false;
		string bigImage = "";
		Color fillColor = Color.Transparent;
		string _itemUniqueName = "";
		

		public ItemImage()
		{
		}
		public ItemImage(object image, object title, int width, int height)
		{
			_Image = image;
			_Title = title;
			if(width > 0)
				this.Width = width;
			if(height > 0)
				this.Height = height;
		}

		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;

			_itemUniqueName = ControlUtils.GetBoundedDataField(this.NamingContainer, "UniqueName").ToString();

			if (_Image == null)
			{
				if(ThumbImage)
					_Image = ControlUtils.GetBoundedDataField(this.NamingContainer, "ThumbImage");
				else
					_Image = ControlUtils.GetBoundedDataField(this.NamingContainer, "Image" + number.ToString());
			}
			if (_Image != null && _Image.ToString() != "")
			{
				switch (Type)
				{
					case ImageType.Resize:
						bigImage = string.Format("{0}/{3}/{1}/{2}",
								lw.CTE.Folders.ProductsImages,
								"Large",
								_Image,
								_itemUniqueName
							);

						this._Src = string.Format("ImageResizer.axd?src={0}&width={1}&height={2}&fillColor={3}",
								bigImage, this.Width, this.Height, FillColor.ToArgb(), WebContext.Root);
						break;
					case ImageType.Thumb:
					case ImageType.Medium:
					case ImageType.Large:
					default:
						this._Src = string.Format("{4}/{0}/{3}/{1}/{2}",
								lw.CTE.Folders.ProductsImages, 
								this.Type, 
								_Image,
								_itemUniqueName,
								WebContext.Root
							);
						break;
				}
			}
			else if (Type == ImageType.Thumb)
			{
				if (HideIfNoImage)
					this.Visible = false;
				this._Src = "/images/image-not-available.gif";
			}
			
			if (_Title == null)
				_Title = ControlUtils.GetBoundedDataField(this.NamingContainer, "Title");
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
			if (this._Src != "")
			{
				if(this.imageType == ImageType.Resize)
					this.Src = this._Src;
				else
					this.Src = WebContext.Root + this._Src;
				if (this.bigImage != "")
				{
					this.Attributes["rel"] = bigImage;
				}
				base.Render(writer);
			}
		}

		public int Number
		{
			get
			{
				return number;
			}
			set
			{
				number = value;
			}
		}

		public ImageType Type
		{
			get
			{
				return imageType;
			}
			set
			{
				imageType = value;
			}
		}
		public bool ThumbImage
		{
			get
			{
				return _thumbImage;
			}
			set
			{
				_thumbImage = value;
			}
		}
		public Color FillColor
		{
			get { return fillColor; }
			set { fillColor = value; }
		}


		bool _hideIfNoImage = false;
		public bool HideIfNoImage
		{
			get
			{
				return _hideIfNoImage;
			}
			set
			{
				_hideIfNoImage = value;
			}
		}
	}
	
}