using System.Web.UI;

using lw.Content.Controls;


namespace lw.Products.Controls
{
	public class OutOfStock : DisplayContainer
	{
		public override void DataBind()
		{
			Display = true;
			object obj = DataBinder.Eval(this.NamingContainer, "DataItem.Status");

			if (obj != null)
			{
				if (Tools.CheckStatus(ItemStatus.ForSale, (int)obj))
				{
					obj = DataBinder.Eval(this.NamingContainer, "DataItem.Price");
					if (obj != null)
					{
						if ((decimal)obj > 0)
						{
							obj = DataBinder.Eval(this.NamingContainer, "DataItem.Inventory");
							if (double.Parse(obj.ToString()) > 0)
								Display = false;
						}
					}
				}
			}

			base.DataBind();
		}
	}
}