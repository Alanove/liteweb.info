using System;
using System.Web.UI;
using lw.ImageControls;
using lw.Utils;
using lw.WebTools;

namespace lw.VideoGallery.Controls
{
	public class VideoImage : ImageResizer
	{
		bool _bound = false;
		object DataObj;
		string noImage = "";
		object alt;
		ImageType imageType = ImageType.Resize;
		string videoThumb = "";

		public VideoImage()
		{ 
		}

		void bind()
		{
			if (_bound)
				return;

			DataObj = DataBinder.Eval(this.NamingContainer, "DataItem");
			alt = ControlUtils.GetBoundedDataField(this.NamingContainer, "Title");
			videoThumb = ControlUtils.GetBoundedDataField(this.NamingContainer, "ThumbImage").ToString();

			_bound = DataObj != null;
		}
		public override void DataBind()
		{
			bind();
			if (_bound)
			{
				string temp = "";
				switch (VideoImageType)
				{
					case ImageType.Thumb:
					default:
						temp = MediaGalleryManager.GetThumbImage(videoThumb);
						if(!StringUtils.IsNullOrWhiteSpace(temp))
							this.Src = WebContext.Root + "/" + temp;
						if (this.Src != "")
							NoResize = true;
						break;
				}
				if (String.IsNullOrEmpty(this.Src))
				{
					if (NoImage == "")
					{
						Visible = false;
						return;
					}
					else
						this.Src = ResolveUrl(NoImage);
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
		public ImageType VideoImageType
		{
			get { return imageType; }
			set { imageType = value; }
		}
	}
}
