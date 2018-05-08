using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using lw.CTE;
using lw.DataControls;
using lw.Data;
using lw.WebTools;

namespace lw.Pages.Controls
{
	public class PageImagesDataSource : CustomDataSource
	{
		bool _bound = false;
		EmptyDataSrc _dataSrc = null;

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;


			_dataSrc = new EmptyDataSrc();

			PagesManager pMgr = new PagesManager();

			if (PageId != null)
			{
				var images = pMgr.GetPageImages(PageId.Value);

				this.Data = images;

				_dataSrc.RowsCount = images.Count();
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
		int? pageId;
		public int? PageId
		{
			get
			{
				if (pageId == null)
				{
					object obj = ControlUtils.GetBoundedDataField(this.NamingContainer, "PageId");

					if (obj == null)
					{
						obj = MyPage.GetQueryValue(RoutingParameters.PageId);
					}
					if (obj == null)
					{
						obj = MyPage.GetQueryValue("PageId");
					}
				
					if (obj != null)
						pageId = int.Parse(obj.ToString());
				}
				return pageId;
			}
			set
			{
				pageId = value;
			}
		}
#endregion
	}
}
