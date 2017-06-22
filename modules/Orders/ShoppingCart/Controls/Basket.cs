using System.Data;


namespace lw.ShoppingCart.Controls
{
	public class Basket : lw.DataControls.CustomRepeater
	{
		bool bound = false;
		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;
			lw.ShoppingCart.ShoppingCart sCart = new lw.ShoppingCart.ShoppingCart();
			this.DataSource = new DataView(sCart.ShoppingItems.BasketItems);
			base.DataBind();
		}
	}
}
