using System;
using System.Collections.Specialized;
using System.Data;
using System.Threading;
using System.Web.UI;
using lw.Base;
using lw.DataControls;
using lw.Utils;
using lw.WebTools;

namespace lw.Products.Controls
{


	public class ItemsSource : CustomDataSource
	{
		public static ReaderWriterLock _lock = new ReaderWriterLock();

		string _cond = "", _category = "", _brand = "";
		int _max = -1;
		ItemStatus _Status = ItemStatus.Enabled;
		ListItemsBy _listBy = ListItemsBy.All;

		int _packageItemId = -1;

		bool _bound = false;

		string _itemIds = "";

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


		public ItemsSource()
		{
			this.DataLibrary = lw.Products.cte.lib;
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			lw.Base.CustomPage page = this.Page as lw.Base.CustomPage;

			if (WebContext.Request["Search"] == "1")
			{
				ListBy = ListItemsBy.Search;
			}

			switch (ListBy)
			{
				case ListItemsBy.Brand:
					if (Brand == "")
						_brand = page.GetQueryValue("Brand");
					break;
				case ListItemsBy.Category:
					if (String.IsNullOrWhiteSpace(Category))
						_category = page.GetQueryValue("Category");
					if (String.IsNullOrWhiteSpace(SubCategory))
						_subCategory = page.GetQueryValue("SubCategory");
					break;
				case ListItemsBy.Package:
					
					object obj = DataBinder.Eval(this.NamingContainer, "DataItem.ItemId");
					if (obj == null)
					{
						string uniqueName = page.GetQueryValue("Item");
						if (uniqueName != "")
						{
							ProductsDS.ItemsRow Item = (new ItemsMgr()).GetItemByUniqueName(uniqueName);
							_packageItemId = Item.ItemId;
						}
					}
					else
					{
						_packageItemId = (int)obj;
					}

					break;
				case ListItemsBy.Search:
					Cond += string.Format(" and (ProductNumber like '%{0}%' or Title like '%{0}%' or Keywords like '%{0}%')", StringUtils.SQLEncode(page.GetQueryValue("q")));
					break;
				case ListItemsBy.All:
				default:
					break;
			}

			
			string categoryCondition = "";

			if (!String.IsNullOrWhiteSpace(Category))
			{
				categoryCondition = string.Format("(Name='{0}' or Title='{0}')", 
					StringUtils.SQLEncode(Category));
			}
			if (!String.IsNullOrWhiteSpace(SubCategory))
			{
				categoryCondition += (categoryCondition != ""? " or " : "") + 
					string.Format("(Name='{0}' or Title='{0}')", StringUtils.SQLEncode(SubCategory));
			}

			string moreConditions = "";

			if (!String.IsNullOrWhiteSpace(categoryCondition))
			{
				CategoriesMgr cMgr = new CategoriesMgr();
				DataTable cat = cMgr.GetCategories(categoryCondition);

				string cats = "", sep = "";

				if (cat.Rows.Count > 0)
				{

					foreach (DataRow row in cat.Rows)
					{
						cats += sep + row["CategoryId"].ToString();
						sep = ",";
					}
					Cond += string.Format(" and CategoryId in ({0})", cats);
				}

				if (!String.IsNullOrWhiteSpace(SubCategory))
				{
					moreConditions = @"Join (select ItemId, Count(*) as _Count from ItemCategories where CategoryId in (" + cats + ")  group by ItemId) x on s.ItemId = x.ItemId where x._Count >= 2";
				}
			}
			

			if (!String.IsNullOrWhiteSpace(Brand))
			{
				BrandsMgr bMgr = new BrandsMgr();
				DataRow brand = bMgr.GetBrand(Brand);
				if (brand != null)
				{
					Cond += string.Format(" and BrandId={0}", brand["BrandId"]);
				}
			}

			if (BrandId != null)
			{
				Cond += string.Format(" and BrandId={0}", BrandId);
			}

			if (_packageItemId > 0)
			{
				Cond += string.Format(" and ItemId in (select ItemId1 from ItemPackages where ItemId={0})", _packageItemId);
			}

			string _status = page.GetQueryValue("status");
			if (null != _status && _status != "")
			{
				Status = (ItemStatus)Enum.Parse(typeof(ItemStatus), _status);
			}
			Cond += string.Format(" and Status&{0}<>0", (int)Status);
			if(Status == ItemStatus.OnSale)
				Cond += string.Format(" and Status&{0}=0", (int)ItemStatus.Package);

			SelectCommand = string.Format(@"Select Distinct top 100 percent s.ItemId,s.ProductNumber,s.Title,s.BrandId,s.Status,s.Price,s.Image1,s.Image2, 
s.Image3,s.Image4,s.ThumbImage,s.ResellerPrice,s.DownloadableFile,s.Ranking,s.UserRating,s.Views,s.SalePrice,s.UniqueName,s.Inventory, s.Brand 
from (select top {0} 
* from itemsview ",
					_max == -1 ? "100 PERCENT" : _max.ToString());


			if (!String.IsNullOrWhiteSpace(WebContext.Request["ByPrice"]))
			{
				string temp = WebContext.Request["ByPrice"].Replace(";", "").Replace("<=", "Price <=").Replace(">=", "Price >=")
					.Replace("< ", "Price < ").Replace("> ", "Price > ");
				Cond += " and " + temp;
			}


			if (Cond != "")
			{
				System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("^\\s*and");
				SelectCommand += " where " + r.Replace(Cond, "");
			}
			SelectCommand += ") s " + moreConditions + " order by s.ItemId";

			//WebContext.Response.Write(SelectCommand);
			//WebContext.Response.End();

			base.DataBind();
		}


		int? _brandId;
		public int? BrandId
		{
			get
			{
				string obj = MyPage.GetQueryValue("BrandId");
				if (!string.IsNullOrWhiteSpace(obj))
					_brandId = int.Parse(obj);

				return _brandId;
			}
			set
			{
				_brandId = value;
			}
		}

		int? _categoryId;
		public int? CategoryId
		{
			get
			{
				string obj = MyPage.GetQueryValue("CategoryId");
				if (!string.IsNullOrWhiteSpace(obj))
					_brandId = int.Parse(obj);

				return _categoryId;
			}
			set
			{
				_categoryId = value;
			}
		}


		bool _dataSet = false;
		DataTable data;
		public override object Data
		{
			get
			{
				if (!_dataSet)
				{
					data = base.Data as DataTable;

					try
					{
						_lock.AcquireWriterLock(-1);

						


						//Prevent duplicates
						NameValueCollection existingItems = new NameValueCollection();

						for (int i = 0; i < data.Rows.Count; i++)
						{
							DataRow row = data.Rows[i];
							if (existingItems[row["ItemId"].ToString()] != null)
							{
								row.Delete();
								//i--;
								continue;
							}
							existingItems[row["ItemId"].ToString()] = "true";

							_itemIds += string.Format("{0},", row["ItemId"]);
						}

						if (thisPage.PageContext[cte.ItemIdsContext] != null)
							thisPage.PageContext[cte.ItemIdsContext] = "";
						thisPage.PageContext[cte.ItemIdsContext] = string.Format("{0}{1}", thisPage.PageContext[cte.ItemIdsContext], _itemIds);
					}
					finally
					{
						_dataSet = true;
						_lock.ReleaseWriterLock();
					}
				}
				return data;
			}
			set
			{
				base.Data = value;
			}
		}

		public string Cond
		{
			get
			{
				return _cond;
			}
			set
			{
				_cond = value;
			}
		}
		public string Brand
		{
			get { return _brand; }
			set { _brand = value; }
		}
		public string Category
		{
			get
			{
				if (String.IsNullOrWhiteSpace(_category))
					_category =  MyPage.GetQueryValue("Category");
	
				return _category;
			}
			set
			{
				_category = value;
			}
		}

		string _subCategory = null;
		public string SubCategory
		{
			get
			{
				return _subCategory;
			}
			set
			{
				_subCategory = value;
			}
		}
		public int Max
		{
			get
			{
				return _max;
			}
			set
			{
				_max = value;
			}
		}
		public ItemStatus Status
		{
			get
			{
				return _Status;
			}
			set
			{
				_Status = value;
			}
		}
		public ListItemsBy ListBy
		{
			get { return _listBy; }
			set { _listBy = value; }
		}


		/// <summary>
		/// Returns all the item ids fetched by this data source separated by comma ","
		/// </summary>
		public string ItemIds
		{
			get
			{
				if (!_bound)
					DataBind();

				//invokes get data
				object obj = Data;

				return _itemIds;
			}
		}
	}
}