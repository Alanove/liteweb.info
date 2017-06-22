using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;

using lw.DataControls;
using System.Data;
using lw.WebTools;

namespace lw.Pages.Controls
{
	public class NextPageLink:PageLink
	{
		bool _bound = false;

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;


			CustomDataSource dataSrc = null;

			if (!String.IsNullOrWhiteSpace(source))
			{
				Control ctrl = Page.FindControl(source);
				if (ctrl != null)
					dataSrc = ctrl as CustomDataSource;
			}
			else
			{
				dataSrc = this.Parent as CustomDataSource;
			}

			if (dataSrc != null)
			{
				DataTable ds = dataSrc.Data as DataTable;
				Visible = false;
				if (ds != null && dataSrc.HasData)
				{
					int pageId = (int)ControlUtils.GetBoundedDataField(this.NamingContainer, "PageId");

					for (int i =  0; i < ds.Rows.Count; i++)
					{
						DataRow p = ds.Rows[i];

						if ((int)p["PageId"] == pageId && i < ds.Rows.Count - 1)
						{
							Visible = true;

							p = ds.Rows[i + 1];

							this.Title = p["Title"].ToString();
							this.FullURL = p["FullURL"].ToString();
							this.PageId = (int)p["PageId"];

							break;
						}
					}
				}
			}
			base.DataBind();
		}

		string source = "";
		public string Source
		{
			get
			{
				return Source;
			}
			set
			{
				source = value;
			}
		}
	}
}
