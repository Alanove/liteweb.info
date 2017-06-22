using System.Data;
using lw.DataControls;
using lw.WebTools;

namespace lw.Orders.Controls
{
	public class OrdersSource : CustomDataSource
	{
		string _cond = "";
		DataTable dataTable;

		public OrdersSource()
		{
			this.DataLibrary = lw.Orders.cte.lib;
			this.SelectCommand = string.Format("Select * From Orders where MemberId={0}",
				WebContext.Profile.UserId);
			this.OrderBy = "DateCreated Desc";
		}


		public override void DataBind()
		{
			base.DataBind();
		}
	}
}
