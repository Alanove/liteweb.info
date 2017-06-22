using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using lw.CTE;
using lw.DataControls;
using lw.Data;
using lw.WebTools;

namespace lw.PhotoAlbums.Controls
{
	public class PhotoAlbumImagesDataSource : CustomDataSource
	{
		bool _bound = false;
		EmptyDataSrc _dataSrc = null;

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;


			_dataSrc = new EmptyDataSrc();

			PhotoAlbumsManager pMgr = new PhotoAlbumsManager();

			if (AlbumId != null)
			{
				DataView images = pMgr.GetImages(AlbumId.Value);

				this.Data = images;

				_dataSrc.RowsCount = images.Count;
				_dataSrc.Data = this.Data;
				_dataSrc.HasData = _dataSrc.RowsCount > 0;
			}
			
			base.DataBind();
		}

		public override Data.IDataSource DataSrc
		{
			get
			{
				return _dataSrc;
			}
			set
			{
				base.DataSrc = value;
			}
		}

#region Properties
		int? albumId;
		public int? AlbumId
		{
			get
			{
				if (albumId == null)
				{
					object obj = ControlUtils.GetBoundedDataField(this.NamingContainer, "Id");

					if (obj == null)
					{
						obj = MyPage.GetQueryValue(RoutingParameters.AlbumId);
					}
					if (obj == null)
					{
						obj = MyPage.GetQueryValue("Id");
					}
					if (obj != null)
						albumId = int.Parse(obj.ToString());
				}
				return albumId;
			}
			set
			{
				albumId = value;
			}
		}
#endregion
	}
}
