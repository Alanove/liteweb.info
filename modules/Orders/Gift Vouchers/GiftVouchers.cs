using System.Data;

using lw.Utils;

namespace lw.Orders
{
	public class GiftVouchers
	{
		public GiftVouchers()
		{
		}

		public DataTable GetGiftVouchers(string cond)
		{
			GiftVoucherOrdersAdp adp = new GiftVoucherOrdersAdp();
			return adp.GetVouchers(cond);
		}
		public OrdersDS.GiftVouchersRow GetGiftVoucherByCode(string code)
		{
			VouchersAdp adp = new VouchersAdp();
			DataTable dt = adp.GetVouchers("VoucherCode='" + StringUtils.SQLEncode(code) + "'");
			return dt.Rows.Count > 0 ? (OrdersDS.GiftVouchersRow)dt.Rows[0] : null;
		}
		public DataRow GetGiftVoucherByOrder(int orderId)
		{
			DataTable dt = GetGiftVouchers("OrderId=" + orderId.ToString());
			return dt.Rows.Count > 0 ? dt.Rows[0] : null;
		}

		public DataTable CreateVoucher(decimal amount)
		{
			OrdersDSTableAdapters.CreateGiftVoucherTableAdapter adp = new lw.Orders.OrdersDSTableAdapters.CreateGiftVoucherTableAdapter();
			OrdersDS.CreateGiftVoucherDataTable voucher = adp.GetData(amount);

			return voucher;
		}

		public void UpdateVoucher(int voucherId, string toName, string toEmail, int orderId)
		{
			OrdersDSTableAdapters.GiftVouchersTableAdapter adp = new lw.Orders.OrdersDSTableAdapters.GiftVouchersTableAdapter();
			adp.UpdateQuery(orderId, toEmail, toName, 0, voucherId);
		}
		public void UpdateVoucherStatus(string code, byte status)
		{
			OrdersDSTableAdapters.GiftVouchersTableAdapter adp = new lw.Orders.OrdersDSTableAdapters.GiftVouchersTableAdapter();
			adp.UpdateStatus(status, code);
		}
	}
}
