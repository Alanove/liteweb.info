using System;
using System.Data;
using System.Web;
using lw.Products;
using lw.WebTools;

namespace lw.ShoppingCart.Handlers
{
	public class ShoppingCartAdd : IHttpHandler
	{
		public ShoppingCartAdd()
		{
		}

		public void ProcessRequest(HttpContext context)
		{
			System.Web.HttpRequest Request = context.Request;
			System.Web.HttpResponse Response = context.Response;



			int itemId = -1, qty = 1;

			if (Request.Form["ItemId"] != null)
			{
				try
				{
					itemId = Int32.Parse(Request.Form["ItemId"]);
				}
				catch
				{
				}
			}
			else
			{
				if (Request.QueryString["ItemId"] != null)
				{
					try
					{
						itemId = Int32.Parse(Request.QueryString["ItemId"]);
					}
					catch
					{
					}
				}
			}
			if (Request.Form["QTY"] != null)
			{
				try
				{
					qty = Int32.Parse(Request.Form["QTY"]);
				}
				catch
				{
				}
			}
			else
			{
				if (Request.QueryString["QTY"] != null)
				{
					try
					{
						qty = Int32.Parse(Request.QueryString["QTY"]);
					}
					catch
					{
					}
				}
			}

			

			lw.ShoppingCart.ShoppingCart sCart = new lw.ShoppingCart.ShoppingCart();
			sCart.Empty();
			string optionsKey = Request["Options"];
			optionsKey = String.IsNullOrEmpty(optionsKey)? "": optionsKey;

			ChoicesMgr cMgr = new ChoicesMgr();

			if (optionsKey == "")
			{
				DataTable dt = cMgr.GetItemOptions(itemId);
				if (dt.Rows.Count > 0)
				{
					Config cfg = new Config();
					ItemsMgr iMgr = new ItemsMgr();
					DataRow item = iMgr.GetItem(itemId);
					string ProductDetailsFolder = cfg.GetKey("ProductDetailsFolder");
					string red = string.Format("{2}/{1}/{0}.aspx", 
						item["UniqueName"],
						ProductDetailsFolder,
						WebContext.Root
					);
					Response.Redirect(red + "?err=" + lw.CTE.Errors.SelectItemOptions);
				}
			}

			sCart.AddItem(itemId, qty, optionsKey, "", "");
			Response.Redirect(WebContext.Root + "/shopping-cart/");
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
