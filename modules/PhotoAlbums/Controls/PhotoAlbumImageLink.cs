using System;
using System.Data;
using System.Web.UI;

using lw.ImageControls;
using lw.Utils;
using lw.WebTools;
using lw.DataControls;

namespace lw.PhotoAlbums.Controls
{
	/// <summary>
	/// Links to the album's image
	/// </summary>
	public class PhotoAlbumImageLink  : lw.Base.BaseControl
	{
		bool _bound = false;
		object DataObj;
		object alt;
		ImageType imageType = ImageType.Large;
		string href = "";
		object image;


		public PhotoAlbumImageLink():base("a")
		{
			RenderContainerTag = true;
		}

		void bind()
		{
			if (_bound)
				return;

			DataObj = ControlUtils.GetBoundedDataField(this.NamingContainer, "Id");
			alt = ControlUtils.GetBoundedDataField(this.NamingContainer, "DisplayName");
			image = ControlUtils.GetBoundedDataField(this.NamingContainer, "Image");

			_bound = DataObj != null;
		}

		public override void DataBind()
		{
			bind();
			if (_bound)
			{
				string temp = "";
				string path = WebContext.Root + "/" + CTE.Folders.PhotoAlbums;

				switch (AlbumImageType)
				{
					case ImageType.Thumb:
					case ImageType.Medium:
						if (image != System.DBNull.Value)
						{
							temp = (string)image;
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								href = string.Format("{0}/Album{1}/Thumb_{2}", path, (int)DataObj, temp);
						}
						break;
					case ImageType.Large:
						if (image != System.DBNull.Value)
						{
							temp = (string)image;
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								href = string.Format("{0}/Album{1}/Large_{2}", path, (int)DataObj, temp);
						}
						break;
					case ImageType.Original:
						if (image != System.DBNull.Value)
						{
							temp = (string)image;
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								href = string.Format("{0}/Album{1}/{2}", path, (int)DataObj, temp);
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
		
		public ImageType AlbumImageType
		{
			get { return imageType; }
			set { imageType = value; }
		}
	}
}
