using System;
using System.Data;
using System.Web.UI;
using lw.ImageControls;
using lw.Utils;
using lw.WebTools;

using lw.Content.Controls;
using lw.DataControls;

namespace lw.PhotoAlbums.Controls
{
	public class PhotoAlbumHasImage  : DisplayContainer
	{
		bool _bound = false;
		object DataObj;
		object alt;
		object image;

		public PhotoAlbumHasImage()
		{
			
		}

		void bind()
		{
			if (_bound)
				return;

			DataObj = ControlUtils.GetBoundedDataField(this.NamingContainer, "Id");
			image = ControlUtils.GetBoundedDataField(this.NamingContainer, "Image");

			_bound = DataObj != null;
		}

		public override void DataBind()
		{
			bind();
			if (_bound)
			{
				Display = image != System.DBNull.Value && !String.IsNullOrWhiteSpace(image.ToString());
				
				base.DataBind();
			}

		}
	}
}
