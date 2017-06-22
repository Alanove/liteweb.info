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
    public class NetworkImage : ImageResizer
    {
        bool _bound = false;
        object DataObj;
        string noImage = "";
        object alt;
        object image;
        ImageType imageType = ImageType.Large;

        public NetworkImage()
        {
            NoResize = true;
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

                switch (NetworkImageType)
                {
                    case ImageType.Thumb:
                        if (image != System.DBNull.Value)
                        {
							temp = (string)image;
                            if (!StringUtils.IsNullOrWhiteSpace(temp))
                                this.Src = string.Format("{0}/Network{1}/Thumb/{2}", path, (int)DataObj, temp);
                        }
                        if (this.Src != "")
                            NoResize = true;
                        break;
					case ImageType.Medium:
						if (image != System.DBNull.Value)
						{
							temp = (string)image;
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								this.Src = string.Format("{0}/Network{1}/Medium/{2}", path, (int)DataObj, temp);
						}
						if (this.Src != "")
							NoResize = true;
						break;
                    case ImageType.Large:
                        if (image != System.DBNull.Value)
                        {
							temp = (string)image;
                            if (!StringUtils.IsNullOrWhiteSpace(temp))
                                this.Src = string.Format("{0}/Network{1}/Large/{2}", path, (int)DataObj, temp);
                        }
                        if (this.Src != "")
                            NoResize = true;
                        break;
					case ImageType.Original:
						if (image != System.DBNull.Value)
						{
							temp = (string)image;
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								this.Src = string.Format("{0}/Network{1}/{2}", path, (int)DataObj, temp);
						}
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
            if (Visible)
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
        public ImageType NetworkImageType
        {
            get { return imageType; }
            set { imageType = value; }
        }
    }
}
