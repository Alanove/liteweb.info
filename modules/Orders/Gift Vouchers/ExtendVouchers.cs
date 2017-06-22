
namespace lw.Orders
{
	public class VouchersAdp : OrdersDSTableAdapters.GiftVouchersTableAdapter
	{
		public OrdersDS.GiftVouchersDataTable GetVouchers(string cond)
		{
			if (cond != "")
				cond = " where " + cond;
			base.CommandCollection[0].CommandText += cond;
			return base.GetData();
		}
	}

	public class GiftVoucherOrdersAdp : OrdersDSTableAdapters.GiftVoucherOrdersTableAdapter
	{
		public OrdersDS.GiftVoucherOrdersDataTable GetVouchers(string cond)
		{
			if (cond != "")
				cond = " where " + cond;
			base.CommandCollection[0].CommandText += cond;
			return base.GetData();
		}
	}
}
