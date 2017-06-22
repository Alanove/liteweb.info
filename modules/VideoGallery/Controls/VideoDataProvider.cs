using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lw.CTE;
using lw.DataControls;
using lw.VideoGallery;
using lw.WebTools;
using lw.Utils;

namespace lw.VideoGallery.Controls
{
	public class VideoDataProvider : DataProvider
	{
		bool _bound = false;

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			MediaGalleryManager mgMgr = new MediaGalleryManager();


			string title = "";

			if (VideoId != null)
			{
				Video v = mgMgr.GetVideo(VideoId.Value);
				this.DataItem =v;
				title = v.Title;
			}
			else if (VideoName != "")
			{
				VideosView v =  mgMgr.GetVideoDetails(VideoName);
				this.DataItem = v;
				title = v.Title;
			}

			
			
			base.DataBind();

			if (!String.IsNullOrWhiteSpace(title))
			{
				if (MyPage != null)
				{
					Config cfg = new Config();
						MyPage.CustomTitle = string.Format("{0} Video - {1}",
							StringUtils.StripOutHtmlTags(title), cfg.GetKey("SiteName"));
				}
			}
		}


#region Properties
		int? videoId;
		public int? VideoId
		{
			get
			{
				if (videoId == null)
				{
					string obj = MyPage.GetQueryValue(RoutingParameters.VideoId);
					if (string.IsNullOrWhiteSpace(obj))
					{
						obj = MyPage.GetQueryValue("Id");
					}
					if (!string.IsNullOrWhiteSpace(obj))
						videoId = int.Parse(obj);
				}
				return videoId;
			}
			set
			{
				videoId = value;
			}
		}


		/// <summary>
		/// Used to get Vidos by their Name
		/// </summary>
		string videoName;
		public string VideoName
		{
			get
			{
				if (videoName == null)
				{
					string obj = MyPage.GetQueryValue(RoutingParameters.VideoName);
					if (string.IsNullOrWhiteSpace(obj))
					{
						obj = MyPage.GetQueryValue("UniqueName");
					}
					if (!string.IsNullOrWhiteSpace(obj))
						videoName = obj;
				}
				return videoName;
			}
			set
			{
				videoName = value;
			}
		}
#endregion
	}
}
