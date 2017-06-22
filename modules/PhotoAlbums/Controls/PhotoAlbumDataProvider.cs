using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lw.CTE;
using lw.DataControls;
using lw.Utils;
using lw.WebTools;
using System.Data;

namespace lw.PhotoAlbums.Controls
{
	public class PhotoAlbumDataProvider : DataProvider
	{
		bool _bound = false;
		bool _overridePageTitle = true;

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			PhotoAlbumsManager pMgr = new PhotoAlbumsManager();

			if (AlbumId != null)
			{
				this.DataItem = pMgr.GetAlbumDetails(AlbumId.Value);
			} else if (AlbumName != "")
			{
				this.DataItem = pMgr.GetAlbumDetails(AlbumName);
			}

			

			base.DataBind();

			if (OverridePageTitle)
			{
				Config cfg = new Config();

				if (MyPage != null)
				{
					DataRow album = this.DataItem as DataRow;

						MyPage.CustomTitle = string.Format("{0} - {1}",
							StringUtils.StripOutHtmlTags(album["DisplayName"].ToString()),
							cfg.GetKey("SiteName"));

						if (!String.IsNullOrEmpty(album["Description"].ToString()))
						MyPage.Description = string.Format("{0}",
							StringUtils.StripOutHtmlTags(album["Description"].ToString()));

				}
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
					string obj = MyPage.GetQueryValue(RoutingParameters.AlbumId);
					if (string.IsNullOrWhiteSpace(obj))
					{
						obj = MyPage.GetQueryValue("Id");
					}
					if (!string.IsNullOrWhiteSpace(obj))
						albumId = int.Parse(obj);
				}
				return albumId;
			}
			set
			{
				albumId = value;
			}
		}

		/// <summary>
		/// Used to get Albums by their Name
		/// </summary>
		string albumName;
		public string AlbumName
		{
			get
			{
				if (albumName == null)
				{
					string obj = MyPage.GetQueryValue(RoutingParameters.AlbumName);
					if (string.IsNullOrWhiteSpace(obj))
					{
						obj = MyPage.GetQueryValue("Name");
					}
					if (!string.IsNullOrWhiteSpace(obj))
						albumName = obj;
				}
				return albumName;
			}
			set
			{
				albumName = value;
			}
		}

		/// <summary>
		/// Flag to either override Page title from the news title or to leave it untouched
		/// </summary>
		public bool OverridePageTitle
		{
			get { return _overridePageTitle; }
			set { _overridePageTitle = value; }
		}
#endregion
	}
}
