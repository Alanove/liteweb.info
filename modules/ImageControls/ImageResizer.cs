using System;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using lw.WebTools;

namespace lw.ImageControls
{
	public class ImageResizer : WebControl, IHttpHandler, IDisposable
	{
		string src = "";
		bool _useStyles = true;
		bool _noResize = false;
		bool _crop = false;
		Color _fillColor = Color.Transparent;

		public ImageResizer(): base(HtmlTextWriterTag.Img)
		{
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (src != "")
			{
				if (!UseStyles)
				{
					writer.Write("<img src=\"{0}\" alt=\"{1}\"/>", this.Attributes["src"], this.Attributes["title"]);
					return;
				}
				base.Render(writer);
			}
		}

		public override void DataBind()
		{
			if (!NoResize)
			{
				this.Attributes.Add("src", string.Format("{4}/ImageResizer.axd?src={0}&width={1}&height={2}&fillColor={3}&crop={5}",
						src, this.Width.Value, this.Height.Value, FillColor.ToArgb(), WebContext.Root, Crop));
			}
			else
			{
				this.Attributes.Add("src", Src);
			}
			base.DataBind();
		}


		public void ProcessRequest(HttpContext context)
		{
			System.Web.HttpRequest Request = context.Request;
			System.Web.HttpServerUtility Server = context.Server;
			System.Web.HttpResponse Response = context.Response;


			string Image = Request.QueryString["src"];

			

			object obj = Request.QueryString["fillColor"];
			if (obj != null && obj.ToString() != "")
				_fillColor = Color.FromArgb(Int32.Parse(obj.ToString()));
			

			int width = 50, height = 50;

			if (Request.QueryString["width"] != null && Request["width"] != "")
				width = Int32.Parse(Request["width"]);

			if (Request["height"] != null && Request["height"] != "")
				height = Int32.Parse(Request["height"]);

			if (width <= 0)
				width = 99999;
			if (height <= 0)
				height = 99999;

			if (!String.IsNullOrEmpty(Request["Crop"]))
				Crop = bool.Parse(Request["Crop"]);

			Image = Server.MapPath("~/" + Image);



			if (!System.IO.File.Exists(Image))
			{
				Response.Write(Image);
				return;
			}

			System.Drawing.Image im = System.Drawing.Image.FromFile(Image);

		//	Response.ContentType = "image/png";

			System.Drawing.Image resp = null;


			if (Crop)
				resp = lw.GraphicUtils.ImageUtils.Crop(im, width, height, lw.GraphicUtils.ImageUtils.AnchorPosition.Default);
			else
				resp = lw.GraphicUtils.ImageUtils.FixedSize(im, width, height);

			//resp.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png);

			Response.Write(resp);
			Response.End();

			resp.Save(Server.MapPath("~/prv/ballout.png"));

			resp.Dispose();
			im.Dispose();
		}

		public bool NoResize
		{
			get { return _noResize; }
			set { _noResize = value; }
		}

		public string Src
		{
			get
			{
				return src;
			}
			set
			{
				src = value;
			}
		}
	
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
		public bool UseStyles
		{
			get { return _useStyles; }
			set { _useStyles = value; }
		}
		public Color FillColor
		{
			get { return _fillColor; }
			set { _fillColor = value; }
		}

		public bool Crop
		{
			get { return _crop; }
			set { _crop = value; }
		}
	}
}