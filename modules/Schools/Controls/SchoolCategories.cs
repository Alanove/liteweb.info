using System;
using System.Text;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using lw.Products;
using lw.Data;

namespace SABIS.Controls
{
	public class SchoolCategories : Repeater
	{
		bool bound = false;
		string _Filter = "";
		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			CategoriesMgr pMgr = new CategoriesMgr();
			DataView regions = pMgr._GetCategories(Filter).DefaultView;
			regions.Sort = "SortingOrder asc, Title";
			this.DataSource = regions;

			base.DataBind();
		}
		public string Filter
		{
			get
			{
				return _Filter;
			}
			set
			{
				_Filter = value;
			}
		}
	}
}
