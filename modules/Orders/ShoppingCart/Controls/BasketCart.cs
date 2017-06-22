using System.Web.UI;


namespace lw.ShoppingCart.Controls
{
	
	[System.ComponentModel.DefaultProperty("method"),
		ToolboxData("<{0}:Cart runat=server></{0}:Cart>")]
	[ParseChildren(ChildrenAsProperties = false)]
	public class BasketCart : System.Web.UI.WebControls.WebControl
	{
		public BasketCart()
			: base("form")
		{
			this.Attributes["method"] = "post";
			this.Attributes["Action"] = "ShoppingCartUpdate.axd";
		}
	}
}
