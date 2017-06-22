using System.Web.UI;
using lw.CTE.Enum;
using lw.Members;
using lw.Members.Security;
using lw.Utils;

namespace lw.Products.Controls
{
	public class ItemPricesSrc : System.Web.UI.WebControls.Label
	{
		string format = "Price: {0:c}";
		PriceCategory _price = PriceCategory.Price;
		bool _formatSet = false;
		decimal _priceValue = 0;
		bool bound;

		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			this.Visible = false;

			UserStatus status = UserStatus.Unknown;
			if (User.LoggedIn)
			{
				status = MembersManager.GetUserStatus(User.LoggedInUser(this));
			}
			if (Price == PriceCategory.ResellerPrice && status != UserStatus.Reseller)
			{
				this.Visible = false;
				return;
			}
			object obj = DataBinder.Eval(this.NamingContainer, "DataItem." + Price.ToString());

			if (obj != null && obj != System.DBNull.Value)
			{
				int stat = (int)DataBinder.Eval(this.NamingContainer, "DataItem.Status");
				if(!Tools.CheckStatus(ItemStatus.OnSale, stat) && Price == PriceCategory.SalePrice)
					return;
				_priceValue = (decimal)obj;
				
				this.Text = string.Format(Format, obj);
				this.Visible = true;

				if (Price == PriceCategory.Price)
				{
					if (Tools.CheckStatus(ItemStatus.OnSale, stat))
					{
						this.CssClass += " strike";
					}
					else
					{
						if (status == UserStatus.Reseller)
						{
							this.CssClass += " strike";
						}
					}
				}
			}
			base.DataBind();
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!bound)
				DataBind();
			if (_priceValue == 0)
				return;

			base.Render(writer);
		}

		public PriceCategory Price
		{
			get { return _price; }
			set { _price = value; }
		}
		public string Format
		{
			get
			{
				if (!_formatSet)
				{
					_formatSet = true;
					format = EnumHelper.GetDescription(Price) + ": {0:c}";
				}
				return format;
			}
			set
			{
				_formatSet = true;
				format = value;
			}
		}
	}
	
}