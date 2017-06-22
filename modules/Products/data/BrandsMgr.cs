using System;
using System.Data;
using lw.CTE;
using lw.Data;
using lw.Utils;
using lw.WebTools;


namespace lw.Products
{
	public class BrandsMgr : DirectorBase
	{
		public BrandsMgr()
			: base(cte.lib)
		{
		}
		public ProductsDS.BrandsDataTable GetBrands(string cond)
		{
			BrandsAdp adp = new BrandsAdp();
			return adp.GetBrands(cond);
		}

		public ProductsDS.BrandsRow GetBrand(int BrandId)
		{
			DataTable dt = GetBrands(string.Format("BrandId = {0}", BrandId));
			if(dt.Rows.Count > 0)
				return (ProductsDS.BrandsRow)dt.Rows[0];
			return null;
		}
		public ProductsDS.BrandsRow GetBrand(string Title)
		{
			DataTable dt = GetBrands(string.Format("Title='{0}'", StringUtils.SQLEncode(Title)));
			if(dt.Rows.Count > 0)
				return (ProductsDS.BrandsRow)dt.Rows[0];
			return null;
		}

		public int AddBrand(string Title, System.Web.HttpPostedFile Image)
		{
			BrandsAdp adp = new BrandsAdp();
			adp.Insert(Title, "", DateTime.Now, DateTime.Now);

			int BrandId = GetBrand(Title).BrandId;

			string PartName = lw.Utils.StringUtils.ToURL(Title);

			if (PartName.Length >= 35)
				PartName = PartName.Substring(0, 35);

			if (Image != null && Image.ContentLength > 0)
			{
				string pathTo = WebContext.Root + lw.CTE.Folders.BrandsImages;
				pathTo = WebContext.Server.MapPath(pathTo);

				string extension = lw.Utils.StringUtils.GetFileExtension(Image.FileName);
				string ImageName = PartName + "_" + BrandId + "." + extension;
				string Large = pathTo + "/" + ImageName;

				Image.SaveAs(Large);

				Config cfg = new Config();
				Dimension dim = new Dimension(cfg.GetKey(Settings.BrandsImagesSize));
				if (dim.Valid)
				{
					lw.GraphicUtils.ImageUtils.Resize(Large, Large, dim.IntWidth, dim.IntHeight);
				}
				adp.UpdateImage(ImageName, BrandId);
			}

			return BrandId;
		}

		public int UpdateBrand(int BrandId, string Title, bool DeleteImage,
			System.Web.HttpPostedFile Image)
		{
			ProductsDS _ds = new ProductsDS();
			if (GetBrands(string.Format("Title='{0}' and BrandId <> {1}", Title, BrandId)).Count > 0)
				return -1;

			ProductsDS.BrandsRow row = GetBrand(BrandId);


			row.Title = Title;
			row.LastModified = DateTime.Now;

			string path = WebContext.Server.MapPath(WebContext.Root + lw.CTE.Folders.BrandsImages);

			if (DeleteImage && row.Image != "")
			{
				if (GetBrands("Image='" + row.Image + "' and BrandId<>" + BrandId.ToString()).Count == 0)
				{
					if (System.IO.File.Exists(path + "/" + row.Image))
						System.IO.File.Delete(path + "/" + row.Image);
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
					if (GetBrands("Image='" + row.Image + "' and BrandId<>" + BrandId.ToString()).Count == 0)
					{
						if (System.IO.File.Exists(path + "/" + row.Image))
							System.IO.File.Delete(path + "/" + row.Image);
					}
				}

				string extension = lw.Utils.StringUtils.GetFileExtension(Image.FileName);
				string ImageName = PartName + "_" + row.BrandId + "." + extension;
				string Large = path + "/" + ImageName;

				Image.SaveAs(Large);

				Config cfg = new Config();

				Dimension dim = new Dimension(cfg.GetKey(Settings.BrandsImagesSize));
				if (dim.Valid)
				{
					lw.GraphicUtils.ImageUtils.Resize(Large, Large, dim.IntWidth, dim.IntHeight);
				}

				row.Image = ImageName;
			}

			BrandsAdp adp = new BrandsAdp();
			adp.Update(row);

			return BrandId;
		}
		public bool DeleteBrand(int BrandId)
		{
			ProductsDS.BrandsRow brand = this.GetBrand(BrandId);

			string path = WebContext.Server.MapPath(WebContext.Root + lw.CTE.Folders.BrandsImages);

			BrandsAdp adp = new BrandsAdp();
			adp.DeleteBrand(BrandId);

			if (brand.Image != "")
			{
				if (GetBrands("Image='" + brand.Image + "' and BrandId<>" + BrandId.ToString()).Count == 0)
				{
					if (System.IO.File.Exists(path + "/" + brand.Image))
						System.IO.File.Delete(path + "/" + brand.Image);
				}
			}

			return true;
		}
	}
}
