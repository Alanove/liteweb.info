using System.Web.UI;
using lw.Content.Controls;
using lw.Products;
using lw.DataControls;
using lw.WebTools;

namespace lw.ShoppingCart.Controls
{
	public class HasCart : DisplayContainer
	{
		bool _forSale = false, _bound = false;
		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			object obj = ControlUtils.GetBoundedDataField(this.NamingContainer, "Status");

			if (obj != null)
			{
				if (Tools.CheckStatus(ItemStatus.ForSale, (int)obj))
				{
					obj = ControlUtils.GetBoundedDataField(this.NamingContainer, "Price");
					if (obj != null && obj != System.DBNull.Value)
					{
						if ((decimal)obj > 0)
						{
							obj = ControlUtils.GetBoundedDataField(this.NamingContainer, "Inventory");
							if (double.Parse(obj.ToString()) > 0)
								_forSale = true;
						}
					}
				}
			}


			this.Display = _forSale;
			base.DataBind();
		}

	}
}
