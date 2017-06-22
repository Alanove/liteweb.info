using System;

using lw.Utils;

namespace lw.Orders
{
	public class OrdersAdp : OrdersDSTableAdapters.OrdersTableAdapter
	{
		public OrdersDS.OrdersDataTable GetOrders(string condition)
		{
			if (condition != "")
				condition = " where " + condition;

			base.CommandCollection[0].CommandText += condition + " order by OrderId desc";
			OrdersDS.OrdersDataTable dt = base.GetData();

			//dt.Columns.Add("StatusDescription");
			//dt.Columns.Add("TypeDescription");

			foreach (OrdersDS.OrdersRow dr in dt.Rows)
			{
				OrderStatus stat = (OrderStatus)Enum.Parse(typeof(OrderStatus), dr["Status"].ToString());
				dr.StatusDescription = EnumHelper.GetDescription(stat);

				OrderType type = (OrderType)Enum.Parse(typeof(OrderType), dr["OrderType"].ToString());

				dr.TypeDescription = EnumHelper.GetDescription(type);
			}

			return dt;
		}
	}
}
