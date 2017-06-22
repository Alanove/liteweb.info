using lw.WebTools;
using System.Web.UI;
using System.Web.UI.HtmlControls;



namespace lw.Products.Controls
{
	public class ItemImageLink : HtmlAnchor
	{
		bool _bound = false;
		string image = "";
		int _itemId = -1;
		int imageNumber = 1;
		bool _thumbImage = false;
		int width, height;

		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;

			object obj = ControlUtils.GetBoundedDataField(this.NamingContainer, "ItemId");

			if (obj != null && obj.ToString() != "")
				_itemId = (int)obj;

			object title = ControlUtils.GetBoundedDataField(this.NamingContainer, "Title");

			if (ThumbImage)
				image = ControlUtils.GetBoundedDataField(this.NamingContainer, "ThumbImage").ToString();
			else
				image = ControlUtils.GetBoundedDataField(this.NamingContainer, "Image" + imageNumber.ToString()).ToString();

			this.HRef = string.Format("~/{0}/{2}/Large/{1}", 
				lw.CTE.Folders.ProductsImages, 
				image, 
				ControlUtils.GetBoundedDataField(this.NamingContainer, "UniqueName").ToString());

			this.Title = title.ToString();

			if(this.Controls.Count == 0)
				this.Controls.Add(new ItemImage(image, "Click to Enlarge", width, height));

			base.DataBind();
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if(image != "")
				base.Render(writer);
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
	}
}