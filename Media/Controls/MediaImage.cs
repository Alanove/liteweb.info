using System;
using System.Data;
using System.Web.UI;
using lw.Utils;
using lw.WebTools;
using lw.DataControls;

using lw.CTE.Enum;
using lw.Widgets;
using lw.ImageControls;
using System.IO;

namespace lw.Widgets.Controls
{
	
	public class MediaImage : System.Web.UI.HtmlControls.HtmlImage
	{
		bool _bound = false;
		object DataObj;
		string noImage = "";
		object src = null;
		object alt;
		object dateAdded;
		string folderDate;
		DefaultMediaTypes _type = DefaultMediaTypes.NULL;
		bool lazyLoad = false;
		string _forcedSrc = "";
		
		public MediaImage()
		{
		}

		void bind()
		{
			if (_bound)
				return;

			dateAdded = (DateTime)ControlUtils.GetBoundedDataField(this.NamingContainer, "DateAdded");
			folderDate = string.Format("{0:MM-yyyy}", dateAdded);
			alt = ControlUtils.GetBoundedDataField(this.NamingContainer, "Caption");
			src = ControlUtils.GetBoundedDataField(this.NamingContainer, "MediaFile");
			if ((int)_type == 0)
				_type = (DefaultMediaTypes)Enum.Parse(typeof(DefaultMediaTypes), EnumHelper.GetEnumTypeName<DefaultMediaTypes>(Int32.Parse(ControlUtils.GetBoundedDataField(this.NamingContainer, "Type").ToString())));

			_bound = true;
		}
		public override void DataBind() 
		{
			bind();
			if (_bound)
			{
				var root = WebContext.Root == "" ? "" : WebContext.Root + "/";
				if (!String.IsNullOrWhiteSpace(ForcedSrc))
				{
					this.Src = root + ForcedSrc;
				}
				else if (src != null && src != DBNull.Value && !String.IsNullOrWhiteSpace(src.ToString()))
				{
					string s = src.ToString();
					string temp = s;
					string ext = Path.GetExtension(s);
					switch (ImageSize)
					{
						case ImageType.Thumb:
							temp = MediaManager.GetFilePath(Type, folderDate);
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								this.Src = root + temp + "/" + s.Replace(ext, "-t" + ext);
							break;
						case ImageType.Medium:
							temp = MediaManager.GetFilePath(Type, folderDate);
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								this.Src = root + temp + "/" + s.Replace(ext, "-m" + ext);
							break;
						case ImageType.Large:
							temp = MediaManager.GetFilePath(Type, folderDate);
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								this.Src = root + temp + "/" + s.Replace(ext, "-l" + ext);
							break;
						default:
							temp = MediaManager.GetFilePath(Type, folderDate);
							if (!StringUtils.IsNullOrWhiteSpace(temp))
								this.Src = root + temp + "/" + s;
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


		ImageType imageSize = ImageType.Medium;
		public ImageType ImageSize
		{
			get { return imageSize; }
			set { imageSize = value; }
		}

		public DefaultMediaTypes Type
		{
			get
			{ return _type; }
			set
			{ 
				_type = value;
			}
		}

		public string ForcedSrc
		{
			get { return _forcedSrc; }
			set { _forcedSrc = value; }
		}
		#endregion
	}
}
