
using lw.Utils;

namespace lw.Orders
{
	/// <summary>
	/// Class containing all the constants that are using in the <see cref="Orders"/> library
	/// </summary>
	public class cte
	{
		public const string lib = "OrdersManager";
	}

	public enum OrderType:short
	{
		[Description("Gift Voucher")]
		GiftVoucher = 1,

		Products = 2
	}
	public enum OrderStatus
	{
		Pending = 1,
		
		Verifying = 2,

		[Description("Ready For Shipping")]
		ReadyForShipping = 3,

		Shipped = 4,

		Canceled = 5
	}
}