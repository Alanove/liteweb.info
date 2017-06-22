using System;
using System.Data;
using System.Web.UI;
using lw.CTE;
using lw.WebTools;
using System.Text.RegularExpressions;

namespace lw.Products.Controls
{
	public class Categories : lw.DataControls.CustomRepeater
	{
		bool _bound = false;
		int _categoryId = -1;
		CategoryType _type = CategoryType.Regular;
		int _brandId = -1;
		string _parent = "";
		string _categoryUniqueName = "";
		lw.Base.CustomPage _page;


		CategoriesMgr pMgr = new CategoriesMgr();

		public Categories()
		{
		}
		public override void DataBind()
		{
			if (_bound)
				return;

			_page = this.Page as lw.Base.CustomPage;

			if (this.CategoryId > 0 || this._brandId > 0 || this.Type == CategoryType.Search)
			{
				string q = _page.GetQueryValue("q");


				DataView cats = null;
				string search = "1=1";

				if(!String.IsNullOrWhiteSpace(_boundTo))
				{
					ItemsSource source = _page.FindControlRecursive(_boundTo) as ItemsSource;
					if (source != null)
					{
						string items = source.ItemIds;
						if (!String.IsNullOrWhiteSpace(items))
							search += " and CategoryId in (select CategoryId from ItemCategories where ItemId in (" + items.TrimEnd(',') +"))";
					}
				}
				if (q != null && this.Type == CategoryType.Search && q != "")
				{
					search += string.Format(" and (Name like '%{0}%' or Title like '%{0}%' or Description like '%{0}%')", lw.Utils.StringUtils.SQLEncode(q));
				}
				else
				{
					if (this._brandId == -1)
					{
						cats = pMgr.GetChildrenCategories(CategoryId, search).DefaultView;
					}
					else
					{
						search += string.Format(" and CategoryId in (Select CategoryId from ItemCategories Where ItemId in (Select ItemId from Items where BrandId={0})) And ParentId <> -1",
							_brandId);
					}
				}

				if (cats == null)
				{
					cats = pMgr.GetCategories(search).DefaultView;
				}
				cats.RowFilter = "Status=1";
				cats.Sort = "SortingOrder";
				this.DataSource = cats;

				base.DataBind();

				for (int i = 0; i < this.Controls.Count; i++)
				{
					if ((this.Controls[i] as Categories) != null)
						this.Controls[i].DataBind();
				}
			}
		}
		public int CategoryId
		{
			get
			{
				if (_categoryId == -1)
				{
					object obj = null;
					switch (this.Type)
					{
						case CategoryType.Hierarchy:
							obj = ControlUtils.GetBoundedDataField(this.NamingContainer, "CategoryId");
							if (obj != null)
								_categoryId = (int)obj;
							break;
						case CategoryType.Regular:
							obj = _page.GetQueryValue("CategoryId");
							if (obj != null && obj.ToString() != "")
								_categoryId = Int32.Parse(obj.ToString());
							else
							{
								obj = _page.GetQueryValue("Category");
								if (obj != null && obj.ToString() != "")
								{
									ProductsDS.CategoriesRow cat = pMgr.GetCategory(obj.ToString());
									if(cat != null)
										CategoryId = cat.CategoryId;
								}
							}
							break;
						case CategoryType.RootByBrand:
							obj = _page.GetQueryValue("BrandId");
							if (obj != null && obj.ToString() != "")
							{
								this._brandId = Int32.Parse(obj.ToString());
								this.CategoryId = -1;
								break;
							}
							goto case CategoryType.Root;
						case CategoryType.Root:
							Config cfg = new Config();
							string MainCategory = cfg.GetKey(Settings.MainCategory);
							if (MainCategory != null && MainCategory != "")
								_categoryId = (int)pMgr.GetCategory(MainCategory)["CategoryId"];
							break;
						case CategoryType.Parent:
							if (_parent != "")
							{
								DataRow p = pMgr.GetCategory(Parent);
								if (p != null)
									_categoryId = (int)p["CategoryId"];
							}
							break;
						case CategoryType.Category:
							if (_categoryUniqueName != "")
							{
								DataRow dr = pMgr.GetCategory(string.Format("{0}", CategoryUniqueName));
								_categoryId = (int)dr["CategoryId"];
							}
							break;
					}
				}
				return _categoryId;
			}
			set
			{
				_categoryId = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			object obj = this.DataSource;
			if (obj != null && ((DataView)obj).Count > 0)
				base.Render(writer);
		}


		public string Parent
		{
			get
			{
				return _parent;
			}
			set
			{
				_parent = value;
			}
		}
		public CategoryType Type
		{
			get
			{
				return Parent != "" ? CategoryType.Parent : _type;
			}
			set
			{
				_type = value;
			}
		}
		public string CategoryUniqueName
		{
			get
			{
				return _categoryUniqueName;
			}
			set
			{
				_categoryUniqueName = value;
			}
		}

		string _boundTo = "";
		/// <summary>
		/// Bounds the tag to an Items DataSource
		/// The categories returned by this source will definetly have items from this data source
		/// Good for using filters
		/// </summary>
		public string BoundTo
		{
			get
			{
				return _boundTo;
			}
			set
			{
				_boundTo = value;
			}
		}
	}
}