using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lw.DataControls;
using lw.PhotoAlbums;
using lw.WebTools;
using System.Data;

namespace lw.Pages.Controls
{
	public class PageRelatedAlbum : DataProvider
	{

		public override void DataBind()
		{
			PhotoAlbumsManager pMgr = new PhotoAlbumsManager();
			DataRow dr = pMgr.GetAlbumDetails(AlbumName);
			if (dr != null)
			{
				this.DataItem = pMgr.GetAlbumDetails(AlbumName);

				base.DataBind();
			}
		}

		#region properties
		string _albumName = null;
		public string AlbumName
		{
			get
			{
				if (_albumName == null)
				{
					int pageId = (int)ControlUtils.GetBoundedDataField(this.NamingContainer, "PageId");
					string url = ControlUtils.GetBoundedDataField(this.NamingContainer, "URL").ToString();

					_albumName = string.Format("{0}-{1}", url, pageId);
				}
				return _albumName;
			}
			set
			{
				_albumName = value;
			}
		}
		#endregion
	}
}
