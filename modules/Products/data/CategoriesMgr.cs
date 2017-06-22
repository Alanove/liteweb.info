using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using lw.CTE;
using lw.Data;
using lw.Utils;
using lw.WebTools;

namespace lw.Products
{
	public class CategoriesMgr : DirectorBase
	{
		public CategoriesMgr():base(cte.lib)
		{

		}

		/// <summary>
		/// returns all root categories
		/// </summary>
		/// <returns></returns>
		public ProductsDS.CategoriesViewDataTable GetRootCategories()
		{
			return GetCategories(string.Format("ParentId={0}", -1));
		}

		public ProductsDS.CategoriesDataTable _GetCategories(string cond)
		{
			CategoriesAdp adp = new CategoriesAdp();
			return adp.GetCategories(cond);
		}

		public ProductsDS.CategoriesViewDataTable GetCategories(string cond)
		{
			CategoriesViewAdp adp = new CategoriesViewAdp();
			return adp.GetCategories(cond);
		}

		/// <summary>
		/// Gets All Ancestor Categories
		/// </summary>
		/// <param name="CategoryId">Category</param>
		/// <returns>
		/// Table (CategoryId, Name, Title, ParentId, Level)
		/// </returns>
		public DataTable GetCategoryAncestors(int CategoryId)
		{
			return DBUtils.GetDataSet(string.Format("select * from dbo.Categories_GetAncestors({0})", CategoryId), cte.lib).Tables[0];
		}

		/// <summary>
		/// Gets All Ancestor Categories
		/// </summary>
		/// <param name="CategoryId">Category</param>
		/// <returns>
		/// Table (CategoryId, Name, Title, ParentId, Level)
		/// </returns>
		public DataTable GetCategoryAncestors(ProductsDS.CategoriesRow cat)
		{
			if (cat == null)
				return null;
			return GetCategoryAncestors(cat.CategoryId);
		}

		/// <summary>
		/// Gets All Descendant or OffSpring Categories
		/// </summary>
		/// <param name="CategoryId">Category</param>
		/// <returns>
		/// Table (CategoryId, Name, Title, ParentId, Level)
		/// </returns>
		public DataTable GetCategoryDescendants(ProductsDS.CategoriesRow cat)
		{
			if (cat == null)
				return null;
			return GetCategoryDescendants(cat.CategoryId);
		}

		/// <summary>
		/// Gets All Descendant or OffSpring Categories
		/// </summary>
		/// <param name="CategoryId">Category</param>
		/// <returns>
		/// Table (CategoryId, Name, Title, ParentId, Level)
		/// </returns>
		public DataTable GetCategoryDescendants(int CategoryId)
		{
			return DBUtils.GetDataSet(string.Format("select * from dbo.Categories_GetDescendants({0})", CategoryId), cte.lib).Tables[0];
		}


		/// <summary>
		/// returns all Descendant or OffSpring Items
		/// </summary>
		/// <param name="CategoryId"></param>
		/// <returns></returns>
		public DataTable GetAllDescendantItems(int CategoryId)
		{
			SqlCommand procedure = DBUtils.StoredProcedure("Items_GetCategoryDescendants", cte.lib);
			DBUtils.AddProcedureParameter(procedure, "@CategoryId", SqlDbType.Int, CategoryId, ParameterDirection.Input);

			return DBUtils.GetDataSet(procedure, cte.lib).Tables[0];
		}

		public ProductsDS.CategoriesRow GetCategory(int CategoryId)
		{
			ProductsDS.CategoriesDataTable cats = _GetCategories(string.Format("CategoryId={0}", CategoryId));

			if (cats.Rows.Count > 0)
			{
				DataView dv = cats.DefaultView;
				dv.Sort = "SortingOrder ASC";

				return (ProductsDS.CategoriesRow)dv[0].Row;
			}
			return null;
		}

		public ProductsDS.CategoriesRow GetCategory(string CategoryName)
		{
			ProductsDS.CategoriesDataTable cats = _GetCategories(string.Format("Name='{0}' or Title='{0}'", StringUtils.SQLEncode(CategoryName)));
			if (cats.Count > 0)
			{
				DataView dv = cats.DefaultView;
				dv.Sort = "SortingOrder ASC";

				return (ProductsDS.CategoriesRow)dv[0].Row;
			}
			return null;
		}

		public ProductsDS.CategoriesRow GetCategory(string CategoryName, int ParentId)
		{
			ProductsDS.CategoriesDataTable cats = _GetCategories(string.Format("Name='{0}' and ParentId={1}", 
				StringUtils.SQLEncode(CategoryName), ParentId));
			if (cats.Count > 0)
				return (ProductsDS.CategoriesRow)cats.Rows[0];

			return null;
		}
		public ProductsDS.CategoriesViewDataTable GetChildrenCategories(int CategoryId, string cond)
		{
			if (cond != "")
				cond = " And " + cond;
			ProductsDS.CategoriesViewDataTable cats = GetCategories(string.Format("ParentId={0}{1}", CategoryId, cond));
			
			return cats;
		}
		public ProductsDS.CategoriesViewDataTable GetChildrenCategories(int CategoryId)
		{
			return GetChildrenCategories(CategoryId, "");
		}
		public ProductsDS.CategoriesViewDataTable GetChildrenCategories(int CategoryId, int ItemId)
		{
			ProductsDS.CategoriesViewDataTable cats = GetCategories(
				string.Format("ParentId={0} And CategoryId not in (select CategoryId from ItemCategories where ItemId={1})", 
					CategoryId, ItemId)
			);
			return cats;
		}
		public ProductsDS.CategoriesViewDataTable GetChildrenCategories(string CategoryName)
		{
			return GetChildrenCategories(CategoryName, -1);
		}

		public ProductsDS.CategoriesViewDataTable GetChildrenCategories(string CategoryName, int ParentId)
		{
			ProductsDS.CategoriesViewDataTable cats = this.GetCategories(
				string.Format("ParentId in (select CategoryId from Categories where Title='{0}' and ParentId={1})", 
					lw.Utils.StringUtils.SQLEncode(CategoryName), ParentId)
			);
			return cats;
		}

		public ProductsDS.CategoriesViewRow GetCategoryRowView(int CategoryId)
		{
			ProductsDS.CategoriesViewDataTable cats = GetCategories(string.Format("CategoryId={0}", CategoryId));
			if (cats.Count > 0)
				return (ProductsDS.CategoriesViewRow)cats.Rows[0];

			return null;
		}

		public ProductsDS.CategoriesViewDataTable GetParentCategories(int CategoryId)
		{
			ArrayList ar = new ArrayList();
			while (CategoryId != -1)
			{
				ProductsDS.CategoriesViewRow cat = this.GetCategoryRowView(CategoryId);
				if (cat != null)
					ar.Add(cat);
				CategoryId = cat.ParentId;
			}
			ProductsDS.CategoriesViewDataTable dt = new ProductsDS.CategoriesViewDataTable();
			for (int i = ar.Count - 1; i >= 0; i--)
				dt.Rows.Add((ProductsDS.CategoriesRow)ar[i]);
			return dt;
		}

		public int AddCategory(string Name)
		{
			return AddCategory(Name, -1);
		}
		public int AddCategory(string Name, int ParentId)
		{
			Name = StringUtils.CapitilizeFirstChar(Name.ToLower());
			return AddCategory(Name, Name, 0, true, null, "", ParentId, -1);
		}

		public int AddCategory(string UniqueName, string Title, int SortingOrder, bool status,
			HttpPostedFile Image,
			string Description, int ParentId, int ClassId)
		{
			ProductsDS _ds = new ProductsDS();
			
			DataTable cats = this.GetCategories(string.Format("(Title='{0}' and ParentId={1}) or Name='{2}'", Title, ParentId, UniqueName));
			if (cats.Rows.Count > 0)
				return (int)cats.Rows[0]["CategoryId"];

			UniqueName = StringUtils.ToURL(UniqueName);

			CategoriesAdp adp = new CategoriesAdp();

			adp.Insert(UniqueName, StringUtils.SQLEncode(Title), status, "", Description, ParentId, null, DateTime.Now, DateTime.Now, null, null, SortingOrder);

			ProductsDS.CategoriesRow cat = GetCategory(UniqueName);

			int CategoryId = cat.CategoryId;

			string PartName = lw.Utils.StringUtils.ToURL(Title);

			if (PartName.Length >= 35)
				PartName = PartName.Substring(0, 35);

			if (Image != null && Image.ContentLength > 0)
			{
				string pathTo = WebContext.Root + lw.CTE.Folders.CategoriesImages;

				pathTo = WebContext.Server.MapPath(pathTo);

				if (!Directory.Exists(pathTo))
					Directory.CreateDirectory(pathTo);
				if (!Directory.Exists(Path.Combine(pathTo, "thumb")))
					Directory.CreateDirectory(Path.Combine(pathTo, "thumb"));
				if (!Directory.Exists(Path.Combine(pathTo, "large")))
					Directory.CreateDirectory(Path.Combine(pathTo, "large"));

				string extension = lw.Utils.StringUtils.GetFileExtension(Image.FileName);
				string ImageName = PartName + "_" + CategoryId.ToString() + "." + extension;
				string Large = pathTo + "/large/" + ImageName;
				string Thumb = pathTo + "/thumb/" + ImageName;

				Image.SaveAs(Large);

				Config cfg = new Config();
				Dimension dim = new Dimension(cfg.GetKey(Settings.CategoryImagesSize));
				if (dim.Valid)
				{
					lw.GraphicUtils.ImageUtils.Resize(Large, Large, dim.IntWidth, dim.IntHeight);
				}
				adp.UpdateImage(ImageName, CategoryId);
			}

			return CategoryId;
		}

		public int UpdateCategory(int CategoryId, string UniqueName, string Title, int SortingOrder,
			bool status, HttpPostedFile Image, bool DeleteImage,
			string Description, int ParentId, int ClassId)
		{
			ProductsDS _ds = new ProductsDS();
			//if (this.GetCategories(string.Format("(Title='{0}' and ParentId={1} and CategoryId <> {3}) or (Name='{2}' and CategoryId <> {3})", Title, ParentId, UniqueName, CategoryId)).Count > 0)
			//	return -1;

			ProductsDS.CategoriesRow row = GetCategory(CategoryId);

			CategoriesAdp adp = new CategoriesAdp();

			UniqueName = StringUtils.ToURL(UniqueName);

			row.Name = UniqueName;
			row.Title = Title;
			row.SortingOrder = SortingOrder;
			row.Status = status;
			row.Description = Description;
			row.ParentId = ParentId;
			row.ClassId = ClassId;
			row.LastModified = DateTime.Now;

			string path = WebContext.Server.MapPath(WebContext.Root + lw.CTE.Folders.CategoriesImages);
			
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			if (!Directory.Exists(Path.Combine(path, "Thumb")))
				Directory.CreateDirectory(Path.Combine(path, "Thumb"));
			if (!Directory.Exists(Path.Combine(path, "Large")))
				Directory.CreateDirectory(Path.Combine(path, "Large"));

			if (DeleteImage && row.Image != "")
			{
				if (this.GetCategories("Image='" + row.Image + "' and CategoryId<>" + row.CategoryId.ToString()).Count == 0)
				{
					if (System.IO.File.Exists(path + "/Thumb/" + row.Image))
						System.IO.File.Delete(path + "/Thumb/" + row.Image);
					if (System.IO.File.Exists(path + "/Large/" + row.Image))
						System.IO.File.Delete(path + "/Large/" + row.Image);

				}
				row.Image = "";
			}

			string PartName = lw.Utils.StringUtils.ToURL(Title);

			if (PartName.Length >= 35)
				PartName = PartName.Substring(0, 35);

			if (Image != null && Image.ContentLength > 0)
			{
				if (!DeleteImage)
				{
					if (System.IO.File.Exists(path + "/Thumb/" + row.Image))
						System.IO.File.Delete(path + "/Thumb/" + row.Image);
					if (System.IO.File.Exists(path + "/Large/" + row.Image))
						System.IO.File.Delete(path + "/Large/" + row.Image);
				}

				string extension = lw.Utils.StringUtils.GetFileExtension(Image.FileName);
				string ImageName = PartName + "_" + row.CategoryId + "." + extension;
				string Large = path + "/Large/" + ImageName;
				string Thumb = path + "/Thumb/" + ImageName;

				Image.SaveAs(Large);

				Config cfg = new Config();
				Dimension dim = new Dimension(cfg.GetKey(Settings.CategoryImagesSize));
				if (dim.Valid)
				{
					lw.GraphicUtils.ImageUtils.CreateThumb(Large, Thumb, dim.IntWidth, dim.IntHeight, true);
				}
				row.Image = ImageName;
			}

			adp.Update(row);

			return row.CategoryId;
		}

		public bool DeleteCategory(object catId)
		{
			int cId = Int32.Parse(catId.ToString());
			return DeleteCategory(cId);
		}
		public bool DeleteCategory(int CategoryId)
		{
			ProductsDS.CategoriesViewDataTable cats = this.GetChildrenCategories(CategoryId);
			foreach (DataRow cat in cats)
				DeleteCategory((Int32)cat["CategoryId"]);

			
			ProductsDS.CategoriesRow category = GetCategory(CategoryId);

			string path = WebContext.Server.MapPath(WebContext.Root + lw.CTE.Folders.CategoriesImages);

			if (category["Image"] != System.DBNull.Value)
			{
				if (category.Image != "")
				{
					if (this.GetCategories("Image='" + category.Image + "' and CategoryId<>" + CategoryId.ToString()).Count == 0)
					{
						if (System.IO.File.Exists(path + "/Large/" + category.Image))
							System.IO.File.Delete(path + "/Large/" + category.Image);
					}
				}
			}

			CategoriesAdp adp = new CategoriesAdp();
			adp.DeleteCategory(CategoryId);
			
			return true;
		}
	}
}
