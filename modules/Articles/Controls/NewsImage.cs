using System;
using System.Data;
using System.Web.UI;
using lw.ImageControls;
using lw.Utils;
using lw.WebTools;

namespace lw.Articles.Controls
{
	public class NewsImageThumb : NewsImage  
	{
	}
	public class NewsImage  : ImageResizer
	{
		bool _bound = false;
		object DataObj;
		string noImage = "";
		object alt;
		ImageType imageType = ImageType.Resize;
		bool lazyLoad = false;

		public NewsImage()
		{
		}

		void bind()
		{
			if (_bound)
				return;

			DataObj = ControlUtils.GetBoundedDataField(this.NamingContainer, "NewsId");
			alt = ControlUtils.GetBoundedDataField(this.NamingContainer, "Title");

			_bound = DataObj != null;
		}
		public override void DataBind()
		{
			bind();
			if (_bound)
			{
				NewsManager nMgr = new NewsManager();

				DataRow news = nMgr.GetNews("NewsId=" + DataObj.ToString())[0].Row;
				string temp = "";
				switch (NewsImageType)
				{
					case ImageType.Thumb:
						temp = NewsManager.GetThumbImage(news);
						if(!StringUtils.IsNullOrWhiteSpace(temp))
                            this.Src = ResolveUrl(WebContext.StartDir + temp);
						if (this.Src != "")
							NoResize = true;
						break;
					case ImageType.Medium:
						temp = NewsManager.GetMediumImage(news);
						if (!StringUtils.IsNullOrWhiteSpace(temp))
                            this.Src = ResolveUrl(WebContext.StartDir + temp);
						if (this.Src != "")
							NoResize = true;
						break;
					case ImageType.Large:
					case ImageType.Crop:
						Crop = true;
						goto default;
					default:
						temp = NewsManager.GetLargeImage(news);
						if (!StringUtils.IsNullOrWhiteSpace(temp))
							this.Src = ResolveUrl(WebContext.StartDir + temp);
						break;
				}
				if (String.IsNullOrEmpty(this.Src))
				{
					if (String.IsNullOrEmpty(NoImage))
					{
						Visible = false;
						return;
					}
					else
					{
						NoResize = true;
						this.Src = ResolveUrl(NoImage);
					}
				}

				if (LazyLoad)
				{
					this.Attributes.Add("data-image", this.Src);
					this.CssClass += " lazy-load";
					this.Src = null;
				}

				this.Attributes["alt"] = alt != null ? StringUtils.StripOutHtmlTags(alt.ToString()) : "";
				base.DataBind();
			}

		}

		protected override void Render(HtmlTextWriter output)
		{
			if(Visible)
				base.Render(output);
		}

		#region Properties

		/// <summary>
		/// Delays the loading of the image.
		/// For this to work lazy load must be included in lw.js 
		/// Included as of version 4.0.4.5, developed with the Creative Newsletter
		/// </summary>
		public bool LazyLoad
		{
			get
			{
				return lazyLoad;
			}
			set
			{
				lazyLoad = value;
			}
		}
	
		/// <summary>
		/// Display this image if no image is related to the news
		/// </summary>
		public string NoImage
		{
			get
			{
				if (String.IsNullOrWhiteSpace(noImage))
				{
					noImage = Config.GetFromWebConfig(lw.CTE.parameters.ImageNotFound);
				}
				return noImage;
			}
			set
			{
				noImage = value;
			}
		}
		public ImageType NewsImageType
		{
			get { return imageType; }
			set { imageType = value; }
		}
		#endregion
	}
}
