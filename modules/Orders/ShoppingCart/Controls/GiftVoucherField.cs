


namespace lw.ShoppingCart.Controls
{
	public class GiftVoucherField : System.Web.UI.HtmlControls.HtmlInputText
	{
		public override void DataBind()
		{
			lw.ShoppingCart.ShoppingCart cart = new lw.ShoppingCart.ShoppingCart();

			this.Value = cart.MaskVoucher();

			base.DataBind();
		}
		public override string UniqueID
		{
			get
			{
				return "GiftVoucher";
			}
		}
	}
}
