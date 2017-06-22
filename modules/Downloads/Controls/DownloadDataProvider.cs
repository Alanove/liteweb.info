using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lw.CTE;
using lw.DataControls;
using lw.Downloads;

namespace lw.Downloads.Controls
{
	public class DownloadDataProvider : DataProvider
	{
		bool _bound = false;
		
		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			Downloads Ds = new Downloads();

			if (DownloadId != null)
			{
				this.DataItem = Ds.GetDownload(DownloadId.Value);
			}
			
			base.DataBind();
		}


#region Properties
		int? downloadId;
		public int? DownloadId
		{
			get
			{
				if (downloadId == null)
				{
					string obj = MyPage.GetQueryValue(RoutingParameters.DownloadId);
					if (string.IsNullOrWhiteSpace(obj))
					{
						obj = MyPage.GetQueryValue("Id");
					}
					if (!string.IsNullOrWhiteSpace(obj))
						downloadId = int.Parse(obj);
				}
				return downloadId;
			}
			set
			{
				downloadId = value;
			}
		}
#endregion
	}
}
