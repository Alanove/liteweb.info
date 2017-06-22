using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using lw.ShoppingCart;
using lw.ShoppingCart.ShoppingDsTableAdapters;


namespace lw.Orders.Controls
{
	public class ShoppingItems : System.Web.UI.HtmlControls.HtmlTable
	{
		bool bound = false;

		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			int orderId = (int)DataBinder.Eval(NamingContainer, "DataItem.OrderId");

			OrdersManager oMgr = new OrdersManager();

			OrdersDS.OrdersRow order = oMgr.GetOrder(orderId);

			ShoppingItemsTableAdapter adp = new ShoppingItemsTableAdapter();

			StringBuilder sb = new StringBuilder();

			ShoppingDs.ShoppingItemsDataTable shoppingItems = adp.GetDataByOrder(orderId);

			HtmlTableRow header = new HtmlTableRow();
			header.Attributes["class"] = "header";

			HtmlTableCell cell = new HtmlTableCell();
			cell.InnerHtml = "Product";
			header.Cells.Add(cell);

			cell = new HtmlTableCell();
			cell.InnerHtml = "Quantity";
			header.Cells.Add(cell);

			cell = new HtmlTableCell();
			cell.InnerHtml = "Unit Price";
			cell.Align = "right";
			header.Cells.Add(cell);

			cell = new HtmlTableCell();
			cell.InnerHtml = "Sub Total";
			cell.Align = "right";
			header.Cells.Add(cell);

			this.Rows.Add(header);

			bool alter = false;
			foreach (ShoppingDs.ShoppingItemsRow item in shoppingItems)
			{
				HtmlTableRow row = new HtmlTableRow();
				row.Attributes["class"] = alter ? "alter" : "";
				string temp = string.Format(
					"{0}<BR /># {1}<BR>{2}", 
					item.Title, item.ItemNumber, item.Description
				);
				cell = new HtmlTableCell();
				cell.InnerHtml = temp;
				row.Cells.Add(cell);

				cell = new HtmlTableCell();
				cell.InnerHtml = item.Quantity.ToString();
				row.Cells.Add(cell);

				cell = new HtmlTableCell();
				cell.Align = "right";
				cell.InnerHtml = string.Format(" {0:##,###0.00} SAR", item.UnitPrice);
				row.Cells.Add(cell);

				cell = new HtmlTableCell();
				cell.Align = "right";
				cell.InnerHtml = string.Format(" {0:##,###0.00} SAR", (item.UnitPrice * item.Quantity));
				row.Cells.Add(cell);

				alter = !alter;
				Rows.Add(row);
			}

			HtmlTableRow footer = new HtmlTableRow();
			footer.Attributes["class"] = "footer";
			HtmlTableCell footerCell = new HtmlTableCell();
			footerCell.ColSpan = 5;

			sb.Append(string.Format("Sub Total:  {0:##,###0.00} SAR<br />", order.SubTotal));
			sb.Append(string.Format("Shipping:  {0:##,###0.00} SAR<br />", order.Shipping));
			sb.Append(string.Format("Handling:  {0:##,###0.00} SAR<br />", order.Handling));
			if (order.Discount > 0)
				sb.Append(string.Format("Discount:  {0:##,###0.00} SAR<br />", order.SubTotal));
			if (order.VoucherValue != 0)
				sb.Append(string.Format("Voucher Code Value:  {0:##,###0.00} SAR<br />", order.VoucherValue));
			string paymentName = "";
			lw.Payments.Payment payment = lw.Payments.Payments.GetPayment(order.PaymentType);
			if (payment != null)
				paymentName = payment.DisplayName;

			sb.Append(string.Format("Payment Type: {0}<br />", paymentName));
			sb.Append(string.Format("Payment Cost:  {0:##,###0.00} SAR<br />", order.PaymentCost));
			sb.Append(string.Format("<b>Order Total:  {0:##,###0.00} SAR</b><br />", order.Total));

			footerCell.InnerHtml = sb.ToString();
			footer.Cells.Add(footerCell);
			
			Rows.Add(footer);

			base.DataBind();
		}
	}
}
