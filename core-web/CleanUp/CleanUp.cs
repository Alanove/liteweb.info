using System;
using lw.Data;
using lw.Orders;

namespace lw.CleanUp
{
	public class DBCleanUp
	{
		public static void Clean()
		{
			//CleanUnverifiedOrders();
			CleanProfiles();
		}
		public static void CleanProfiles()
		{
			DBUtils.ExecuteQuery(cte.spCleanUpProfiles + "\"10\"", cte.lib);
		}
		public static void CleanUnverifiedOrders()
		{
			OrdersManager oMgr = new OrdersManager();

			string cond = string.Format("DateCreated <'{0}' and Status={1} and PaymentType <>'{2}' ",
				DateTime.Now.AddMinutes(-20), (int)OrderStatus.Verifying, Payments.PaymentTypes.PO.ToString());

			OrdersDS.OrdersDataTable orders = oMgr.GetOrders(cond);
			foreach (OrdersDS.OrdersRow order in orders.Rows)
				CleanUpOrder(order.OrderVisualId);
		}
		public static void CleanUpOrder(string OrderId)
		{
			OrdersManager oMgr = new OrdersManager();
			oMgr.ConfirmOrder(OrderId, false, "Timeout");
		}
	}
}