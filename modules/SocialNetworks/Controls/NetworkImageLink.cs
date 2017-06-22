using System;
using System.Data;
using System.Web.UI;
using System.IO;

using lw.ImageControls;
using lw.Utils;
using lw.WebTools;
using lw.DataControls;

namespace lw.Networking.Controls
{
	/// <summary>
	/// Links to the network's image
	/// </summary>
	public class NetworkImageLink  : lw.Base.BaseControl
	{
		bool _bound = false;
		object DataObj;
		object alt;
		ImageType imageType = ImageType.Large;
		string href = "";
		object image;


		public NetworkImageLink():base("a")
		{
			RenderContainerTag = true;
		}

		void bind()
		{
			if (_bound)
				return;

			DataObj = ControlUtils.GetBoundedDataField(this.NamingContainer, "NetworkId");
			alt = ControlUtils.GetBoundedDataField(this.NamingContainer, "Name");
            image = ControlUtils.GetBoundedDataField(this.NamingContainer, "Image");

			_bound = DataObj != null;
		}

		public override void DataBind()
		{
			bind();
			if (_bound)
			{
				string temp = "";
				string path = WebContext.Root + "/" + CTE.Folders.Networks;

				switch (NetworkTypeImage)
				{
					case ImageType.Thumb:
						if (image != System.DBNull.Value)
						{
							temp = (string)image;
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								href = string.Format("{0}/Network{1}/Thumb/{2}", path, (int)DataObj, temp);
						}
						break;
					case ImageType.Medium:
						if (image != System.DBNull.Value)
						{
							temp = (string)image;
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								href = string.Format("{0}/Network{1}/Medium/{2}", path, (int)DataObj, temp);
						}
						break;
                    case ImageType.Large:
                        if (image != System.DBNull.Value)
                        {
							temp = (string)image; 
                            if (!StringUtils.IsNullOrWhiteSpace(temp))
                                href = string.Format("{0}/Network{1}/Large/{2}", path, (int)DataObj, temp);
                        }
                        break;
					case ImageType.Original:
						if (image != System.DBNull.Value)
						{
							temp = (string)image;
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								href = string.Format("{0}/Network{1}/{2}", path, (int)DataObj, temp);
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

		public ImageType NetworkTypeImage
		{
			get { return imageType; }
			set { imageType = value; }
		}
	}
}
