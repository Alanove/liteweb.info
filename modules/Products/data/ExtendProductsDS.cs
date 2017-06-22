using System;
using System.Data;

using lw.Data;
using lw.Utils;

namespace lw.Products
{
	public class ItemsFullViewAdp : ProductsDSTableAdapters.ItemsFullViewTableAdapter
	{
		public DataTable GetItemsFullView(string condition)
		{
			if (condition != "")
				condition = " where " + condition;
			base.CommandCollection[0].CommandText += condition;
			DataTable dt =  base.GetData();

			foreach (ItemStatus status in Enum.GetValues(typeof(ItemStatus)))
			{
				string t = status.ToString();
				if (t == "Enabled" || t == "Disabled")
					continue;

				dt.Columns.Add(t);
			}

			foreach (DataRow dr in dt.Rows)
			{
				int s = (int)dr["Status"];
				foreach (ItemStatus status in Enum.GetValues(typeof(ItemStatus)))
				{
					string t = status.ToString();
					if (t == "Enabled" || t == "Disabled")
						continue;

					dr[t] = (s & (int)status) != 0 ? 1 : 0;
				}
				dr["Status"] = (s & (int)ItemStatus.Enabled) != 0 ? 1 : 0;
			}

			return dt;
		}
		public DataTable GetForInventoryConfirmation(string cond)
		{
			if (cond != "")
				cond = " where " + cond;
			base.CommandCollection[1].CommandText += cond;

			return DBUtils.GetDataSet(base.CommandCollection[1].CommandText, cte.lib).Tables[0];
		}
	}
	public class ItemsAdp : ProductsDSTableAdapters.ItemsTableAdapter
	{
		public ProductsDS.ItemsDataTable GetItems(string condition)
		{
			if (condition != "")
				condition = " where " + condition;
			base.CommandCollection[0].CommandText += condition;
			return base.GetData();
		}
	}
	public class ItemsViewAdp : ProductsDSTableAdapters.ItemsViewTableAdapter
	{
		public ProductsDS.ItemsViewDataTable GetItems(string condition)
		{
			if (condition != "")
				condition = " where " + condition;
			base.CommandCollection[0].CommandText += condition;
			return base.GetData();
		}
		public ProductsDS.ItemsViewDataTable GetItemsDistinct(string condition)
		{
			 if (condition != "")
				condition = " where " + condition;

			base.CommandCollection[0].CommandText = @"
select Distinct
	ItemId, ProductNumber, Title, BrandId, Status, Price, 
	ThumbImage, Image1, Image2, Image3, Image4, 
	DateCreated, LastModified, Brand,
	StockQuantity, ShippingWeight, ShippingLength, ShippingWidth, ShippingHeight, 
	Inventory, Warranty, Packaging, Ranking, UserRating, [Views], SalePrice
from ItemsView " + condition;
			  
			return base.GetData();
		}
	}
	public class CategoriesAdp : ProductsDSTableAdapters.CategoriesTableAdapter
	{
		public ProductsDS.CategoriesDataTable GetCategories(string condition)
		{
			if (condition != "")
				condition = " where " + condition;
			base.CommandCollection[0].CommandText += condition;
			return base.GetData();
		}
	}
	public class CategoriesViewAdp : ProductsDSTableAdapters.CategoriesViewTableAdapter
	{
		public ProductsDS.CategoriesViewDataTable GetCategories(string condition)
		{
			if (condition != "")
				condition = " where " + condition;
			base.CommandCollection[0].CommandText += condition;
			return base.GetData();
		}
	}
	public class BrandsAdp : ProductsDSTableAdapters.BrandsTableAdapter
	{
		public ProductsDS.BrandsDataTable GetBrands(string condition)
		{
			if (condition != "")
				condition = " where " + condition;
			base.CommandCollection[0].CommandText += condition;
			return base.GetData();
		}
	}

	public class ItemPricesAdp : ProductsDSTableAdapters.ItemPricesTableAdapter
	{
		public ProductsDS.ItemPricesDataTable GetItemPrices(string condition)
		{
			if (condition != "")
				condition = " where " + condition;
			base.CommandCollection[0].CommandText += condition;
			return base.GetData();
		}
		public ProductsDS.ItemPricesDataTable GetItemPrices(int ItemId)
		{
			return GetItemPrices("ItemId=" + ItemId.ToString());
		}

        public DataRow GetItemPrice(int ItemId, string PriceFor)
        {
            DataTable dt = GetItemPrices(string.Format("ItemId={0} and PriceFor='{1}'", ItemId, StringUtils.SQLEncode(PriceFor)));
            if (dt.Rows.Count > 0)
                return dt.Rows[0];
            return null;
        }

		public ProductsDS.ItemPricesRow GetItemPrice(int KeyId)
		{
			ProductsDS.ItemPricesDataTable rows = GetItemPrices("Id=" + KeyId.ToString());
			if (rows.Count > 0)
				return rows[0];
			return null;
		}

		public DataView GetItemPrices(int ItemId, lw.Base.CustomPage page)
		{
			ProductsDS.ItemPricesDataTable allPrices;
			if (page.PageContext[cte.ItemPricesContext] == null)
			{
				string itemIds = page.PageContext[cte.ItemIdsContext].ToString();
				string cond = "";
				if (!String.IsNullOrWhiteSpace(itemIds))
				{
					cond = "ItemId in  (" + itemIds.Substring(0, itemIds.Length - 1) + ")";
				}
				allPrices = GetItemPrices(cond);
				page.PageContext[cte.ItemPricesContext] = allPrices;
			}
			else
				allPrices = page.PageContext[cte.ItemPricesContext] as ProductsDS.ItemPricesDataTable;

			return new DataView(allPrices, "ItemId=" + ItemId.ToString(), "", DataViewRowState.CurrentRows);
		}
		
	}
}
