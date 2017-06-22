using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.UI;



namespace lw.ShoppingCart.Controls
{
	public class ShoppingCartDetail: LiteralControl
	{
		string format = "{0}";
		ShoppingCartDetailsItems detail = ShoppingCartDetailsItems.Total;
		bool bound = false;

		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			lw.ShoppingCart.ShoppingCart cart = new lw.ShoppingCart.ShoppingCart();
			
			Type classType = cart.ShoppingItems.GetType();

			PropertyInfo[] pi = classType.GetProperties();

			HybridDictionary dic = new HybridDictionary();

			foreach (PropertyInfo _pi in pi)
			{
				if (_pi.CanRead)
				{
					dic.Add(_pi.Name, _pi.GetValue(cart.ShoppingItems, null));
				}
			}

			if (Detail == ShoppingCartDetailsItems.GiftVoucherValue && cart.ShoppingItems.GiftVoucherValue == 0)
			{
				this.Visible = false;
				return;
			}
			if (detail == ShoppingCartDetailsItems.Discount && cart.ShoppingItems.Discount == 0)
			{
				this.Visible = false;
				return;
			}
			if (cart.ShoppingItems.Payment != null && detail == ShoppingCartDetailsItems.PaymentCost && cart.ShoppingItems.Payment.AdditionalCost == 0)
			{
				this.Visible = false;
				return;
			}
			if (cart.ShoppingItems.Payment != null && detail == ShoppingCartDetailsItems.PaymentType && cart.ShoppingItems.Payment.AdditionalCost == 0)
			{
				this.Visible = false;
				return;
			}
			if (dic[Detail.ToString()] != null)
			{
				if (Detail == ShoppingCartDetailsItems.PaymentType)
					this.Text = string.Format(Format, cart.ShoppingItems.Payment.DisplayName);
				else
					this.Text = string.Format(Format, dic[Detail.ToString()]);
			}
			base.DataBind();
		}

		#region Properties
		public string Format
		{
			get { return format; }
			set { format = value; }
		}
		public ShoppingCartDetailsItems Detail
		{
			get { return detail; }
			set { detail = value; }
		}
		#endregion
	}
}
