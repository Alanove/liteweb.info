using System.Web;

using lw.WebTools;

namespace lw.ShoppingCart.Handlers
{
	public class ShoppingCartUpdate : IHttpHandler
	{
		public ShoppingCartUpdate()
		{
		}

		public void ProcessRequest(HttpContext context)
		{
			lw.ShoppingCart.ShoppingCart cart = new lw.ShoppingCart.ShoppingCart();
			CartUpdateStatus status = cart.Update(context.Request, context.Request["GiftVoucher"]);
			if (status == CartUpdateStatus.IncorrectGiftVoucher)
				WebContext.Response.Redirect(WebContext.Root + "/shopping-cart/?status=" + status.ToString() + "&v=" + context.Request["GiftVoucher"]);
			else
				WebContext.Response.Redirect(WebContext.Root + "/shopping-cart/");
		}

		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
	}
}
