using System.Data;
using System.Web.UI;
using System;

using lw.Utils;
using lw.WebTools;

namespace lw.Articles.Controls
{
	public class NewsPictures : System.Web.UI.WebControls.Literal
	{
		#region internal variables
		bool _bound = false;
		int? _newsId = null;
		string imagesPath = "";
		string _newsTitle = "";
		int? max = null;
		string cssClass = "";
		#endregion


		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			try
			{
				object newsId = ControlUtils.GetBoundedDataField(this.NamingContainer, "NewsId");
				_newsId = (int)newsId;
				_newsTitle = ControlUtils.GetBoundedDataField(this.NamingContainer, "Title").ToString();


				this.Text = GetPictures();

				base.DataBind();
			}
			catch (Exception ex)
			{

			}
		}

		protected string GetPictures()
		{
			if (_newsId == null)
				return "";


			NewsManager newsMgr = new NewsManager();

			DataView images = newsMgr.NewsImages(_newsId.Value);

			if (images.Count == 0)
				return "";

			imagesPath = lw.WebTools.WebContext.Root + lw.CTE.Folders.NewsImages;

			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			sb.Append("<ul class=\"news-images\">");

			int imageCount = Max  != null ? Math.Min(Max.Value, images.Count) : images.Count;

			string className = CssClass != "" ? CssClass : "";

			var counter = 0;

			for(int i= 0; i < imageCount; i++)
			{
				DataRowView im = images[i];
				sb.Append(string.Format(@"<li><a class=""{4}"" href=""{0}/News{1}/Large/{2}"" data-lightbox=""newsimageset-{5}"">
					<img src=""{0}/News{1}/Thumb/{2}"" class=""imgnews"" alt=""{3}"" />
				</a></li>",
					imagesPath,
					_newsId.Value,
					im["Image"],
					StringUtils.StripOutHtmlTags(_newsTitle),
					className,
					_newsId
					));
			}
			sb.Append("</ul>");

			return sb.ToString();
		}

		public int? Max
		{
			get
			{
				return max;
			}
			set
			{
				max = value;
			}
		}
		public string CssClass
		{
			get
			{
				return cssClass;
			}
			set
			{
				cssClass = value;
			}
		}
	}
}
