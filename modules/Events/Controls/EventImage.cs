using System;
using System.Data;
using System.Web.UI;
using lw.Utils;
using lw.WebTools;
using lw.DataControls;

using lw.CTE.Enum;
using lw.Events;
using lw.ImageControls;

namespace lw.Events.Controls
{
	
	public class EventImage : System.Web.UI.HtmlControls.HtmlImage
	{
		bool _bound = false;
		object DataObj;
		string noImage = "";
		object src = null;
		object alt;
		int eventId;
		bool lazyLoad = false;

		public EventImage()
		{
		}

		void bind()
		{
			if (_bound)
				return;

			eventId = (int)ControlUtils.GetBoundedDataField(this.NamingContainer, "Id");
			alt = ControlUtils.GetBoundedDataField(this.NamingContainer, "Title");
			src = ControlUtils.GetBoundedDataField(this.NamingContainer, "Image");

			_bound = true;
		}
		public override void DataBind() 
		{
			bind();
			if (_bound)
			{
				if (src != null && src != DBNull.Value && !String.IsNullOrWhiteSpace(src.ToString()))
				{
					string temp = src.ToString();
					switch (ImageType)
					{
						case ImageType.Thumb:
							temp = CalendarManager.GetThumbImage(eventId, temp);
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								this.Src = WebContext.Root + "/" + temp;
							break;
						case ImageType.Medium:
							temp = CalendarManager.GetMediumImage(eventId, temp);
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								this.Src = WebContext.Root + "/" + temp;
							break;
						case ImageType.Large:
							temp = CalendarManager.GetLargeImage(eventId, temp);
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								this.Src = WebContext.Root + "/" + temp;
							break;
						default:
							temp = CalendarManager.GetImage(eventId, temp);
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								this.Src = WebContext.Root + "/" + temp;
							break;
					}
				}
				else
				{
					if (!String.IsNullOrWhiteSpace(NoImage))
						this.Src = noImage;
					else
						this.Visible = false;
				}
				if (String.IsNullOrEmpty(this.Src))
				{
					if (String.IsNullOrEmpty(NoImage))
					{
						Visible = false;
						return;
					}
				}

				if (LazyLoad)
				{
					this.Attributes.Add("data-image", this.Src);
					this.Attributes["class"] += " lazy-load";
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
					if(!string.IsNullOrWhiteSpace(noImage))
						noImage = ResolveUrl(noImage);
				}
				return noImage;
			}
			set
			{
				noImage = value;
			}
		}


		ImageType imageType = ImageType.Medium;
		public ImageType ImageType
		{
			get { return imageType; }
			set { imageType = value; }
		}
		#endregion
	}
}
