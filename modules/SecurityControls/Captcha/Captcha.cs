using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using lw.GraphicUtils.Filters;
using lw.Utils;
using lw.WebTools;

namespace lw.SecurityControls
{
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:CaptchaImage runat=server></{0}:CaptchaImage>")]
	public class CaptchaImage : WebControl, IHttpHandler, System.Web.SessionState.IRequiresSessionState
	{
		private string _UniqueID = string.Empty;
		private int _size = 7;
		int _width = 200, _height = 100;
		Color textColor = Color.Red, bgColor = Color.White;

		public CaptchaImage()
			: base(HtmlTextWriterTag.Img)
		{
			this.Attributes["Id"] = this.MyUniqueID;
		}

		#region variables
		private string MyUniqueID
		{
			get
			{
				if (String.IsNullOrWhiteSpace(_UniqueID))
					_UniqueID = this.UniqueID;
				return _UniqueID;
			}
		}
		
#endregion





		public string Text
		{
			get
			{
				return string.Format("{0}", WebContext.Cache[this.MyUniqueID]);
			}
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Page.RegisterRequiresControlState(this);
		}
		protected override void LoadControlState(object savedState)
		{
			Pair p = savedState as Pair;
			if (p != null)
			{
				_UniqueID = p.Second as string;
			}
		}
		
		
		protected override object SaveControlState()
		{
			Object baseState = base.SaveControlState();
			Pair prState = new Pair(baseState, this.MyUniqueID);
			return prState;
		}
		protected override void Render(HtmlTextWriter output)
		{
			string path = string.Format("{0}/{1}?uid={2}",
				WebContext.Root,
				lw.CTE.Folders.CaptchaURL,
				this.MyUniqueID);
			output.AddAttribute(HtmlTextWriterAttribute.Src, path);
			base.Render(output);
output.Write("<script type=\"text/javascript\">");
output.Write("function __RefreshImageVerifier(el)");
output.Write("{");
output.Write("  var dt = new Date();");
output.Write("  el.src='" + path + "&ts=' + dt;");
output.Write("}</script>");
		}

		public void ProcessRequest(HttpContext context)
		{
			Bitmap bmp = new Bitmap(MyWidth, MyHeight);
			Graphics g = Graphics.FromImage(bmp);

			string randString = StringUtils.GenerateRandomText(_size);
			if (WebContext.Cache[CacheKey] == null)
				WebContext.Cache.Add(CacheKey, randString, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5), System.Web.Caching.CacheItemPriority.Normal, null);
			else
				WebContext.Cache[CacheKey] = randString;

			Brush bgBrush = new SolidBrush(BgColor);

			g.FillRectangle(bgBrush, 0, 0, (int)_width, (int)_height);
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

			Random rand = new Random();
			Color c = TextColor;
			Brush br = new SolidBrush(c);

			Font drawFont = new Font("Trebuchet MS", 32, FontStyle.Bold);

			float startPos = 10;
			for (int i = 0; i < randString.Length; i++)
			{
				string myChar = randString.Substring(i, 1);
				SizeF stringSize = g.MeasureString(myChar, drawFont);

				g.DrawString(myChar, drawFont, br, startPos, rand.Next() % 30);

				startPos += stringSize.Width * (float)0.545;
			}

			
			SkewFilter sk = new SkewFilter();


			int ranMax = 60;
			if (rand.Next() % 100 > 50)
				sk.UpShift = -1 * rand.Next(40, ranMax);
			else
				sk.UpShift = rand.Next(40, ranMax);

			sk.RightShift = -1 * sk.UpShift;// -rand.Next() % ranMax;

			sk.BackGroundColor = BgColor;



			System.IO.MemoryStream s = new System.IO.MemoryStream();
			bmp.Save(s, ImageFormat.Jpeg);

			System.Drawing.Image im = Bitmap.FromStream(s);

			im = sk.ExecuteFilter(im);
			
			ResizeFilter rs = new ResizeFilter();
			rs.Width = MyWidth;
			rs.Height = MyHeight;
			im = rs.ExecuteFilter(im);
			



			//lw.GraphicUtils.Filters.TextWatermarkFilter f = new TextWatermarkFilter();
			//im = f.ExecuteFilter(im);

			//lw.GraphicUtils.Filters.BitmapFilter.Smooth(bmp, 20);

			bmp.Dispose();
			bmp = new Bitmap(im);
			im.Dispose();

			context.Response.Clear();
			context.Response.ClearHeaders();
			context.Response.ContentType = "image/jpeg";
			bmp.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
			context.Response.End();
			
			g.Dispose();
			bmp.Dispose();
		}

		#region functions
		public bool Validate(string text)
		{
			if (WebContext.Cache[CacheKey] != null)
				return string.Equals(text, WebContext.Cache[CacheKey].ToString(), StringComparison.OrdinalIgnoreCase);

			return false;
		}
		#endregion


		#region properties

		public Color TextColor
		{
			get
			{
				if (WebContext.Session[string.Format("{0}TextColor", this.MyUniqueID)] != null)
					textColor = (Color)HttpContext.Current.Session[string.Format("{0}TextColor", this.MyUniqueID)];
				return textColor;
			}
			set
			{
				WebContext.Session.Add(string.Format("{0}TextColor", this.MyUniqueID), value);
				textColor = value;
			}
		}
		public Color BgColor
		{
			get
			{
				if (WebContext.Session[string.Format("{0}BgColor", this.MyUniqueID)] != null)
					bgColor = (Color)HttpContext.Current.Session[string.Format("{0}BgColor", this.MyUniqueID)];
				return bgColor;
			}
			set
			{
				WebContext.Session.Add(string.Format("{0}BgColor", this.MyUniqueID), value);
				bgColor = value;
			}
		}
		public int Size
		{
			get
			{
				if (WebContext.Session[string.Format("{0}Size", this.MyUniqueID)] != null)
					_size = (int)HttpContext.Current.Session[string.Format("{0}Size", this.MyUniqueID)];
				return _size;
			}
			set
			{
				WebContext.Session.Add(string.Format("{0}Size", this.MyUniqueID), value);//, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5), System.Web.Caching.CacheItemPriority.Normal, null);
				_size = value;
			}
		}
		public int MyWidth
		{
			get
			{
				if (WebContext.Session[string.Format("{0}Width", this.MyUniqueID)] != null)
					_width = (int)HttpContext.Current.Session[string.Format("{0}Width", this.MyUniqueID)];
				return _width;
			}
			set
			{
				WebContext.Session.Add(string.Format("{0}Width", this.MyUniqueID), value);//, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5), System.Web.Caching.CacheItemPriority.Normal, null);
				_width = value;
			}
		}
		public int MyHeight
		{
			get
			{
				if (HttpContext.Current.Session[string.Format("{0}Height", this.MyUniqueID)] != null)
					_height = (int)HttpContext.Current.Session[string.Format("{0}Height", this.MyUniqueID)];
				return _height;
			}
			set
			{
				WebContext.Session.Add(string.Format("{0}Height", this.MyUniqueID), value);//, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5), System.Web.Caching.CacheItemPriority.Normal, null);
				_height = value;
			}
		}

		public string CacheKey
		{
			get
			{
				return WebContext.Profile.UserName + "capt";
			}
		}

		#endregion

		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
	}

	public class ImageVerifier : CaptchaImage
	{
	}
}
