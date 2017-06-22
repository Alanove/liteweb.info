using System;

using System.Text;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using lw.Products;
using lw.Data;
using lw.Utils;

namespace SABIS.Controls
{
	public class Schools : Repeater
	{
		bool bound = false;
		string region = "";

		public Schools()
		{
		}
		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			CategoriesMgr pMgr = new CategoriesMgr();

			string cond = "";
			Object obj = null;
			try
			{
				obj = DataBinder.Eval(this.NamingContainer, "DataItem.CategoryId");
			}
			catch
			{
			}
			if (obj == null)
			{
				if (region != "")
				{
					DataRow regions = pMgr.GetCategory(StringUtils.SQLEncode(region));
					if (regions != null)
						cond = string.Format("CategoryId={0}", regions["CategoryId"]);
				}
			}
			else
				cond = string.Format("CategoryId={0}", obj);

			string sql = "select * from ItemsView where " + cond;
			DataView schools = DBUtils.GetDataSet(sql, lw.Products.cte.lib).Tables[0].DefaultView;
			schools.Sort = "Brand, UserRating DESC, Ranking Asc, Title, ProductNumber";
			this.DataSource = schools;

			base.DataBind();
		}
		public string Region
		{
			get
			{
				return region;
			}
			set
			{
				region = value;
			}
		}
	}
}
