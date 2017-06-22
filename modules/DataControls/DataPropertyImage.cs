using System.Web.UI;


using lw.WebTools;

namespace lw.DataControls
{
	public class DataPropertyImage : System.Web.UI.WebControls.Literal
	{
		string property, format = "{0}";
		string _imageCss = "";
		string _font = "Verdana";
		int _fontSize = 8;
		int _fontRed = 0;
		int _fontGreen = 0;
		int _fontBlue = 0;

		protected override void Render(HtmlTextWriter writer)
		{
			object obj = DataBinder.Eval(this.NamingContainer, "DataItem." + Property);
			if (obj != System.DBNull.Value && obj != null && obj.ToString() != "")
			{
				string url = string.Format(@"ItemData.axd?ItemId={1}&Property={2}
					&Font={3}&FontSize={4}&FontRed={5}&
					FontGreen={6}&FontBlue={7}",
					WebContext.Root,
					DataBinder.Eval(this.NamingContainer, "DataItem.ItemId"),
					Property,
					Font, FontSize, FontRed, FontGreen, FontBlue);

				writer.Write(Format, string.Format("<img src=\"{0}\" class=\"{1}\">", url, ImageCss));
			}
			base.Render(writer);
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

		public string Format
		{
			get
			{
				return format;
			}
			set
			{
				format = value;
			}
		}
		public string Font
		{
			get
			{
				return _font;
			}
			set
			{
				_font = value;
			}
		}
		public int FontSize
		{
			get
			{
				return _fontSize;
			}
			set
			{
				_fontSize = value;
			}
		}
		public int FontRed
		{
			get
			{
				return _fontRed;
			}
			set
			{
				_fontRed = value;
			}
		}
		public int FontGreen
		{
			get
			{
				return _fontGreen;
			}
			set
			{
				_fontGreen = value;
			}
		}
		public int FontBlue
		{
			get
			{
				return _fontBlue;
			}
			set
			{
				_fontBlue = value;
			}
		}
		public string ImageCss
		{
			get
			{
				return _imageCss;
			}
			set
			{
				_imageCss = value;
			}
		}
	}
}
