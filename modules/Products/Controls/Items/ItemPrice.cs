using System.Data;
using System.Web.UI;
using lw.Base;
using lw.CTE.Enum;
using lw.Data;
using lw.Members;
using lw.Members.Security;
using lw.Utils;

namespace lw.Products.Controls
{
	public class ItemPrice : System.Web.UI.WebControls.Label
	{
		string format = "Price: {0:c}";
		PriceCategory _price = PriceCategory.Price;
		bool _formatSet = false;
		decimal _priceValue = 0;
		bool bound;

		CustomPage _thisPage = null;
		CustomPage thisPage
		{
			get
			{
				if (_thisPage == null)
					_thisPage = this.Page as CustomPage;
				return _thisPage;
			}
		}

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


			DataRowView price = GetDefaultPrice();

			bool UseMultiplePriceForItems = true;
			string temp = lw.WebTools.XmlManager.GetFromWebConfig(lw.CTE.parameters.UseMultiplePriceForItems);
			if (!string.IsNullOrWhiteSpace(temp))
			{
				UseMultiplePriceForItems = bool.Parse(temp);
			}

			try
			{

				if (price == null || !UseMultiplePriceForItems)
				{
					object obj = DataBinder.Eval(this.NamingContainer, "DataItem." + Price.ToString());

					if (obj != null && obj != System.DBNull.Value)
					{
						int stat = (int)DataBinder.Eval(this.NamingContainer, "DataItem.Status");
						if (!Tools.CheckStatus(ItemStatus.OnSale, stat) && Price == PriceCategory.SalePrice)
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
				}
				else
				{
					object obj = price["Price"];

					if (obj != null && obj != System.DBNull.Value)
					{
						int stat = (int)DataBinder.Eval(this.NamingContainer, "DataItem.Status");
						if (!Tools.CheckStatus(ItemStatus.OnSale, stat) && Price == PriceCategory.SalePrice)
							return;
						_priceValue = (decimal)obj;

						this.Text = string.Format(Format, obj) + "/" + price["PriceFor"].ToString();
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
				}
			}
			catch (System.Exception ex)
			{
				lw.WebTools.ErrorContext.Add(ex);
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

		DataView GetItemPrices()
		{
			DataView ret = null;

			int ItemId = (int)DataBinder.Eval(this.NamingContainer, "DataItem.ItemId");

			DataTable dt = null;

			if (thisPage.PageContext["ItemPrices"] != null)
			{
				dt = (DataTable)thisPage.PageContext["ItemPrices"];
			}
			else
			{
				string sql = "select * from ItemPrices";
				dt = DBUtils.GetDataSet(sql, cte.lib).Tables[0];
				thisPage.PageContext["ItemPrices"] = dt;
			}

			ret = new DataView(dt, "ItemId=" + ItemId.ToString(), "IsDefault", DataViewRowState.CurrentRows);

			return ret;
		}
		DataRowView GetDefaultPrice()
		{
			int ItemId = (int)DataBinder.Eval(this.NamingContainer, "DataItem.ItemId");
			DataView dv = GetItemPrices();
			dv.RowFilter = "IsDefault=1 And ItemId=" + ItemId.ToString();
			if (dv.Count > 0)
				return dv[0];
			else
				dv.RowFilter = "";

			return dv.Count > 0 ? dv[0] : null;
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