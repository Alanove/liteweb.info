using System;
using System.Data;
using System.Text;
using lw.CTE.Enum;
using lw.Orders;
using lw.Payments;
using lw.Products;
using lw.Shipping;
using lw.Utils;
using lw.WebTools;
using lw.Members;

namespace lw.ShoppingCart
{
	public class ShoppingCart
	{
		Basket _shoppingItems;

		decimal subTotal = 0;
		decimal total = 0;
		decimal discount = 0;
		decimal tax = 0;
		decimal shipping = 0;
		string giftVoucher = "";
		decimal giftVoucherValue = 0;
		string country = "", region = "";

		public ShoppingCart()
		{
		}
		public void AddItem(int ItemId, decimal qty, string optionsKey, string priceOption, string category)
		{
			ItemsMgr itemsMgr = new ItemsMgr();


			DataRow item = itemsMgr.GetItem(ItemId);
			string description = "";
			if (!String.IsNullOrWhiteSpace(optionsKey))
			{
				ChoicesMgr cMgr = new ChoicesMgr();
				DataTable options = cMgr.GetItemOptionInventory(ItemId, optionsKey);
				if (options.Rows.Count > 0)
				{
					description = options.Rows[0]["OptionNames"].ToString();
				}
			}

			UserStatus memberStatus = UserStatus.Enabled;

			//if (lw.Security.User.LoggedIn)
			//	memberStatus = lw.Members.MembersManager.GetUserStatus(lw.Security.User.LoggedInUser);


			decimal price = 0, salePrice = 0, resellerPrice = 0;

			if (lw.Products.Tools.CheckStatus(ItemStatus.ForSale, item))
			{
				if (String.IsNullOrWhiteSpace(priceOption))
				{
					if (item["Price"] != System.DBNull.Value)
						price = (decimal)item["Price"];

					if (item["SalePrice"] != System.DBNull.Value)
						salePrice = (decimal)item["SalePrice"];

					if (item["ResellerPrice"] != System.DBNull.Value)
						resellerPrice = (decimal)item["ResellerPrice"];

				}
				else
				{
					ItemPricesAdp itemPricesAdp = new ItemPricesAdp();
					DataRow priceRow = itemPricesAdp.GetItemPrice(ItemId, priceOption);

					if (priceRow["Price"] != System.DBNull.Value)
						price = (decimal)priceRow["Price"];

					if (priceRow["SalePrice"] != System.DBNull.Value)
						salePrice = (decimal)priceRow["SalePrice"];

					if (priceRow["ResellerPrice"] != System.DBNull.Value)
						resellerPrice = (decimal)priceRow["ResellerPrice"];
				}
				if (Tools.CheckStatus(ItemStatus.OnSale, item))
					price = salePrice > 0 ? salePrice : price;
				else
				{
					switch (memberStatus)
					{
						case UserStatus.Reseller:
							price = resellerPrice > 0 ? resellerPrice : price;
							break;
						default:
							break;
					}
				}
			}

			DataRow[] rows = this.ShoppingItems.BasketItems.Select(string.Format("ItemId={0} and OptionsKey='{1}' and PriceFor='{2}'", 
				ItemId, 
				StringUtils.SQLEncode(optionsKey),
				StringUtils.SQLEncode(priceOption)
			   ));
			if (rows.Length > 0)
				rows[0]["Quantity"] = (decimal)rows[0]["Quantity"] + qty;
			else
			{
				DataRow row = ShoppingItems.BasketItems.NewRow();

				decimal weight = 0;
				if (item["ShippingWeight"] != DBNull.Value)
					weight = decimal.Parse(item["ShippingWeight"].ToString());

				if (item["ShippingVWeight"] != DBNull.Value)
					weight = Math.Max(weight, decimal.Parse(item["ShippingVWeight"].ToString()));


				row["Weight"] = weight;

				row["ItemId"] = ItemId;
				row["Title"] = item["Title"].ToString();
				row["ItemNumber"] = item["ProductNumber"].ToString();
				row["Quantity"] = qty;
				row["UnitPrice"] = price ;
				row["Discount"] = 0;
				row["UniqueName"] = item["UniqueName"].ToString();
				row["OptionsKey"] = optionsKey;
				row["Description"] = description;
				row["Tax"] = 0;
				
				row["UniqueName"] = item["UniqueName"].ToString();
				row["PriceFor"] = priceOption;
				row["Category"] = category;

				this.ShoppingItems.BasketItems.Rows.Add(row);

				this.ShoppingItems.BasketItems.AcceptChanges();
			}
			this.AdjustTotals(true);
		}
		public void UpdateItem(int ItemId, int qty, string optionsKey, string priceOption)
		{
			UpdateItem(ItemId, qty, optionsKey, priceOption, true);
		}
		public void UpdateItem(int ItemId, int qty, string optionsKey, string priceOption, bool adjustTotals)
		{
			if (qty <= 0)
			{
				this.DeleteItem(ItemId, optionsKey);
				return;
			}
			DataRow[] rows = this.ShoppingItems.BasketItems.Select(string.Format("ItemId={0} and OptionsKey='{1}' and PriceFor='{2}'", 
				ItemId, 
				StringUtils.SQLEncode(optionsKey),
				StringUtils.SQLEncode(priceOption)
			 ));
			if (rows.Length > 0)
			{
				rows[0]["Quantity"] = qty;
			}
			if(adjustTotals)
				this.AdjustTotals(true);
		}
		public void DeleteItem(int ItemId, string optionsKey)
		{
			DeleteItem(ItemId, optionsKey, true);
		}
		public void DeleteItem(int ItemId, string optionsKey, bool adjustTotals)
		{
			DataRow[] rows = this.ShoppingItems.BasketItems.Select(string.Format("ItemId={0} and OptionsKey='{1}'", ItemId, optionsKey));
			if (rows.Length > 0)
			{
				rows[0].Delete();
			}
			if(adjustTotals)
				this.AdjustTotals(true);
		}

		public void DeleteItem(int ShoppingItemId)
		{
			DataRow[] rows = this.ShoppingItems.BasketItems.Select(string.Format("ShoppingItemId={0}", ShoppingItemId));
			if (rows.Length > 0)
			{
				rows[0].Delete();
			}
			this.AdjustTotals(true);
		}

		public string MaskVoucher()
		{
			if (this.ShoppingItems.GiftVouvher != "")
				return StringUtils.MaskString(this.ShoppingItems.GiftVouvher, '*', 4);
			return "";
		}
		public void AdjustTotals()
		{
			AdjustTotals(false);
		}
		public void AdjustTotals(bool SaveSession)
		{
			decimal subTotal = 0;
			decimal total = 0;
			decimal discount = 0;
			decimal tax = 0;
			decimal shipping = 0;
			decimal giftVoucherValue = ShoppingItems.GiftVoucherValue;
			decimal weight = 0;
			decimal handling = 0;

			foreach (DataRow row in ShoppingItems.BasketItems.Rows)
			{
				row["Total"] = (decimal)row["UnitPrice"] * (decimal)row["Quantity"];
				subTotal += (decimal)row["Total"];
				discount += (decimal)row["Discount"];
				tax += (decimal)row["Tax"];
				weight += (decimal)row["Weight"] * (decimal)row["Quantity"];
			}
			

			ShippingManager sm = new ShippingManager();

			handling = sm.CalculateHandling(subTotal);


			total = subTotal -
				discount +
				tax +
				giftVoucherValue +
				handling +
				ShoppingItems.PaymentCost;

			if (false && lw.Members.Security.User.LoggedIn)
			{
				DataRow member = lw.Members.Security.User.LoggedInUser(null, true);
				
				if(member["Country"] != System.DBNull.Value)
					shipping = sm.CalculateShipping(weight, member["Country"].ToString(), "All", subTotal);
	
				total += shipping;
			}
			else
				shipping = sm.CalculateShipping(weight, total);

			total = subTotal -
				discount +
				tax +
				shipping -
				giftVoucherValue +
				handling +
				ShoppingItems.PaymentCost;

			if (total < 0)
				total = 0;

			ShoppingItems.Total = total;
			ShoppingItems.SubTotal = subTotal;
			ShoppingItems.Discount = discount;
			ShoppingItems.Tax = tax;
			ShoppingItems.Shipping = shipping;
			ShoppingItems.Handling = handling;

			if (SaveSession)
				ShoppingItems.AcceptChanges();
		}
		public void SetPayment(string paymentKey)
		{
			Payment p = lw.Payments.Payments.Options.Find(delegate(Payment _p)
			{
				return _p.Key == paymentKey;
			});
			SetPayment(p);
		}
		public void SetPayment(Payment p)
		{
			ShoppingItems.Payment = p;
			ShoppingItems.AcceptChanges();
			AdjustTotals(true);
		}
		public bool CheckVoucherCode()
		{
			bool passed = true;
			if (this.ShoppingItems.GiftVouvher != "")
			{
				GiftVouchers gf = new GiftVouchers();
				OrdersDS.GiftVouchersRow voucher = gf.GetGiftVoucherByCode(this.ShoppingItems.GiftVouvher);
				if (voucher == null || voucher.Status == 1)
					passed = false;
			}
			return passed;
		}
		public bool CheckQuantities()
		{
			bool passed = true;

			StringBuilder cond = new StringBuilder();

			string sep = "";

			foreach (DataRow row in ShoppingItems.BasketItems.Rows)
			{
				if (row["OptionsKey"] == System.DBNull.Value || row["OptionsKey"].ToString() == "")
				{
					cond.Append(string.Format("{0}(Inventory<{1} and ItemId={2})",
						sep, row["Quantity"], row["ItemId"]));
				}
				else
				{
					cond.Append(string.Format("{0}(Inventory<{1} and ItemId={2} and OptionsKey='{3}')",
						sep, row["Quantity"], row["ItemId"], row["OptionsKey"]));
				}
				sep = " or ";
			}
			ItemsFullViewAdp adp = new ItemsFullViewAdp();

			DataTable dt = adp.GetForInventoryConfirmation(cond.ToString());


			if (dt.Rows.Count > 0)
			{
				StringBuilder missedInventoryItems = new StringBuilder();

				foreach (DataRow row in dt.Rows)
				{
					if(row["OptionsKey"] == DBNull.Value)
					{
						DataRow[] rows = ShoppingItems.BasketItems.Select(string.Format("ItemId={0}", row["ItemId"]));
						missedInventoryItems.Append(string.Format("{0} - {1}: Original Quantity({2})<BR />",
							rows[0]["Title"], rows[0]["ItemNumber"], rows[0]["Quantity"]));
						rows[0]["Quantity"] = Int32.Parse(row["Inventory"].ToString());
					}
					else
					{
						DataRow[] rows = ShoppingItems.BasketItems.Select(string.Format("ItemId={0} and OptionsKey='{1}'", row["ItemId"], row["OptionsKey"]));
						missedInventoryItems.Append(string.Format("{0} - {1} ({3}): Original Quantity({2})<BR />",
							rows[0]["Title"], rows[0]["ItemNumber"], rows[0]["Quantity"], rows[0]["Description"]));
						rows[0]["Quantity"] = Int32.Parse(row["Inventory"].ToString());
					}
				}
				ErrorContext.Add("wrong-inventory", missedInventoryItems.ToString());
				AdjustTotals(true);
				passed = false;
			}

			return passed;
		}
		public CartUpdateStatus Update(System.Web.HttpRequest req)
		{
			return Update(req, req["GiftVoucher"]);
		}
		public void UpdateCartItemXml(string id, string xml)
		{

		}
		public CartUpdateStatus Update(System.Web.HttpRequest req, string giftVoucherCode)
		{
			for(int i = 0; i <ShoppingItems.BasketItems.Rows.Count; i ++)
			{
				DataRow row = ShoppingItems.BasketItems.Rows[i];
				string id = row["ItemId"].ToString();
				string options = row["OptionsKey"].ToString();
				
				int qty = 0;
				if (req.Form[string.Format("Remove_{0}_{1}", id, StringUtils.SQLEncode(options))] != "on")
				{
					try
					{
						qty = Int32.Parse(req.Form[string.Format("Qty_{0}_{1}", id, StringUtils.SQLEncode(options))]);
					}
					catch
					{
						qty = (int)row["Quantity"];
					}
				}
				this.UpdateItem((int)row["ItemId"], qty, options, "", false);
				if (qty == 0)
					i--;
			}
			if (!String.IsNullOrEmpty(giftVoucherCode))
			{
				if(giftVoucherCode != MaskVoucher())
				{
					GiftVouchers gf = new GiftVouchers();
					OrdersDS.GiftVouchersRow voucher = gf.GetGiftVoucherByCode(giftVoucherCode);
					if (voucher == null)
						return CartUpdateStatus.IncorrectGiftVoucher;

					if (voucher.Status == (byte)1)
						return CartUpdateStatus.IncorrectGiftVoucher;

					ShoppingItems.GiftVouvher = giftVoucherCode;
					ShoppingItems.GiftVoucherValue = voucher.Amount;
					AdjustTotals(true);
					return CartUpdateStatus.GiftVoucherSuccessfullyAdded;
				}
			}
			else if(MaskVoucher() != "")
			{
				ShoppingItems.GiftVouvher = "";
				ShoppingItems.GiftVoucherValue = 0;
			}
			AdjustTotals(true);
			return CartUpdateStatus.Success;
		}
		public bool HasItems
		{
			get
			{
				return this.ShoppingItems.BasketItems.Rows.Count > 0;
			}
		}

		public void Empty()
		{
			WebContext.Profile.Basket = null;
			_shoppingItems = new Basket();
			ShoppingItems.AcceptChanges();
		}

		public Basket ShoppingItems
		{
			get
			{
				if(_shoppingItems == null)
				{
					_shoppingItems = new Basket();
				}
				return _shoppingItems;
			}
		}
	}

	public enum CartUpdateStatus
	{
		Success, IncorrectGiftVoucher, GiftVoucherSuccessfullyAdded
	}
}