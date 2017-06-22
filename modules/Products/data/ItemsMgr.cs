using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using lw.CTE;
using lw.Data;
using lw.Utils;
using lw.WebTools;

namespace lw.Products
{
	public class ItemsMgr : DirectorBase
	{
		CategoriesMgr _categoriesMgr;
		public ItemsMgr()
			: base(cte.lib)
		{
			_categoriesMgr = new CategoriesMgr();
		}

		#region Get Items

		public ProductsDS.ItemsViewDataTable GetItemsView(string cond)
		{
			ItemsViewAdp adp = new ItemsViewAdp();
			return adp.GetItems(cond);
		}
		public ProductsDS.ItemsViewDataTable GetItemsSearchView(string cond)
		{
			ItemsViewAdp adp = new ItemsViewAdp();
			return adp.GetItemsDistinct(cond);
		}

		public ProductsDS.ItemsDataTable GetItems(string cond)
		{
			ItemsAdp adp = new ItemsAdp();
			return adp.GetItems(cond);
		}
		public ProductsDS.ItemsViewDataTable GetItems(int CategoryId)
		{
			return this.GetItemsView(string.Format("CategoryId={0}", CategoryId));
		}
		public ProductsDS.ItemsViewDataTable GetItems(int CategoryId, ItemStatus itemStatus)
		{
			return this.GetItemsView(string.Format("CategoryId={0} and Status&{1}<>0", CategoryId, (int)itemStatus));
		}

		public ProductsDS.ItemsRow GetItem(int ItemId)
		{
			ProductsDS.ItemsDataTable items = this.GetItems(string.Format("ItemId={0}", ItemId));
			if (items.Rows.Count > 0)
				return (ProductsDS.ItemsRow)items.Rows[0];
			return null;
		}
		public ProductsDS.ItemsRow GetItemByUniqueName(string UniqueName)
		{
			ProductsDS.ItemsDataTable items = this.GetItems(string.Format("UniqueName='{0}'", StringUtils.SQLEncode(UniqueName)));
			if (items.Rows.Count > 0)
			{
				ProductsDS.ItemsRow item = (ProductsDS.ItemsRow)items.Rows[0];
				DataRow dr = this.GetItemsSearchView(string.Format("ItemId={0}", item.ItemId))[0];
				item["Inventory"] = double.Parse(dr["Inventory"].ToString());
				return item;
			}
			return null;
		}
		public ProductsDS.ItemsRow GetItem(string ProductNumber)
		{
			ProductsDS.ItemsDataTable items = this.GetItems(string.Format("ProductNumber='{0}'", StringUtils.SQLEncode(ProductNumber)));
			if (items.Rows.Count > 0)
				return (ProductsDS.ItemsRow)items.Rows[0];
			return null;
		}

		public ProductsDS.ItemsViewDataTable GetEnabledItems(string cond)
		{
			if (cond != "")
				cond += string.Format(" And Status&{0}<>0", ItemStatus.Enabled);
			else
				cond = string.Format(" And Status&{0}<>0", ItemStatus.Enabled);

			return GetItemsView(cond);
		}

		public ProductsDS.ItemsViewDataTable GetEnabledItems(int CategoryId)
		{
			return GetEnabledItems(string.Format("CategoryId={0}", CategoryId));
		}
		public ProductsDS.CategoriesViewDataTable GetItemParents(int ItemId)
		{
			string sql = "CategoryId in (select CategoryId from ItemCategories Where ItemId={0})";
			sql = string.Format(sql, ItemId);

			return _categoriesMgr.GetCategories(sql);
		}

		public DataTable GetItemsFullView(string Search)
		{
			ItemsFullViewAdp adp = new ItemsFullViewAdp();
			return adp.GetItemsFullView(Search);
		}
		#endregion

		#region Add Items


		/// <summary>
		/// Adds an item to the database
		/// </summary>
		/// <param name="req">Request.Form</param>
		/// <returns>ItemId</returns>
		public int AddItem(HttpRequest req)
		{
			int _status = 0;

			foreach (ItemStatus status in Enum.GetValues(typeof(ItemStatus)))
			{
				string s = req[status.ToString()];
				if (s != null && s == "on")
					_status |= (int)status;
			}
			_status |= (req["ItemEnabled"] == "on" ? (int)ItemStatus.Enabled : (int)ItemStatus.Disabled);

			int BrandId = Int32.Parse(req["BrandId"]);

			decimal? Price = null;
			if (!String.IsNullOrWhiteSpace(req["Price"]))
				Price = decimal.Parse(req["Price"]);

			decimal ResellerPrice = 0;
			if (!String.IsNullOrWhiteSpace(req["ResellerPrice"]))
				ResellerPrice = decimal.Parse(req["ResellerPrice"]);

			decimal SalePrice = 0;
			if (!String.IsNullOrWhiteSpace(req["SalePrice"]))
				SalePrice = decimal.Parse(req["SalePrice"]);

			int StockQuantity = 0;
			if (req["Inventory"] != null && req["Inventory"] != "")
				StockQuantity = Int32.Parse(req["Inventory"]);

			double ShippingWeight = 0;
			if (req["ShippingWeight"] != null && req["ShippingWeight"] != "")
				ShippingWeight = double.Parse(req["ShippingWeight"]);

			double ShippingWidth = 0;
			if (req["ShippingWidth"] != null && req["ShippingWidth"] != "")
				ShippingWidth = double.Parse(req["ShippingWidth"]);

			double ShippingHeight = 0;
			if (req["ShippingHeight"] != null && req["ShippingHeight"] != "")
				ShippingHeight = double.Parse(req["ShippingHeight"]);

			double ShippingLength = 0;
			if (req["ShippingLength"] != null && req["ShippingLength"] != "")
				ShippingLength = double.Parse(req["ShippingLength"]);

			int Ranking = 0;
			if (req["Ranking"] != null && req["Ranking"] != "")
				Ranking = Int32.Parse(req["Ranking"]);

			double ShippingVWeight = 0;
			if (!String.IsNullOrWhiteSpace(req["ShippingVWeight"]))
				ShippingVWeight = double.Parse(req["ShippingVWeight"]);

			int ItemId = AddItem(req["ProductNumber"], req["Title"], req["Keywords"], BrandId, _status, req["SmallDescription"],
				WebContext.Server.HtmlDecode(req["LargeDescription"]), Price, ResellerPrice, SalePrice,
				req.Files["ThumbImage"], req.Files["Image1"], req.Files["Image2"], req.Files["Image3"], req.Files["Image4"],
				req["Specs"], StockQuantity, ShippingWeight, ShippingVWeight, ShippingWidth, ShippingHeight, ShippingLength, req.Files["DownloadableFile"],
				req["Warranty"], req["Packaging"], Ranking, req["Website"], WebContext.Server.HtmlDecode(req["Template"]));

			if (ItemId > 0)
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				string temp = "Insert into ItemCategories (ItemId, CategoryId) values ({0}, {1});";

				ProductsDS.CategoriesViewDataTable cats = _categoriesMgr.GetCategories("");
				foreach (DataRow v in cats)
				{
					if (req["c" + v["CategoryId"].ToString()] == "on")
						sb.Append(string.Format(temp, ItemId, v["CategoryId"]));
				}

				string str = sb.ToString();
				if (str != "")
				{
					System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
					cmd.CommandText = str;
					base.ExecuteCommand(cmd);
				}


				UpdateItemPrices(ItemId, req);
			}
			return ItemId;
		}

		public int AddItem(string ProductNumber, string Title, string Keywords, int BrandId, int Status, string SmallDescription,
			string LargeDescription, decimal? Price, decimal? ResellerPrice, decimal? SalePrice,
			HttpPostedFile ThumbImage, HttpPostedFile Image1, HttpPostedFile Image2, HttpPostedFile Image3, HttpPostedFile Image4,
			string Specs, int StockQuantity, double ShippingWeight,double ShippingVWeight,  double ShippingWidth, double ShippingHeight, double ShippingLength,
			HttpPostedFile DownloadableFile, string Warranty, string Packaging, int Ranking, string Website, string Template)
		{
			ProductsDS.ItemsDataTable oldItems = this.GetItems(string.Format("ProductNumber='{0}' or Title='{1}'", 
				StringUtils.SQLEncode(ProductNumber), 
				StringUtils.SQLEncode(Title)));

			if (oldItems.Rows.Count > 0)
			{
				ProductsDS.ItemsRow oldItem = (ProductsDS.ItemsRow)oldItems.Rows[0];
				UpdateItem(oldItem.ItemId, ProductNumber, Title, Keywords, BrandId,
					Status, SmallDescription,
					LargeDescription, Price, ResellerPrice, SalePrice,
					ThumbImage, false, Image1, false,
					Image2, false, Image3, false,
					Image4, false, Specs, StockQuantity, ShippingWeight, ShippingVWeight,
					ShippingWidth, ShippingHeight, ShippingLength,
					DownloadableFile, false, Warranty, Packaging, Ranking,
					oldItem.UserRating, oldItem.Views, Packaging, Website, Template);

				return oldItem.ItemId;
			}
			string UniqueName = GetUniqueName(Title, ProductNumber);
			ProductsDSTableAdapters.ItemsTableAdapter adp = new lw.Products.ProductsDSTableAdapters.ItemsTableAdapter();
			adp.Insert(ProductNumber, Title, Keywords, BrandId, Status, SmallDescription, LargeDescription,
				Price, SalePrice, ResellerPrice, "", "", "", "", DateTime.Now, DateTime.Now, "", Specs, StockQuantity,
				ShippingVWeight, 
				ShippingWeight, ShippingWidth, ShippingHeight, ShippingLength, "", Warranty, Packaging, Ranking,
				0, 0, UniqueName, Website, Template);

			int ItemId = GetItem(ProductNumber).ItemId;

			HandleItemFiles(ItemId,
				ThumbImage, false,
				Image1, false,
				Image2, false,
				Image3, false,
				Image4, false,
				DownloadableFile, false);


			return ItemId;
		}
		public string GetUniqueName(string Title, string ProductNumber)
		{
			string uniqueName = Title + " " + ProductNumber;
			Regex r = new Regex("-+");
			uniqueName = StringUtils.ToURL(uniqueName);
			return r.Replace(uniqueName, "-");
		}

		#endregion

		#region Update Items
		public int UpdateItem(int ItemId, System.Web.HttpRequest req)
		{
			int _status = 0;

			foreach (ItemStatus status in Enum.GetValues(typeof(ItemStatus)))
			{
				string s = req[status.ToString()];
				if (s != null && s == "on")
					_status |= (int)status;
			}
			_status |= (req["ItemEnabled"] == "on" ? (int)ItemStatus.Enabled : (int)ItemStatus.Disabled);

			int BrandId = Int32.Parse(req["BrandId"]);

			decimal? Price = null;
			if (!String.IsNullOrWhiteSpace(req["Price"]))
				Price = decimal.Parse(req["Price"]);

			decimal? ResellerPrice = null;
			if (!String.IsNullOrWhiteSpace(req["ResellerPrice"]))
				ResellerPrice = decimal.Parse(req["ResellerPrice"]);

			decimal? SalePrice = null;
			if (!String.IsNullOrWhiteSpace(req["SalePrice"]))
				SalePrice = decimal.Parse(req["SalePrice"]);

			int StockQuantity = 0;
			if (req["Inventory"] != null && req["Inventory"] != "")
				StockQuantity = Int32.Parse(req["Inventory"]);

			double ShippingWeight = 0;
			if (req["ShippingWeight"] != null && req["ShippingWeight"] != "")
				ShippingWeight = double.Parse(req["ShippingWeight"]);

			double ShippingVWeight = 0;
			if (req["ShippingVWeight"] != null && req["ShippingVWeight"] != "")
				ShippingVWeight = double.Parse(req["ShippingVWeight"]);

			double ShippingWidth = 0;
			if (req["ShippingWidth"] != null && req["ShippingWidth"] != "")
				ShippingWidth = double.Parse(req["ShippingWidth"]);

			double ShippingHeight = 0;
			if (req["ShippingHeight"] != null && req["ShippingHeight"] != "")
				ShippingHeight = double.Parse(req["ShippingHeight"]);

			double ShippingLength = 0;
			if (req["ShippingLength"] != null && req["ShippingLength"] != "")
				ShippingLength = double.Parse(req["ShippingLength"]);

			int Ranking = 0;
			if (req["Ranking"] != null && req["Ranking"] != "")
				Ranking = Int32.Parse(req["Ranking"]);

			int UserRating = 0;
			if (req["UserRating"] != null && req["UserRating"] != "")
				UserRating = Int32.Parse(req["UserRating"]);

			int Views = 0;
			if (req["Views"] != null && req["Views"] != "")
				Views = Int32.Parse(req["Views"]);

			UpdateItem(ItemId, req["ProductNumber"], req["Title"], req["Keywords"], BrandId, _status, req["SmallDescription"],
				WebContext.Server.HtmlDecode(req["LargeDescription"]), Price, ResellerPrice, SalePrice,
				req.Files["ThumbImage"], req["DeleteThumbImage"] == "on",
				req.Files["Image1"], req["DeleteImage1"] == "on",
				req.Files["Image2"], req["DeleteImage2"] == "on",
				req.Files["Image3"], req["DeleteImage3"] == "on",
				req.Files["Image4"], req["DeleteImage4"] == "on",
				req["Specs"], StockQuantity, ShippingWeight,ShippingVWeight, ShippingWidth, ShippingHeight, ShippingLength,
				req.Files["DownloadableFile"], req["DeleteDownloadableFile"] == "on",
				req["Warranty"], req["Packaging"], Ranking, UserRating, Views, req["Packages"], req["Website"],
				WebContext.Server.HtmlDecode(req["Template"]));

			if (ItemId > 0)
			{
				string temp = string.Format("Delete from ItemCategories  where ItemId = {0};", ItemId);
				System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
				cmd.CommandText = temp;
				base.ExecuteCommand(cmd);

				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				temp = "Insert into ItemCategories (ItemId, CategoryId) values ({0}, {1});";

				ProductsDS.CategoriesViewDataTable cats = _categoriesMgr.GetCategories("");
				foreach (DataRow v in cats.Rows)
				{
					if (req["c" + v["CategoryId"].ToString()] == "on")
						sb.Append(string.Format(temp, ItemId, v["CategoryId"]));
				}

				string str = sb.ToString();
				if (str != "")
				{
					cmd = new System.Data.SqlClient.SqlCommand();
					cmd.CommandText = str;
					base.ExecuteCommand(cmd);
				}
			}

			UpdateItemPrices(ItemId, req);

			return ItemId;
		}

		public int UpdateItem(int ItemId, string ProductNumber, string Title, string Keywords, int BrandId,
			int Status, string SmallDescription,
			string LargeDescription, decimal? Price, decimal? ResellerPrice, decimal? SalePrice,
			HttpPostedFile ThumbImage, bool DeleteThumb, HttpPostedFile Image1, bool DeleteImage1,
			HttpPostedFile Image2, bool DeleteImage2, HttpPostedFile Image3, bool DeleteImage3,
			HttpPostedFile Image4, bool DeleteImage4, string Specs, int StockQuantity, double ShippingWeight, double ShippingVWeight,
			double ShippingWidth, double ShippingHeight, double ShippingLength,
			HttpPostedFile DownloadableFile, bool DeleteDownloadable, string Warranty, string Packaging, int Ranking,
			int UserRating, int Views, string Packages, string Website, string Template)
		{
			if (Packages != null && Packages.Length > 0 && Packages != "<packages></packages>")
				Status = Status | (int)ItemStatus.Package;

			if (this.GetItem(ProductNumber) != null)
			{
				int _ItemId = GetItem(ProductNumber).ItemId;
				if (ItemId != _ItemId)
				{
					ErrorContext.Add("items", "Item " + ProductNumber + " already exists");
					return -1;
				}
			}

			ProductsDS.ItemsRow Item = this.GetItem(ItemId);

			string oldUniqueName = Item.UniqueName;

			Item.Title = Title;
			/*
			Item.UniqueName = GetUniqueName(Title, ProductNumber);

			if (oldUniqueName != Item.UniqueName)
			{
				//Rename folder
				string path = WebContext.Server.MapPath(WebContext.Root + "/" + Folders.ProductsImages);

				string oldPath = Path.Combine(path, oldUniqueName);

				path = Path.Combine(path, Item.UniqueName);

				if (Directory.Exists(oldPath))
				{
					Directory.Move(oldPath, path);
				}
			}
			*/
			Item.Keywords = Keywords;
			Item.ProductNumber = ProductNumber;
			Item.Status = Status;
			Item.SmallDescription = SmallDescription;
			Item.LargeDescription = LargeDescription;
			Item.Price = Price != null ? Price.Value : 0;
			Item.Specs = Specs;
			Item.BrandId = BrandId;
			Item.ResellerPrice = ResellerPrice != null ? ResellerPrice.Value : 0;
			Item.SalePrice = SalePrice != null ? SalePrice.Value : 0;
			Item.StockQuantity = StockQuantity;
			Item.ShippingWidth = ShippingWidth;
			Item.ShippingHeight = ShippingHeight;
			Item.ShippingLength = ShippingLength;
			Item.ShippingWeight = ShippingWeight;
			Item.ShippingVWeight = ShippingVWeight;
			Item.Warranty = Warranty;
			Item.Packaging = Packaging;
			Item.Ranking = Ranking;
			Item.UserRating = UserRating;
			Item.Views = Views;
			Item.Website = Website;
			Item.Template = Template;

			ItemsAdp adp = new ItemsAdp();
			adp.Update(Item);

			HandleItemFiles(ItemId,
				ThumbImage, DeleteThumb,
				Image1, DeleteImage1,
				Image2, DeleteImage2,
				Image3, DeleteImage3,
				Image4, DeleteImage4,
				DownloadableFile, DeleteDownloadable);

			if (Packages != null)
			{
				PackagesMgr pMgr = new PackagesMgr();
				pMgr.UpdateItemPackage(ItemId, Packages);
			}
			return ItemId;
		}


		public void IncrementViews(int ItemId)
		{
			ItemsAdp adp = new ItemsAdp();
			adp.IncrementViews(ItemId);
		}

		public bool DeleteItem(int ItemId)
		{
			ProductsDS.ItemsRow Item = GetItem(ItemId);
			HandleItemFiles(ItemId, null, true, null, true, null, true, null, true, null, true, null, true);

			ItemsAdp adp = new ItemsAdp();
			adp.DeleteItem(ItemId);

			return true;
		}

		/// <summary>
		/// Updates item custom prices
		/// </summary>
		/// <param name="ItemId">ItemId to be updated</param>
		/// <param name="req">The request object</param>
		public void UpdateItemPrices(int ItemId, HttpRequest req)
		{
			string patern = "price_option_id";
			foreach (string key in req.Form.Keys)
			{
				if (key.IndexOf(patern) == 0)
				{
					try
					{
						string[] temp = key.Split('_');
						int keyId = Int32.Parse(temp[temp.Length - 1]);

						string priceFor = req[string.Format("price_option_for_{0}", keyId)];

						decimal? price = null;
						if (!String.IsNullOrWhiteSpace(req[string.Format("price_option_price_{0}", keyId)]))
						{
							price = decimal.Parse(req[string.Format("price_option_price_{0}", keyId)]);
						}
						decimal? resellerPrice = null;
						if (!String.IsNullOrWhiteSpace(req[string.Format("price_option_reseller_price_{0}", keyId)]))
						{
							resellerPrice = decimal.Parse(req[string.Format("price_option_reseller_price_{0}", keyId)]);
						}
						bool onSale = req[string.Format("price_option_onsale_{0}", keyId)] == "on";

						decimal? salePrice = null;
						if (!String.IsNullOrWhiteSpace(req[string.Format("price_option_sale_price_{0}", keyId)]))
						{
							salePrice = decimal.Parse(req[string.Format("price_option_sale_price_{0}", keyId)]);
						}

						decimal? inventory = null;
						if (!String.IsNullOrWhiteSpace(req[string.Format("price_option_inventory_{0}", keyId)]))
						{
							inventory = decimal.Parse(req[string.Format("price_option_inventory_{0}", keyId)]);
						}
						string description = req[string.Format("price_option_description_{0}", keyId)];

						bool isDefault = req["IsDefaultPrice"] == keyId.ToString();

						ItemPricesAdp adp = new ItemPricesAdp();

						if (keyId <= 0)
						{
							adp.Insert(ItemId, priceFor, 1, price, resellerPrice, onSale, salePrice, inventory, description, isDefault);
						}
						else
						{
							if (string.IsNullOrWhiteSpace(priceFor))
							{
								adp.DeleteItemPrice(keyId);
							}
							else
							{
								adp.UpdateQuery(ItemId, priceFor, 1, price, resellerPrice, onSale, salePrice, inventory, description,
									isDefault, keyId);
							}
						}

					}
					catch (Exception ex)
					{
						ErrorContext.Add("updateitemprices", "Error at " + key);
					}
				}
			}
		}

		#endregion

		#region Items and Categories

		public void AddItemToCategory(int itemId, int categoryId)
		{
			string temp = @"if (Not Exists(select * from ItemCategories where ItemId={0} and CategoryId={1}))
								insert into ItemCategories(ItemId, CategoryId) values ({0}, {1});";

			temp = string.Format(temp, itemId, categoryId);

			DBUtils.ExecuteQuery(temp, cte.lib);
		}

		public void RemoveItemFromCategory(int itemId, int categoryId)
		{
			string temp = @"delete from ItemCategories where ItemId={0} and CategoryId={1};";

			temp = string.Format(temp, itemId, categoryId);

			DBUtils.ExecuteQuery(temp, cte.lib);
		}

		#endregion

		#region Import Items

//        public void ImportItems(HttpPostedFile ExcelFile)
//        {
//            lw.ExcelDataReader.ExcelDataReader spreadsheet = null;
//            StringBuilder output = new StringBuilder();

//            string fileName = "excel-item-import-" + DateTime.Now.Millisecond.ToString() + ".xls";
//            fileName = WebContext.Server.MapPath("~/temp/" + fileName);

//            ExcelFile.SaveAs(fileName);

//            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
//            spreadsheet = new ExcelDataReader.ExcelDataReader(fs);
//            fs.Close();

//            BrandsMgr bMgr = new BrandsMgr();
//            CategoriesMgr cMgr = new CategoriesMgr();
//            ChoicesMgr choicesMgr = new ChoicesMgr();
//            DataView brands = new DataView(bMgr.GetBrands(""));


//            foreach (DataTable table in spreadsheet.WorkbookData.Tables)
//            {
//                NameValueCollection mapping = new NameValueCollection();
//                if (table.Rows.Count > 0)
//                {
//                    for (int i = 0; i < table.Columns.Count; i++)
//                    {
//                        string colName = table.Columns[i].ColumnName;
//                        string rowHeader = table.Rows[0][colName].ToString();
//                        if (rowHeader.Trim() != "")
//                            mapping[rowHeader.Trim()] = colName;
//                    }

//                    bool start = false;
//                    foreach (DataRow dr in table.Rows)
//                    {
//                        if (!start)
//                        {
//                            start = true;
//                            continue;
//                        }

//                        int ItemId = -1;

//                        string ProductNumber = GetRowValue(mapping, dr, "ProductNumber", null);

//                        if (ProductNumber.Trim() == "")
//                            continue;

//                        ProductsDS.ItemsRow Item = GetItem(ProductNumber);
//                        if (Item != null)
//                            ItemId = Item.ItemId;

//                        string Title = GetRowValue(mapping, dr, "Title", Item);

//                        int BrandId = -1;
//                        string brandName = GetRowValue(mapping, dr, "Brand", Item);
//                        if (brandName != "")
//                        {
//                            brands.RowFilter = "Title='" + StringUtils.SQLEncode(brandName) + "'";
//                            if (brands.Count > 0)
//                                BrandId = (int)brands[0]["BrandId"];
//                            else
//                                BrandId = bMgr.AddBrand(brandName, null);
//                        }
//                        else if (Item != null)
//                            BrandId = Item.BrandId;

//                        int status = 0;
//                        string _stat = GetRowValue(mapping, dr, "Status", Item);
//                        if (_stat != "")
//                            status = _stat.ToLower() == "1" || _stat.ToLower() == "true" ? (int)ItemStatus.Enabled : (int)ItemStatus.Disabled;

//                        foreach (ItemStatus myStatus in Enum.GetValues(typeof(ItemStatus)))
//                        {
//                            string sName = myStatus.ToString();
//                            if (sName == "Enabled" || sName == "Disabled")
//                                continue;
//                            string t = GetRowValue(mapping, dr, sName, null);
//                            if (t == "1" || t.ToLower() == "true")
//                                status |= (int)myStatus;
//                        }

//                        int inventory = 0;
//                        string _inventory = GetRowValue(mapping, dr, "Inventory", Item);
//                        if (_inventory != "")
//                            inventory = Int32.Parse(_inventory);

//                        string SmallDescription = GetRowValue(mapping, dr, "SmallDescription", Item);
//                        string LargeDescription = GetRowValue(mapping, dr, "LargeDescription", Item);

//                        decimal price = 0;
//                        string _price = GetRowValue(mapping, dr, "Price", Item);
//                        if (_price != "")
//                            price = decimal.Parse(_price);

//                        decimal resellerPrice = 0;
//                        string _resellerPrice = GetRowValue(mapping, dr, "ResellerPrice", Item);
//                        if (_resellerPrice != "")
//                            resellerPrice = decimal.Parse(_resellerPrice);

//                        decimal salePrice = 0;
//                        string _salePrice = GetRowValue(mapping, dr, "SalePrice", Item);
//                        if (_salePrice != "")
//                            salePrice = decimal.Parse(_salePrice);

//                        string specs = GetRowValue(mapping, dr, "Specs", Item);

//                        double ShippingWeight = 0, ShippingWidth = 0, ShippingHeight = 0, ShippingLength = 0;

//                        string _temp = GetRowValue(mapping, dr, "ShippingWeight", Item);
//                        if (_temp != "")
//                            ShippingWeight = double.Parse(_temp);

//                        _temp = GetRowValue(mapping, dr, "ShippingWidth", Item);
//                        if (_temp != "")
//                            ShippingWidth = double.Parse(_temp);

//                        _temp = GetRowValue(mapping, dr, "ShippingHeight", Item);
//                        if (_temp != "")
//                            ShippingHeight = double.Parse(_temp);

//                        _temp = GetRowValue(mapping, dr, "ShippingLength", Item);
//                        if (_temp != "")
//                            ShippingLength = double.Parse(_temp);

//                        bool isDefault = false;
//                        string _isDefault = GetRowValue(mapping, dr, "IsDefault", null);
//                        if (_isDefault != "")
//                            isDefault = _isDefault == "1" ? true : false;


//                        string Warranty = GetRowValue(mapping, dr, "Warranty", Item);
//                        string Packaging = GetRowValue(mapping, dr, "Packaging", Item);
//                        string Website = GetRowValue(mapping, dr, "Website", Item);
//                        string Template = GetRowValue(mapping, dr, "Template", Item);

//                        string categories = GetRowValue(mapping, dr, "Categories", null);
//                        string Options = GetRowValue(mapping, dr, "Options", null);

//                        int Ranking = 0, Views = 0;

//                        _temp = GetRowValue(mapping, dr, "Ranking", Item);
//                        if (_temp != "")
//                            Ranking = int.Parse(_temp);

//                        _temp = GetRowValue(mapping, dr, "Views", Item);
//                        if (_temp != "")
//                            Views = int.Parse(_temp);


//                        if (ItemId < 0)
//                        {
//                            ItemId = AddItem(ProductNumber, Title, "",
//                                BrandId, status,
//                                SmallDescription, LargeDescription, price, resellerPrice, salePrice,
//                                null, null, null, null, null, specs,
//                                Options == "" ? inventory : 0,
//                                ShippingWeight, 0, ShippingWidth,
//                                ShippingHeight, ShippingLength,
//                                null, Warranty, Packaging, Ranking, Website, Template);
//                        }
//                        else
//                        {
//                            UpdateItem(ItemId, ProductNumber, Title, "",
//                                BrandId, status,
//                                SmallDescription, LargeDescription, price, resellerPrice, salePrice,
//                                null, false, null, false, null, false, null, false, null, false,
//                                specs,
//                                Options == "" ? inventory : 0,
//                                ShippingWeight, 0, ShippingWidth, ShippingHeight, ShippingLength,
//                                null, false, Warranty, Packaging, Ranking, 0, Views, "", Website, Template);
//                        }

//                        if (categories != "")
//                        {
//                            DataView Categories = new DataView(cMgr.GetCategories(""));

//                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
//                            string temp = @"if (Not Exists(select * from ItemCategories where ItemId={0} and CategoryId={1}))
//								insert into ItemCategories(ItemId, CategoryId) values ({0}, {1});";

//                            sb.Append("delete from Itemcategories where ItemId=" + ItemId + ";");

//                            string[] cats = categories.Split('|');

//                            foreach (string cat in cats)
//                            {
//                                string _cat = cat.Trim();
//                                Categories.RowFilter = "Name='" + StringUtils.SQLEncode(_cat) + "'";
//                                int catId;
//                                if (Categories.Count > 0)
//                                    catId = (int)Categories[0]["CategoryId"];
//                                else
//                                    catId = cMgr.AddCategory(_cat);

//                                sb.Append(string.Format(temp, ItemId, catId));
//                            }

//                            string str = sb.ToString();
//                            if (str != "")
//                            {
//                                SqlCommand cmd = new SqlCommand();
//                                cmd.CommandText = str;
//                                base.ExecuteCommand(cmd);
//                            }
//                        }
//                        if (Options != "")
//                        {
//                            string patern = "\\s*\\|\\s*";
//                            Regex r = new Regex(patern);
//                            string _options = "'" + r.Replace(Options, "','") + "'";
//                            if (_options != "''")
//                            {
//                                DataTable optionsTable = choicesMgr.GetOptions(_options);

//                                DataView options = new DataView(optionsTable, "", "OptionId", DataViewRowState.CurrentRows);

//                                StringBuilder key = new StringBuilder();

//                                string sep = "";
//                                foreach (DataRowView drv in options)
//                                {
//                                    key.Append(sep + drv["OptionId"].ToString());
//                                    sep = "|";
//                                }

//                                choicesMgr.ItemOptions(ItemId, key.ToString(), "", false);
//                                choicesMgr.UpdateOptionsInventory(ItemId, key.ToString().Replace("|", ","), inventory, isDefault);
//                            }
//                        }
//                    }
//                }
//            }
//            if (ErrorContext.Count == 0)
//                SuccessContext.Add("import", "Successfully imported " +
//                    spreadsheet.WorkbookData.Tables[0].Rows.Count.ToString() + " Items");

//            File.Delete(fileName);
//        }
		string GetRowValue(NameValueCollection mapping, DataRow dr, string columnName, ProductsDS.ItemsRow Item)
		{
			string obj = "";

			if (mapping[columnName] != null)
			{
				obj = dr[mapping[columnName]].ToString().Trim();
			}
			else
			{
				try
				{
					obj = Item != null && Item[columnName] != System.DBNull.Value ? Item[columnName].ToString() : "";
				}
				catch
				{
				}
			}
			return obj;
		}

//        public void ImportOnce(HttpPostedFile ExcelFile)
//        {
//            ExcelDataReader.ExcelDataReader spreadsheet = null;
//            StringBuilder output = new StringBuilder();

//            string fileName = "excel-item-import-" + DateTime.Now.Millisecond.ToString() + ".xls";
//            fileName = WebContext.Server.MapPath("~/temp/" + fileName);

//            ExcelFile.SaveAs(fileName);

//            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
//            spreadsheet = new ExcelDataReader.ExcelDataReader(fs);
//            fs.Close();

//            BrandsMgr bMgr = new BrandsMgr();
//            CategoriesMgr cMgr = new CategoriesMgr();
//            ChoicesMgr choicesMgr = new ChoicesMgr();

//            DataView Categories = new DataView(cMgr.GetCategories(""));

//            foreach (DataTable table in spreadsheet.WorkbookData.Tables)
//            {
//                int CategoryId = 1;
//                Categories.RowFilter = "Title='" + table.TableName + "'";
//                if (Categories.Count > 0)
//                    CategoryId = (int)Categories[0]["CategoryId"];
//                else
//                {
//                    CategoryId = cMgr.AddCategory(table.TableName, 1);
//                    Categories = new DataView(cMgr.GetCategories(""));
//                }
//                NameValueCollection mapping = new NameValueCollection();
//                if (table.Rows.Count > 0)
//                {
//                    for (int i = 0; i < table.Columns.Count; i++)
//                    {
//                        string colName = table.Columns[i].ColumnName;
//                        string rowHeader = table.Rows[0][colName].ToString();
//                        if (rowHeader.Trim() != "")
//                            mapping[rowHeader.Trim()] = colName;
//                    }

//                    bool start = false;
//                    foreach (DataRow dr in table.Rows)
//                    {
//                        if (!start)
//                        {
//                            start = true;
//                            continue;
//                        }

//                        int ItemId = -1;

//                        string ProductNumber = GetRowValue(mapping, dr, "ProductNumber", null);

//                        if (ProductNumber.Trim() == "")
//                            continue;

//                        ProductsDS.ItemsRow Item = GetItem(ProductNumber);
//                        if (Item != null)
//                            ItemId = Item.ItemId;

//                        string Title = GetRowValue(mapping, dr, "Title", Item);

//                        int BrandId = -1;
//                        string brandName = GetRowValue(mapping, dr, "Brand", Item);
//                        if (brandName != "")
//                        {
//                            ProductsDS.BrandsDataTable brands = bMgr.GetBrands("Title='" + StringUtils.SQLEncode(brandName) + "'");

//                            if (brands.Count > 0)
//                                BrandId = (int)brands[0]["BrandId"];
//                        }
//                        else if (Item != null)
//                            BrandId = Item.BrandId;

//                        int status = 0;
//                        string _stat = GetRowValue(mapping, dr, "Status", Item);
//                        if (_stat != "")
//                            status = _stat.ToLower() == "1" || _stat.ToLower() == "true" ? (int)ItemStatus.Enabled : (int)ItemStatus.Disabled;

//                        foreach (ItemStatus myStatus in Enum.GetValues(typeof(ItemStatus)))
//                        {
//                            string sName = myStatus.ToString();
//                            if (sName == "Enabled" || sName == "Disabled")
//                                continue;
//                            string t = GetRowValue(mapping, dr, sName, null);
//                            if (t == "1" || t.ToLower() == "true")
//                                status |= (int)myStatus;
//                        }

//                        int inventory = 0;
//                        string _inventory = GetRowValue(mapping, dr, "Inventory", Item);
//                        if (_inventory != "")
//                            inventory = Int32.Parse(_inventory);

//                        string SmallDescription = GetRowValue(mapping, dr, "SmallDescription", Item);
//                        string LargeDescription = GetRowValue(mapping, dr, "LargeDescription", Item);

//                        decimal price = 0;
//                        string _price = GetRowValue(mapping, dr, "Price", Item);
//                        if (_price != "")
//                            price = decimal.Parse(_price);

//                        decimal resellerPrice = 0;
//                        string _resellerPrice = GetRowValue(mapping, dr, "ResellerPrice", Item);
//                        if (_resellerPrice != "")
//                            resellerPrice = decimal.Parse(_resellerPrice);

//                        decimal salePrice = 0;
//                        string _salePrice = GetRowValue(mapping, dr, "SalePrice", Item);
//                        if (_salePrice != "")
//                            salePrice = decimal.Parse(_salePrice);

//                        string specs = GetRowValue(mapping, dr, "Specs", Item);

//                        double ShippingWeight = 0, ShippingWidth = 0, ShippingHeight = 0, ShippingLength = 0;

//                        string _temp = GetRowValue(mapping, dr, "ShippingWeight", Item);
//                        if (_temp != "")
//                        {
//                            ShippingWeight = double.Parse(_temp);
//                            ShippingWeight = ShippingWeight / 1000;
//                        }

//                        string dim = GetRowValue(mapping, dr, "ShippingDimensions", null);

//                        if (dim.Trim() != "")
//                        {
//                            string[] dims = dim.Split('x');
//                            if (dims.Length == 3)
//                            {
//                                ShippingLength = double.Parse(dims[0]);
//                                ShippingWidth = double.Parse(dims[1]);
//                                ShippingHeight = double.Parse(dims[2]);
//                            }
//                        }
//                        else
//                        {
//                            _temp = GetRowValue(mapping, dr, "ShippingWidth", Item);
//                            if (_temp != "")
//                                ShippingWidth = double.Parse(_temp);

//                            _temp = GetRowValue(mapping, dr, "ShippingHeight", Item);
//                            if (_temp != "")
//                                ShippingHeight = double.Parse(_temp);

//                            _temp = GetRowValue(mapping, dr, "ShippingLength", Item);
//                            if (_temp != "")
//                                ShippingLength = double.Parse(_temp);
//                        }

//                        bool isDefault = false;
//                        string _isDefault = GetRowValue(mapping, dr, "IsDefault", null);
//                        if (_isDefault != "")
//                            isDefault = _isDefault == "1" ? true : false;


//                        string Warranty = GetRowValue(mapping, dr, "Warranty", Item);
//                        string Packaging = GetRowValue(mapping, dr, "Packaging", Item);
//                        string Website = GetRowValue(mapping, dr, "Website", Item);
//                        string Template = GetRowValue(mapping, dr, "Template", Item);

//                        string categories = GetRowValue(mapping, dr, "Categories", null);
//                        string Options = GetRowValue(mapping, dr, "Options", null);

//                        int Ranking = 0, Views = 0;

//                        _temp = GetRowValue(mapping, dr, "Ranking", Item);
//                        if (_temp != "")
//                            Ranking = int.Parse(_temp);

//                        _temp = GetRowValue(mapping, dr, "Views", Item);
//                        if (_temp != "")
//                            Views = int.Parse(_temp);


//                        if (ItemId < 0)
//                        {
//                            ItemId = AddItem(ProductNumber, Title, "",
//                                BrandId, status,
//                                SmallDescription, LargeDescription, price, resellerPrice, salePrice,
//                                null, null, null, null, null, specs,
//                                Options == "" ? inventory : 0,
//                                ShippingWeight, 0, ShippingWidth,
//                                ShippingHeight, ShippingLength,
//                                null, Warranty, Packaging, Ranking, Website, Template);
//                        }
//                        else
//                        {
//                            UpdateItem(ItemId, ProductNumber, Title, "",
//                                BrandId, status,
//                                SmallDescription, LargeDescription, price, resellerPrice, salePrice,
//                                null, false, null, false, null, false, null, false, null, false,
//                                specs,
//                                Options == "" ? inventory : 0,
//                                ShippingWeight, 0, ShippingWidth, ShippingHeight, ShippingLength,
//                                null, false, Warranty, Packaging, Ranking, 0, Views, "", Website, Template);
//                        }

//                        if (categories != "")
//                        {
//                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
//                            string temp = @"if (Not Exists(select * from ItemCategories where ItemId={0} and CategoryId={1}))
//								insert into ItemCategories(ItemId, CategoryId) values ({0}, {1});";

//                            sb.Append("delete from Itemcategories where ItemId=" + ItemId + ";");

//                            string[] cats = categories.Split('|');

//                            foreach (string cat in cats)
//                            {
//                                string _cat = cat.Trim();
//                                Categories.RowFilter = "Title='" + StringUtils.SQLEncode(_cat) + "'";
//                                int catId;
//                                if (Categories.Count > 0)
//                                    catId = (int)Categories[0]["CategoryId"];
//                                else
//                                {
//                                    catId = cMgr.AddCategory(_cat, CategoryId);
//                                    Categories = new DataView(cMgr.GetCategories(""));
//                                }
//                                sb.Append(string.Format(temp, ItemId, catId));
//                            }

//                            string str = sb.ToString();
//                            if (str != "")
//                            {
//                                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
//                                cmd.CommandText = str;
//                                base.ExecuteCommand(cmd);
//                            }
//                        }

//                        string color = GetRowValue(mapping, dr, "Color", null);
//                        string size = GetRowValue(mapping, dr, "Size", null);

//                        string[] colors = color.Split(new char[] { '/', '-', '|', ',' });
//                        string[] sizes = size.Split(new char[] { '/', '-', '|', ',' });

//                        foreach (string c in colors)
//                        {
//                            string _c = c.Trim();
//                            if (_c == "")
//                                continue;

//                            choicesMgr.ItemOptionsforImport(ItemId, choicesMgr.AddOption(1, _c).ToString(), false);
//                        }
//                        foreach (string c in sizes)
//                        {
//                            string _c = c.Trim();
//                            if (_c == "")
//                                continue;

//                            choicesMgr.ItemOptionsforImport(ItemId, choicesMgr.AddOption(2, _c).ToString(), false);
//                        }
//                        choicesMgr.CreateInvntoryRecords(ItemId);
//                    }
//                }
//            }
//            if (ErrorContext.Count == 0)
//                SuccessContext.Add("import", "Successfully imported " +
//                    spreadsheet.WorkbookData.Tables[0].Rows.Count.ToString() + " Items");

//            File.Delete(fileName);
//        }

	
		#endregion

		#region Tools
		public void HandleItemFiles(int ItemId,
			HttpPostedFile ThumbImage, bool DeleteThumb,
			HttpPostedFile Image1, bool DeleteImage1,
			HttpPostedFile Image2, bool DeleteImage2,
			HttpPostedFile Image3, bool DeleteImage3,
			HttpPostedFile Image4, bool DeleteImage4,
			HttpPostedFile DownloadableFile, bool DeleteDownloadable)
		{
			ProductsDS.ItemsRow Item = GetItem(ItemId);
			if (Item == null)
				return;

			Config cfg = new Config();


			Dimension ThumbSize = new Dimension(cfg.GetKey(Settings.ProductImage_ThumbSize));
			Dimension MediumSize = new Dimension(cfg.GetKey(Settings.ProductImage_MediumSize));
			Dimension LargeSize = new Dimension(cfg.GetKey(Settings.ProductImage_LargeSize));


			string filesPath = WebContext.Server.MapPath(WebContext.Root + lw.CTE.Folders.ProductsFiles);
			if (DeleteDownloadable && Item.DownloadableFile != "")
			{
				//check if this file is used by another item, is so we don't delete it
				if (GetItems(string.Format("ItemId<>{0} and DownloadableFile='{1}'",
					ItemId, DownloadableFile)).Rows.Count == 0 && File.Exists(filesPath + "/" + Item.DownloadableFile))
					File.Delete(filesPath + "/" + Item.DownloadableFile);
				Item.DownloadableFile = "";
			}

			if (DownloadableFile != null && DownloadableFile.ContentLength > 0)
			{
				string ext = StringUtils.GetFileExtension(DownloadableFile.FileName);
				string file = StringUtils.GetFriendlyFileName(DownloadableFile.FileName) + "_" + Item.ItemId.ToString() + "." + ext;

				string filePath = WebContext.Server.MapPath(WebContext.Root + Folders.ProductsFiles);

				if (!Directory.Exists(filePath))
				{
					Directory.CreateDirectory(filePath);
				}

				filePath = filePath + "/" + file;

				DownloadableFile.SaveAs(filePath);

				Item.DownloadableFile = file;
			}

			string path = WebContext.Server.MapPath(WebContext.Root + "/" + Folders.ProductsImages);

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			path = Path.Combine(path, Item.UniqueName);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);

				Directory.CreateDirectory(Path.Combine(path, "Large"));
				Directory.CreateDirectory(Path.Combine(path, "Thumb"));
				Directory.CreateDirectory(Path.Combine(path, "Medium"));
			}

			string PartName = Item.UniqueName;
			if (PartName.Length >= 35)
				PartName = PartName.Substring(0, 35);

			Item.ThumbImage = CreateItemImage(ThumbImage, DeleteThumb, Item, ThumbSize, MediumSize, LargeSize, path, PartName, Item.ThumbImage, "thumb");
			Item.Image1 = CreateItemImage(Image1, DeleteImage1, Item, ThumbSize, MediumSize, LargeSize, path, PartName, Item.Image1, "1");
			Item.Image2 = CreateItemImage(Image2, DeleteImage2, Item, ThumbSize, MediumSize, LargeSize, path, PartName, Item.Image2, "2");
			Item.Image3 = CreateItemImage(Image3, DeleteImage3, Item, ThumbSize, MediumSize, LargeSize, path, PartName, Item.Image3, "3");
			Item.Image4 = CreateItemImage(Image4, DeleteImage4, Item, ThumbSize, MediumSize, LargeSize, path, PartName, Item.Image4, "4");

			ItemsAdp adp = new ItemsAdp();
			adp.UpdateFiles(Item.Image1, Item.Image2, Item.Image3, Item.Image4, Item.DownloadableFile, DateTime.Now, Item.ThumbImage, ItemId);
		}


		public string CreateItemImage(HttpPostedFile Image, bool DeleteOld,
			ProductsDS.ItemsRow Item,
			Dimension ThumbSize, Dimension MediumSize, Dimension LargeSize,
			string path, string PartName, string OldImage, string ImageNbr)
		{
			string ImageName = OldImage;

			if (Image != null && Image.ContentLength > 0)
			{
				if (!DeleteOld)
				{
					if (GetItems(string.Format("ItemId<>{0} and (ThumbImage='{1}' or Image1='{1}' or Image2='{1}' or Image3='{1}' or Image4='{1}')",
						Item.ItemId, OldImage)).Rows.Count == 0)
					{
						if (System.IO.File.Exists(path + "/Thumb/" + OldImage))
							System.IO.File.Delete(path + "/Thumb/" + OldImage);

						if (System.IO.File.Exists(path + "/Large/" + OldImage))
							System.IO.File.Delete(path + "/Large/" + OldImage);

						if (System.IO.File.Exists(path + "/Medium/" + OldImage))
							System.IO.File.Delete(path + "/Medium/" + OldImage);
					}
				}

				string extension = lw.Utils.StringUtils.GetFileExtension(Image.FileName);
				ImageName = PartName + "-" + ImageNbr + "-" + Item.ItemId.ToString() + "." + extension;
				string Large = path + "/Large/" + ImageName;

				Image.SaveAs(Large);

				return CreateItemImage(Large, DeleteOld, Item, ThumbSize, MediumSize, LargeSize, path, PartName, OldImage, ImageNbr);
			}
			else
			{
				if (DeleteOld)
				{
					if (GetItems(string.Format("ItemId<>{0} and (ThumbImage='{1}' or Image1='{1}' or Image2='{1}' or Image3='{1}' or Image4='{1}')",
						Item.ItemId, OldImage)).Rows.Count == 0)
					{
						if (System.IO.File.Exists(path + "/Thumb/" + OldImage))
							System.IO.File.Delete(path + "/Thumb/" + OldImage);

						if (System.IO.File.Exists(path + "/Large/" + OldImage))
							System.IO.File.Delete(path + "/Large/" + OldImage);

						if (System.IO.File.Exists(path + "/Medium/" + OldImage))
							System.IO.File.Delete(path + "/Medium/" + OldImage);
					}

					return "";
				}
			}
			return OldImage;
		}


		public string CreateItemImage(string ImagePath, bool DeleteOld,
			ProductsDS.ItemsRow Item,
			Dimension ThumbSize, Dimension MediumSize, Dimension LargeSize,
			string path, string PartName, string OldImage, string ImageNbr)
		{
			string ImageName = OldImage;

			if (DeleteOld && OldImage != "")
			{
				//check if image used by other items
				if (GetItems(string.Format("ItemId<>{0} and (ThumbImage='{1}' or Image1='{1}' or Image2='{1}' or Image3='{1}' or Image4='{1}')",
					Item.ItemId, OldImage)).Rows.Count == 0)
				{
					if (System.IO.File.Exists(path + "/Thumb/" + OldImage))
						System.IO.File.Delete(path + "/Thumb/" + OldImage);

					if (System.IO.File.Exists(path + "/Large/" + OldImage))
						System.IO.File.Delete(path + "/Large/" + OldImage);

					if (System.IO.File.Exists(path + "/Medium/" + OldImage))
						System.IO.File.Delete(path + "/Medium/" + OldImage);

					ImageName = "";
				}
			}

			FileInfo Image = new FileInfo(ImagePath);

			if (Image.Exists && Image.Length > 0)
			{
				/*
				if (!DeleteOld)
				{
					if (GetItems(string.Format("ItemId<>{0} and (ThumbImage='{1}' or Image1='{1}' or Image2='{1}' or Image3='{1}' or Image4='{1}')",
						Item.ItemId, OldImage)).Rows.Count == 0)
					{
						if (System.IO.File.Exists(path + "/Thumb/" + OldImage))
							System.IO.File.Delete(path + "/Thumb/" + OldImage);

						if (System.IO.File.Exists(path + "/Large/" + OldImage))
							System.IO.File.Delete(path + "/Large/" + OldImage);

						if (System.IO.File.Exists(path + "/Medium/" + OldImage))
							System.IO.File.Delete(path + "/Medium/" + OldImage);
					}
				}
				 * */

				string extension = lw.Utils.StringUtils.GetFileExtension(Image.FullName);
				ImageName = PartName + "-" + ImageNbr + "-" + Item.ItemId.ToString() + "." + extension;
				string Large = path + "/Large/" + ImageName;
				string Thumb = path + "/Thumb/" + ImageName;
				string medium = path + "/Medium/" + ImageName;

				if (File.Exists(Large) && Large.ToLower() != ImagePath.ToLower())
				{
					File.Delete(Large);
					File.Delete(Thumb);
					File.Delete(medium);
				}
				try
				{
					Image.CopyTo(Large, true);
				}
				catch
				{

				}
				if (ThumbSize.Valid)
					lw.GraphicUtils.ImageUtils.Resize(Large, Thumb, ThumbSize.IntWidth, ThumbSize.IntHeight);
					//lw.GraphicUtils.ImageUtils.CreateThumb(Large, Thumb, ThumbSize.IntWidth, ThumbSize.IntHeight, true);
				else
					File.Copy(Large, Thumb, true);

				if (MediumSize.Valid)
					lw.GraphicUtils.ImageUtils.Resize(Large, medium, MediumSize.IntWidth, MediumSize.IntHeight);
				else
					File.Copy(Large, Thumb, true);

				if (LargeSize.Valid)
					lw.GraphicUtils.ImageUtils.Resize(Large, Large, LargeSize.IntWidth, LargeSize.IntHeight);
			}
			return ImageName;
		}

		#endregion
	}
}