using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using lw.Data;
using lw.Network;
using lw.ShoppingCart;
using lw.Utils;

namespace lw.Orders
{
	public class OrdersManager : DirectorBase
	{
		public OrdersManager()
			: base(cte.lib)
		{

		}


		public OrdersDS.OrdersDataTable GetOrders(string condition)
		{
			OrdersAdp adp = new OrdersAdp();
			return adp.GetOrders(condition);
		}

		public OrdersDS.OrdersRow GetOrder(int OrderId)
		{
			string cond = string.Format("OrderId={0}", OrderId);
			DataTable dt = GetOrders(cond);
			return dt.Rows.Count > 0 ? (OrdersDS.OrdersRow)dt.Rows[0] : null;
		}
		public OrdersDS.OrdersRow GetOrder(string OrderVisualId)
		{
			string cond = string.Format("OrderVisualId='{0}'", StringUtils.SQLEncode(OrderVisualId));
			DataTable dt = GetOrders(cond);
			return dt.Rows.Count > 0 ? (OrdersDS.OrdersRow)dt.Rows[0] : null;
		}
		public OrdersDS.OrdersRow GetOrderByVisualId(string VisualId)
		{
			VisualId = StringUtils.SQLEncode(VisualId);
			OrdersDSTableAdapters.OrdersTableAdapter adp = new lw.Orders.OrdersDSTableAdapters.OrdersTableAdapter();
			OrdersDS.OrdersDataTable dt = adp.GetOrderByVisualId(VisualId);
			if (dt.Rows.Count == 0)
				return null;

			return (OrdersDS.OrdersRow)dt.Rows[0];
		}
		public OrdersDS.OrdersRow GetOrderById(int OrderId)
		{
			OrdersDSTableAdapters.OrdersTableAdapter adp = new lw.Orders.OrdersDSTableAdapters.OrdersTableAdapter();
			OrdersDS.OrdersDataTable dt = adp.GetById(OrderId);
			if (dt.Rows.Count == 0)
				return null;

			return (OrdersDS.OrdersRow)dt.Rows[0];
		}

		public OrdersDS.OrdersRow CreateOrder(
			int MemberId,
			string FirstName,
			string LastName,
			string Phone,
			string Email,
			string Address,
			string City,
			string State,
			string Zip,
			string Country,
			string Region,
			bool ShippingSame,
			string ShippingPhone,
			string ShippingEmail,
			string ShippingAddress,
			string ShippingCity,
			string ShippingState,
			string ShippingZip,
			string ShippingCountry,
			string ShippingRegion,
			decimal SubTotal,
			decimal Total,
			decimal Discount,
			decimal Tax,
			decimal Shipping,
			decimal Handling,
			OrderType oType,
			string Description,
			string VoucherCode,
			decimal VoucherValue,
			string PaymentType,
			decimal PaymentCost)
		{
			return CreateOrder(MemberId, FirstName, LastName, Phone, Email, Address, City, State, Zip, Country, Region, ShippingSame, ShippingPhone, ShippingEmail,
				ShippingAddress, ShippingCity, ShippingState, ShippingZip, ShippingCountry, ShippingRegion, SubTotal, Total, Discount, Tax, Shipping, Handling, oType,
				Description, VoucherCode, VoucherValue, PaymentType, PaymentCost, "");
		}

		public OrdersDS.OrdersRow CreateOrder(
			int MemberId,
			string FirstName, 
			string LastName,
			string Phone, 
			string Email,
			string Address, 
			string City, 
			string State,
			string Zip,
			string Country,
			string Region,
			bool ShippingSame,
			string ShippingPhone,
			string ShippingEmail,
			string ShippingAddress,
			string ShippingCity,
			string ShippingState,
			string ShippingZip,
			string ShippingCountry,
			string ShippingRegion,
			decimal SubTotal, 
			decimal Total,
			decimal Discount, 
			decimal Tax,
			decimal Shipping,
			decimal Handling,
			OrderType oType,
			string Description,
			string VoucherCode,
			decimal VoucherValue,
			string PaymentType, 
			decimal PaymentCost, string XmlData )
		{
			OrdersDSTableAdapters.OrdersTableAdapter adp = new lw.Orders.OrdersDSTableAdapters.OrdersTableAdapter();
			OrdersDS.OrdersDataTable dt = adp.CreateOrder("", MemberId, FirstName, LastName, Phone, Email, Address, City, State,
				Zip, Country, Region, ShippingSame, ShippingPhone, ShippingAddress, ShippingCity,
				ShippingState, ShippingZip, ShippingCountry, ShippingRegion,
				DateTime.Now, (int)OrderStatus.Verifying, SubTotal, Total, Discount, Tax,
				Shipping, Handling, short.Parse(((int)oType).ToString()), Description, -1, "", "", VoucherCode, VoucherValue,
				PaymentType, PaymentCost, XmlData);

			OrdersDS.OrdersRow row = (OrdersDS.OrdersRow)dt.Rows[0];

			Random ran = new Random();

			string orderNbr = (row.OrderId * Math.Floor((double)ran.Next(1, 15))).ToString();

			orderNbr = Math.Pow(10, 10 - orderNbr.Length).ToString().Substring(1) + orderNbr;

			string[] temp = System.Guid.NewGuid().ToString().Split('-');

			row.OrderVisualId = temp[0] + "-" + temp[1] + "-" + temp[2];//string.Format("ORD{0}", orderNbr);

			adp.Update(row);

			return row;
		}

        public void DeleteOrder(int orderId)
        {
			OrdersDSTableAdapters.OrdersTableAdapter adp = new lw.Orders.OrdersDSTableAdapters.OrdersTableAdapter();
			adp.DeleteByOrderId(orderId);
        }

		public void UpdateOrderProperty(int OrderId, string property, string value)
		{
			string sql = string.Format("Update Orders set {0}={1} where OrderId={2}",
				property, value, OrderId);

			DBUtils.ExecuteQuery(sql, cte.lib);
		}
		public void UpdateOrderStatus(int OrderId, OrderStatus status)
		{
			UpdateOrderProperty(OrderId, "Status", ((int)status).ToString());
		}
       
        public void AddComment(int OrderId, string Comments)
        {
			OrdersDSTableAdapters.OrdersTableAdapter adp = new lw.Orders.OrdersDSTableAdapters.OrdersTableAdapter();
			OrdersDS.OrdersRow order = GetOrder(OrderId);
			adp.UpdateComments(order.Comments + "{sep}" + Comments, OrderId);
		}

		#region Shopping Cart Orders

		void RejectInventoryChanges(int orderId)
		{
			lw.ShoppingCart.ShoppingDsTableAdapters.ShoppingItemsTableAdapter adp = new lw.ShoppingCart.ShoppingDsTableAdapters.ShoppingItemsTableAdapter();

			ShoppingDs.ShoppingItemsDataTable dt = adp.GetDataByOrder(orderId);

			StringBuilder sb = new StringBuilder();
			foreach (DataRow row in dt.Rows)
			{
				if (row["OptionsKey"] == System.DBNull.Value || row["OptionsKey"].ToString() == "")
				{
					sb.Append(string.Format("Update Items set StockQuantity = StockQuantity + {0} where ItemId={1};",
						row["Quantity"], row["ItemId"]));
				}
				else
				{
					sb.Append(string.Format("Update ItemOptionsInventory set Inventory=Inventory+{0} where ItemId={1} and [Key]='{2}';",
						row["Quantity"], row["ItemId"], row["OptionsKey"]));
				}
			}
			if (sb.Length > 0)
				DBUtils.ExecuteQuery(sb.ToString(), cte.lib);
		}

		public string CreateShoppingCartOrder()
		{
			/*
			MembersDs.MembersRow member = (MembersDs.MembersRow)lw.Security.User.LoggedInUser;
			lw.ShoppingCart.ShoppingCart sc = new lw.ShoppingCart.ShoppingCart(); 
			Basket basket = sc.ShoppingItems;
			OrdersDS.OrdersRow order = CreateOrder(
				member.MemberId,
				member.FirstName,
				member.LastName,
				member.Phone,
				member.Email,
				member.Address,
				member.City,
				member.State,
				member.Zip,
				member.Country,
				member.Region,
				true,
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				basket.SubTotal,
				basket.Total,
				basket.Discount,
				basket.Tax,
				basket.Shipping,
				basket.Handling,
				OrderType.Products,
				"",
				basket.GiftVouvher,
				basket.GiftVoucherValue,
				String.IsNullOrEmpty(basket.PaymentType)? Payments.Payments.DefaultPayment.ToString(): basket.PaymentType,
				basket.PaymentCost);

			foreach (ShoppingDs.ShoppingItemsRow item in basket.BasketItems)
				item.OrderId = order.OrderId;

			lw.WebTools.WebContext.Response.Write(basket.BasketItems[0]["Xml"]);

			lw.Orders.ShoppingDsTableAdapters.ShoppingItemsTableAdapter adp = new lw.Orders.ShoppingDsTableAdapters.ShoppingItemsTableAdapter();
			adp.Update(basket.BasketItems);

			StringBuilder sb = new StringBuilder();
			foreach (DataRow row in basket.BasketItems.Rows)
			{
				if (row["OptionsKey"] == System.DBNull.Value || row["OptionsKey"].ToString() == "")
				{
					sb.Append(string.Format("Update Items set StockQuantity = StockQuantity - {0} where ItemId={1};",
						row["Quantity"], row["ItemId"]));
				}
				else
				{
					sb.Append(string.Format("Update ItemOptionsInventory set Inventory=Inventory-{0} where ItemId={1} and [Key]='{2}';",
						row["Quantity"], row["ItemId"], row["OptionsKey"]));
				}
			}
			if(sb.Length > 0)
				DBUtils.ExecuteQuery(sb.ToString(), cte.lib);

			if (order.VoucherCode != "")
			{
				GiftVouchers gf = new GiftVouchers();
				gf.UpdateVoucherStatus(order.VoucherCode, 1);
			}

			return order.OrderVisualId;
			
			 */
			return "";
		}
		#endregion

		#region Order Confirmation
		public void ConfirmProductsOrder(OrdersDS.OrdersRow order, OrderStatus status)
		{
			UpdateOrderStatus(order.OrderId, status);

			NameValueCollection dic = new NameValueCollection();
			dic["OrderId"] = order.OrderVisualId;
			dic["Status"] = status.ToString();
			dic["Name"] = order.FirstName + " " + order.LastName;
			dic["Email"] = order.Email;
			dic["Phone"] = order.Phone;
			dic["Address"] = order.Address;
			dic["ShoppingItems"] = GetShoppingItems(order);
			dic["Amount"] = string.Format("{0:c}", order.Total);
			dic["Description"] = order.Description;
//			dic["ShippingKey"] = string.Format("<p>Shipping Key: {0}</p>", order.ShippingKey);


			Mail m = new Mail("Order-Notification");
			m.Data = dic;
			m.Send();

			m = new Mail("Order-Confirmation");
			m.To = order.Email;
			m.Data = dic;
			m.Send();
		}
		public void ConfirmProductsOrder(OrdersDS.OrdersRow order)
		{
			ConfirmProductsOrder(order, OrderStatus.ReadyForShipping);
		}
		public void DeclineProductsOrder(OrdersDS.OrdersRow order)
		{
			UpdateOrderStatus(order.OrderId, OrderStatus.Canceled);
			RejectInventoryChanges(order.OrderId);
			if (order.VoucherCode != "")
			{
				GiftVouchers gf = new GiftVouchers();
				gf.UpdateVoucherStatus(order.VoucherCode, 0);
			}

			NameValueCollection dic = new NameValueCollection();
			dic["OrderId"] = order.OrderVisualId;
			dic["Status"] = OrderStatus.Canceled.ToString();
			dic["Name"] = order.FirstName + " " + order.LastName;
			dic["Email"] = order.Email;
			dic["Phone"] = order.Phone;
			dic["Country"] = order.Country;
			dic["Region"] = order.Region;
			dic["Address"] = order.Address;
			dic["ShoppingItems"] = GetShoppingItems(order);
			dic["Description"] = order.Description;

			dic["Amount"] = string.Format("{0:c}", order.Total);

			Mail m = new Mail("Order-Notification");
			m.Data = dic;
			m.Subject = "Order Declined";
			m.Send();
		}
		public void ConfirmOrder(string OrderVisualId)
		{
			OrdersDS.OrdersRow order = GetOrder(OrderVisualId);

			OrderStatus status = GetOrderStatus(order.Status);

			ConfirmProductsOrder(order, status);
		}
		public void ConfirmOrder(string OrderVisualId, bool approved, string avs)
		{
			OrdersDS.OrdersRow order = GetOrder(OrderVisualId);
			AddComment(order.OrderId, avs);

			switch(order.OrderType)
			{
				case (short)OrderType.GiftVoucher:
					if (approved)
						ConfirmGiftVoucherOrder(order);
					else
						DeclineGiftVoucherOrder(order);
					break;
				default:
					if (approved)
						ConfirmProductsOrder(order);
					else
						DeclineProductsOrder(order);
					break;
			}
		}
		public string GetShoppingItems(int orderId)
		{
			return GetShoppingItems(GetOrder(orderId));
		}
		public string GetShoppingItems(OrdersDS.OrdersRow order)
		{
			lw.ShoppingCart.ShoppingDsTableAdapters.ShoppingItemsTableAdapter adp = new lw.ShoppingCart.ShoppingDsTableAdapters.ShoppingItemsTableAdapter();

			StringBuilder sb = new StringBuilder();

			ShoppingDs.ShoppingItemsDataTable shoppingItems = adp.GetDataByOrder(order.OrderId);

			sb.Append("<table width=500 border=1 cellpadding=3>");
			sb.Append("<tr style=\"font-weight:bold\"><td>Product</td><td>Quantity</td><td>Unit Price</td><td>Sub Total</td></tr>");

			bool alter = false;
			foreach (ShoppingDs.ShoppingItemsRow item in shoppingItems)
			{
				sb.Append(string.Format(
					"<tr bgcolor={0}><td>{1}<BR /># {2}<BR>{3}</td><td>{4}<BR />{6}</td><td>{5}</td><td>{7}</td></tr>",
					alter ? "#ffffff" : "#efefef",
					item.Title, item.ItemNumber, item.Description,
					item.Quantity, string.Format("{0:##,###0.00} SAR", item.UnitPrice),
					item.PriceFor,
					string.Format("{0:##,###0.00} SAR", item.Quantity*item.UnitPrice)
					)
				);
				alter = !alter;
			}

			sb.Append("<tr><td colspan=4 align=right>");
			sb.Append(string.Format("Sub Total: {0:##,###0.00} SAR<br />", order.SubTotal));
			sb.Append(string.Format("Shipping: {0:##,###0.00} SAR<br />", order.Shipping));
			sb.Append(string.Format("Handling: {0:##,###0.00} SAR<br />", order.Handling));
			if(order.Discount > 0)
				sb.Append(string.Format("Discount: {0:##,###0.00} SAR<br />", order.SubTotal));
			if(order.VoucherValue != 0)
				sb.Append(string.Format("Voucher Code Value: {0:##,###0.00} SAR<br />", order.VoucherValue));
			string paymentName = "";
			lw.Payments.Payment payment = lw.Payments.Payments.GetPayment(order.PaymentType);
			if (payment != null)
				paymentName = payment.DisplayName;

			sb.Append(string.Format("Payment Type: {0}<br />", paymentName));
			sb.Append(string.Format("Payment Cost: {0:##,###0.00} SAR<br />", order.PaymentCost));
			sb.Append(string.Format("<b>Order Total: {0:##,###0.00} SAR</b><br />", order.Total));

			sb.Append("</td></tr></table>");

			return sb.ToString();
		}
		#endregion

		#region GiftVouchers
		public string CreateGiftVoucherOrder(int MemberId, string FromFirstName, string FromLastName, string FromEmail, string ToName, string ToEmail, decimal Amount, string Comments)
		{
			GiftVouchers gv = new GiftVouchers();
			DataTable voucher = gv.CreateVoucher(Amount);

			string desc = Comments;

			OrdersDS.OrdersRow row = this.CreateOrder(MemberId, FromFirstName, FromLastName, "", FromEmail, "", "", "", "", "", "", false,
				"", "", "", "", "", "", "", "", Amount, Amount, 0, 0, 0, 0, OrderType.GiftVoucher, desc, "", 0, "", 0);

			gv.UpdateVoucher(Int32.Parse(voucher.Rows[0]["VoucherId"].ToString()), ToName, ToEmail, row.OrderId);
		
			return row.OrderVisualId;
		}
		void ConfirmGiftVoucherOrder(OrdersDS.OrdersRow order)
		{
			UpdateOrderStatus(order.OrderId, OrderStatus.Shipped);


			GiftVouchers gf = new GiftVouchers();

			OrdersDS.GiftVoucherOrdersRow voucher =
				(OrdersDS.GiftVoucherOrdersRow)gf.GetGiftVoucherByOrder(order.OrderId);
			try
			{
				NameValueCollection dic = new NameValueCollection();
				dic["From"] = voucher.FirstName + " " + voucher.LastName;
				dic["FromEmail"] = voucher.Email;
				dic["To"] = voucher.ToName;
				dic["ToEmail"] = voucher.ToEmail;
				dic["status"] = "Verified";
				dic["Amount"] = string.Format("{0:c}", voucher.Amount);
				dic["VoucherCode"] = voucher.VoucherCode;
				dic["Message"] = order.Description;

				Mail m = new Mail("Gift-Voucher-Notif");
				m.Data = dic;
				m.Send();

				m = new Mail("Gift Card Receiver");
				m.To = dic["ToEmail"].ToString();
				m.Subject = m.Subject.Replace("{FromName}", dic["From"].ToString());
				m.Data = dic;
				m.Send();

				m = new Mail("Gift Card Sender");
				m.To = dic["FromEmail"].ToString();
				m.Data = dic;
				m.Send();
			}
			catch
			{
			}
		}
		void DeclineGiftVoucherOrder(OrdersDS.OrdersRow order)
		{
			UpdateOrderStatus(order.OrderId, OrderStatus.Canceled);

			GiftVouchers gf = new GiftVouchers();

			OrdersDS.GiftVoucherOrdersRow voucher =
				(OrdersDS.GiftVoucherOrdersRow)gf.GetGiftVoucherByOrder(order.OrderId);

			NameValueCollection dic = new NameValueCollection();
			dic["From"] = voucher.FirstName + " " + voucher.LastName;
			dic["FromEmail"] = voucher.Email;
			dic["To"] = voucher.ToName;
			dic["ToEmail"] = voucher.ToEmail;
			dic["status"] = "Declined";
			dic["Amount"] = string.Format("{0:c}", voucher.Amount);
			dic["VoucherCode"] = voucher.VoucherCode;
			dic["Message"] = order.Description;

			Mail m = new Mail("Gift-Voucher-Notif");
			m.Data = dic;
			m.Send();
		}
		#endregion

		#region status
		public static OrderStatus GetOrderStatus(int st)
		{
			return (OrderStatus)Enum.Parse(typeof(OrderStatus), st.ToString());
		}
		public static string GetOrderStatusName(int st)
		{
			return EnumHelper.GetDescription(GetOrderStatus(st));
		}
		#endregion
	}
}