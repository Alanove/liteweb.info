using System;
using System.Data;
using System.Web.UI;

using lw.CTE.Enum;
using lw.Utils;
using lw.WebTools;
using lw.DataControls;
using lw.Pages;
using lw.ImageControls;


namespace lw.Pages.Controls
{
	/// <summary>
	/// Links to the album's image
	/// </summary>
	public class PageImageLink  : lw.Base.BaseControl
	{
		bool _bound = false;
		object DataObj;
		string noImage = "";
		object src = null;
		object alt;
		int pageId;


		public PageImageLink()
			: base("a")
		{
			RenderContainerTag = true;
		}

		void bind()
		{
			if (_bound)
				return;

			pageId = (int)ControlUtils.GetBoundedDataField(this.NamingContainer, "PageId");
			alt = ControlUtils.GetBoundedDataField(this.NamingContainer, "Header");
			src = ControlUtils.GetBoundedDataField(this.NamingContainer, "Image");



			_bound = pageId != null;
		}

		public override void DataBind()
		{
			bind();
			
			string temp;
			string href = "";

			if (_bound)
			{
				if (src != null)
				{
					temp = src.ToString();

					switch (NewsImageType)
					{
						case ImageType.Thumb:
							temp = PagesManager.GetThumbImage(pageId, temp);
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								href =  WebContext.Root + "/" +  temp;
							break;
						case ImageType.Medium:
							temp = PagesManager.GetMediumImage(pageId, temp);
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								href =  WebContext.Root + "/" +  temp;
							break;
						case ImageType.Large:
							temp = PagesManager.GetLargeImage(pageId, temp);
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								href =  WebContext.Root + "/" +  temp;
							break;
						default:
							temp = PagesManager.GetImage(pageId, temp);
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								href =  WebContext.Root + "/" +  temp;
							break;
					}
				}

				if (!String.IsNullOrWhiteSpace(href))
				{
					this.Attributes["href"] = href;
				}

				this.Attributes["title"] = alt != null ? StringUtils.StripOutHtmlTags(alt.ToString()) : "";
				base.DataBind();
			}

		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);
		}

		ImageType imageType = ImageType.Large;
		public ImageType NewsImageType
		{
			get { return imageType; }
			set { imageType = value; }
		}
	}
}
