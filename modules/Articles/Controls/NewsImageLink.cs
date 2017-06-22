using System;
using System.Data;
using System.Web.UI;

using lw.ImageControls;
using lw.Utils;
using lw.WebTools;
using lw.DataControls;

namespace lw.Articles.Controls
{
	/// <summary>
	/// Links to the album's image
	/// </summary>
	public class NewsImageLink  : lw.Base.BaseControl
	{
		bool _bound = false;
		object DataObj;
		object alt;
		ImageType imageType = ImageType.Large;
		string href = "";
		object image;


		public NewsImageLink():base("a")
		{
			RenderContainerTag = true;
		}

		void bind()
		{
			if (_bound)
				return;

			DataObj = ControlUtils.GetBoundedDataField(this.NamingContainer, "NewsId");
			alt = ControlUtils.GetBoundedDataField(this.NamingContainer, "Title");
			image = ControlUtils.GetBoundedDataField(this.NamingContainer, "LargeImage");

			_bound = DataObj != null;
		}

		public override void DataBind()
		{
			bind();
			if (_bound)
			{
				string temp = "";
                string path = WebContext.Root + CTE.Folders.NewsImages;
                //string path = WebContext.Root + "/" + CTE.Folders.NewsImages;

                switch (NewsImageType)
				{
					case ImageType.Thumb:
					case ImageType.Medium:
						if (image != System.DBNull.Value)
						{
							temp = (string)image;
							if (!StringUtils.IsNullOrWhiteSpace(temp))
                                href = string.Format("{0}/News{1}/{2}", path, (int)DataObj, temp);
						}
						break;
					case ImageType.Large:
						if (image != System.DBNull.Value)
						{
							temp = (string)image;
							if (!StringUtils.IsNullOrWhiteSpace(temp))
                                href = string.Format("{0}/News{1}/{2}", path, (int)DataObj, temp);
						}
						break;
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
		
		public ImageType NewsImageType
		{
			get { return imageType; }
			set { imageType = value; }
		}
	}
}
