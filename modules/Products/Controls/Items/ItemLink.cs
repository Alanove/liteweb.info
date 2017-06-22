using System.Drawing;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using lw.CTE.Enum;
using lw.ImageControls;
using lw.Utils;
using lw.WebTools;

namespace lw.Products.Controls
{ 

	public class ItemLink : HtmlAnchor
	{
		#region internal varialbles
		bool _bound = false;
		LinkType _type = LinkType.Title;
		ImageType imageType = ImageType.Thumb;
		string _innerText = "";
		int _itemId = -1;
		int imageNumber = 1;
		bool _thumbImage = true;
		int width, height;
		bool ResizeImage = false;
		Color fillColor = Color.Transparent;
		string property;
		string path = "";
		#endregion

		public ItemLink()
		{
			Config cfg = new Config();
			path = cfg.GetKey(lw.CTE.RoutingParameters.ProductDetailsFolder);
		}

		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;

			object obj = DataBinder.Eval(this.NamingContainer, "DataItem.ItemId");

			if (obj != null && obj.ToString() != "")
				_itemId = (int)obj;


			object title = DataBinder.Eval(this.NamingContainer, "DataItem.Title");
			object _prop = null;

			if (!StringUtils.IsNullOrWhiteSpace(Property))
				_prop = DataBinder.Eval(this.NamingContainer, "DataItem." + Property);

			switch (Type)
			{
				case LinkType.Image:
					object image;
					if (ThumbImage)
						image = DataBinder.Eval(this.NamingContainer, "DataItem.ThumbImage");
					else
						image = DataBinder.Eval(this.NamingContainer, "DataItem.Image" + imageNumber.ToString());

					ItemImage img = new ItemImage(image, title, width, height);
					img.Type = ImageType;
					img.FillColor = FillColor;
					this.Controls.Add(img);
					break;
				case LinkType.Title:
					if (StringUtils.IsNullOrWhiteSpace(Property))
						this.InnerHtml = title.ToString();
					else
						this.InnerHtml = _prop.ToString();
					break;
				default:
					this.InnerHtml = _innerText;
					break;
			}

			Config cfg = new Config();
			this.HRef = string.Format("{2}/{1}/{0}", 
				DataBinder.Eval(this.NamingContainer, "DataItem.UniqueName"),
				path,
				WebContext.Root
			);

			base.DataBind();
		}



		#region Properties
		public string Path
		{
			get { return path; }
			set { path = value; }
		}

		public string Property
		{
			get
			{
				return property;
			}
			set
			{
				property = value;
			}
		}

		public int ImageNumber
		{
			get
			{
				return imageNumber;
			}
			set
			{
				imageNumber = value;
			}
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
		public int Width
		{
			get { return width; }
			set { width = value; }
		}
		public int Height
		{
			get { return height; }
			set { height = value; }
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
		public ImageType ImageType
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
		public Color FillColor
		{
			get { return fillColor; }
			set { fillColor = value; }
		}

		#endregion
	}

}
