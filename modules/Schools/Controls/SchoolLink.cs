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
	public class SchoolLink : HyperLink
	{
		bool bound;

		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			object obj = DataBinder.Eval(this.NamingContainer, "DataItem.Website");

			if (obj != null && obj != System.DBNull.Value)
			{
				string link = obj.ToString();
				if (link.IndexOf("no website") < 0)
					this.NavigateUrl = link;
			}

			//obj = DataBinder.Eval(this.NamingContainer, "DataItem.Title");
			//this.Text = obj.ToString();

			this.ToolTip = this.NavigateUrl;
			this.Target = "_blank";

			base.DataBind();
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!bound)
				DataBind();
			base.Render(writer);
		}
	}
}
