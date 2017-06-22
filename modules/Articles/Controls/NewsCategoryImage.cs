using System;
using System.Data;
using System.Web.UI;
using lw.Articles.LINQ;
using lw.ImageControls;
using lw.Utils;
using lw.WebTools;

namespace lw.Articles.Controls
{
	public class NewsCategoryImage  : ImageResizer
	{
		bool _bound = false;
		object DataObj;
		string noImage = "";
		object alt;
		ImageType imageType = ImageType.Resize;

		public NewsCategoryImage()
		{
		}

		void bind()
		{
			if (_bound)
				return;

			DataObj = DataBinder.Eval(this.NamingContainer, "DataItem.TypeId");
			alt = DataBinder.Eval(this.NamingContainer, "DataItem.Name");

			_bound = DataObj != null;
		}
		public override void DataBind()
		{
			bind();
			if (_bound)
			{
				NewsTypesManager nMgr = new NewsTypesManager();

				DataRow news = nMgr.GetNewsType((int)DataObj);
				string temp = "";
				switch (NewsImageType)
				{
					case ImageType.Thumb:
					default:
						temp = NewsTypesManager.GetThumbImage(news);
						if(!StringUtils.IsNullOrWhiteSpace(temp))
							this.Src = ResolveUrl(WebContext.StartDir + temp);
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
					{
						NoResize = true;
						this.Src = ResolveUrl(NoImage);
					}
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
		public ImageType NewsImageType
		{
			get { return imageType; }
			set { imageType = value; }
		}
	}
}
