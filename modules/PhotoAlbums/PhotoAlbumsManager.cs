using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using lw.CTE.Enum;
using lw.Data;
using lw.GraphicUtils;
using lw.Utils;
using lw.WebTools;


namespace lw.PhotoAlbums
{
	public class PhotoAlbumsManager : DirectorBase
	{
		public PhotoAlbumsManager()
			: base(cte.lib)
		{
		}

		public static string GetImagesPath(int AlbumId)
		{
			string path = WebContext.Root + "/" + CTE.Folders.PhotoAlbums;
			if (AlbumId > 0)
				path = string.Format("{0}/Album{1}", path, AlbumId);
			return path;
		}

		public static string GetImagesPath()
		{
			return GetImagesPath(-1);
		}

		public DataView GetPhotoAlbumCategories(string cond)
		{
			AlbumsDS _ds = new AlbumsDS();
			IDataAdapter Adp = base.GetAdapter(cte.PhotoAlbumCategoriesAdp);
			return ((AlbumsDS)base.FillData(Adp, _ds, cond)).PhotoAlbumsCategories.DefaultView;
		}

		public DataRow GetCategoriesByName(string name)
		{
			DataView cats = GetPhotoAlbumCategories(string.Format("CategoryName='{0}' or UniqueName='{0}'", StringUtils.SQLEncode(name)));
			if (cats.Count > 0)
				return cats[0].Row;
			return null;
		}

		public int AddCategory(string name)
		{
			AlbumsDS _ds = new AlbumsDS();

			IDataAdapter Adp = base.GetAdapter(cte.PhotoAlbumCategoriesAdp);
			AlbumsDS.PhotoAlbumsCategoriesRow row = _ds.PhotoAlbumsCategories.NewPhotoAlbumsCategoriesRow();

			row.CategoryName = name;
			row.UniqueName = StringUtils.ToURL(name);

			_ds.PhotoAlbumsCategories.AddPhotoAlbumsCategoriesRow(row);
			base.UpdateData(Adp, _ds);

			return row.CategoryId;
		}

		public DataView GetPhotoAlbums(string cond)
		{
			string sql = "select * from AlbumsView";

			if (!String.IsNullOrEmpty(cond))
				sql += " where " + cond;

			return new DataView(DBUtils.GetDataSet(sql, cte.lib).Tables[0], "", "sort asc, DateModified desc", DataViewRowState.CurrentRows);
		}


		public DataView GetTopPhotoAlbums(string Top, string cond)
		{
			if (string.IsNullOrEmpty(Top))
				Top = "100 PERCENT";

			string sql = string.Format("select top {0} * from AlbumsView", Top);

			if (!String.IsNullOrEmpty(cond))
				sql += " where " + cond;

			return new DataView(DBUtils.GetDataSet(sql, cte.lib).Tables[0], "", "sort asc, DateModified desc", DataViewRowState.CurrentRows);
		}


		public DataView GetPhotoRelatedAlbum(string cond, string tableName)
		{
			string sql = "select * from " + tableName;

			if (!String.IsNullOrEmpty(cond))
				sql += " where " + cond;

			return new DataView(DBUtils.GetDataSet(sql, cte.lib).Tables[0], "", "", DataViewRowState.CurrentRows);
		}
		/// <summary>
		/// Get the List of Albums that are differentiated by their categories.
		/// List each album in which categories exists, only one category in a row.
		/// </summary>
		/// <param name="cond"></param>
		/// <returns></returns>
		public DataView GetAllPhotoAlbums(string cond)
		{
			string sql = string.Format("Select * FROM AlbumsFullView");
			if (!StringUtils.IsNullOrWhiteSpace(cond))
				sql += " where " + cond;
			return DBUtils.GetDataSet(sql, cte.lib).Tables[0].DefaultView;
		}

		/// <summary>
		/// Returns the top (number) of photo albums unsorted, essential if you want just one random photo album.
		/// </summary>
		/// <param name="top">number of returned photo albums</param>
		/// <returns></returns>
		public DataView GetRandomPhotoAlbums(string top)
		{
			string sql = "Select top {0} *, newid() as ___Ran from AlbumsView order by ___Ran Desc";

			sql = string.Format(sql, top == "" ? "100 PERCENT" : top);

			return DBUtils.GetDataSet(sql, cte.lib).Tables[0].DefaultView;
		}

		/// <summary>
		/// Returns DataRow containing database projection of the Album info
		/// </summary>
		/// <param name="AlbumId">The Photo Album ID</param>
		/// <returns></returns>
		public DataRow GetAlbumDetails(int AlbumId)
		{
			DataView albums = GetPhotoAlbums(string.Format("Id={0}", AlbumId));
			return albums.Count > 0 ? albums[0].Row : null;
		}

		/// <summary>
		/// Returns DataRow containing database projection of the Album info
		/// </summary>
		/// <param name="albumName">The Photo Album Unique Name</param>
		/// <returns></returns>
		public DataRow GetAlbumDetails(string albumName)
		{
			DataView albums = GetPhotoAlbums(string.Format("Name=N'{0}'", StringUtils.SQLEncode(albumName)));
			return albums.Count > 0 ? albums[0].Row : null;
		}

		/// <summary>
		/// Gets all the photo albums in the specified category
		/// </summary>
		/// <param name="Category">CategoryId</param>
		/// <returns></returns>
		public DataView GetPhotoAlbums(int Category)
		{
			return GetPhotoAlbums(string.Format("CategoryId&{0}={0}", Math.Pow(2, Category)));
		}

		/// <summary>
		/// Gets all the photo albums in the specified category
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="status">either return enabled or disabled photo albums</param>
		/// <returns></returns>
		/// <seealso cref="GetPhotoAlbums"/>
		public DataView GetPhotoAlbums(int Category, bool status)
		{
			return GetPhotoAlbums(string.Format("CategoryId&{0}={0} and status={1}", Math.Pow(2, Category), status ? 1 : 0));
		}

		/// <summary>
		/// Gets all the photo albums in the specified category
		/// </summary>
		/// <param name="status">either return enabled or disabled photo albums</param>
		/// <returns></returns>
		/// <seealso cref="GetPhotoAlbums"/>
		public DataView GetPhotoAlbums(bool status)
		{
			return GetPhotoAlbums(string.Format("status={0}", status ? 1 : 0));
		}


		/// <summary>
		/// Returns photo albums related to the specified category
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="Status"></param>
		/// <param name="Language"></param>
		/// <returns></returns>
		public DataView GetPhotoAlbumsByCategory(string Category, bool Status, Languages Language)
		{
			DataRow category = GetCategoriesByName(Category);
			if (category == null)
				return null;

			return GetPhotoAlbums(string.Format(@"
				CategoryId&{0}={0} and 
				status={1} and (Language={2} or Language={3})",
				Math.Pow(2, (int)category["CategoryId"]),
				Status ? 1 : 0,
				1,
				(int)Languages.Default
			));
		}

		/// <summary>
		/// Returns photo albums related to the specified category
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="Status"></param>
		/// <returns></returns>
		public DataView GetPhotoAlbumsByCategory(string Category, bool Status)
		{
			DataRow category = GetCategoriesByName(Category);
			if (category == null)
				return null;

			return GetPhotoAlbums(string.Format(@"
				CategoryId&{0}={0} and 
				status={1}",
				Math.Pow(2, (int)category["CategoryId"]),
				Status ? 1 : 0
			));
		}

		/// <summary>
		/// Gets all the photo albums in the specified language
		/// </summary>
		/// <param name="status">either return enabled or disabled photo albums</param>
		/// <param name="language">Language</param>
		/// <returns></returns>
		/// <seealso cref="GetPhotoAlbums"/>
		public DataView GetPhotoAlbums(bool status, Languages language)
		{
			return GetPhotoAlbums(string.Format("status={0} and (Language={1} or Language={2})",
				status ? 1 : 0,
				(int)language,
				(int)Languages.Default
			));
		}

		/// <summary>
		/// Returns list of images available in the photo Albums database
		/// </summary>
		/// <param name="cond"></param>
		/// <returns></returns>
		public DataView GetImages(string cond)
		{
			AlbumsDS _ds = new AlbumsDS();
			IDataAdapter Adp = base.GetAdapter(cte.PhotoAlbumImagesAdp);
			DataView images = ((AlbumsDS)base.FillData(Adp, _ds, cond)).PhotoAlbumImages.DefaultView;
			images.Sort = "sort asc";

			DataTable dt = images.Table;


			dt.Columns.Add("Thumb");
			dt.Columns.Add("Large");
			dt.Columns.Add("Original");

			if (dt.Rows.Count > 0)
			{
				int albumId = (int)dt.Rows[0]["AlbumId"];
				string path = PhotoAlbumsManager.GetImagesPath(albumId);

				foreach (DataRow dr in dt.Rows)
				{
					dr["Thumb"] = string.Format("{0}/Thumb/{1}", path, dr["FileName"]);
					dr["Large"] = string.Format("{0}/Large/{1}", path, dr["FileName"]);
					dr["Original"] = string.Format("{0}/Original/{1}", path, dr["FileName"]);
				}
			}

			dt.AcceptChanges();

			return dt.DefaultView;
		}
		public DataView GetImages(int AlbumId)
		{
			string cond = string.Format("AlbumId={0}", AlbumId);
			return GetImages(cond);
		}
		public int AddAlbum(System.Web.HttpRequest req, int Category)
		{
			return AddAlbum(req, Category, false);
		}

		public int AddAlbum(System.Web.HttpRequest req, int Category, bool FixedSize)
		{
			Languages lan = Languages.Default;

			if (!String.IsNullOrWhiteSpace(req["Language"]))
			{
				lan = (Languages)Enum.Parse(typeof(Languages), req["Language"]);
			}

			DateTime date;
			DateTime? _date = null;

			bool dateCorrect = false;

			if (!String.IsNullOrEmpty(req["Date"]))
			{
				dateCorrect = DateTime.TryParse(req["Date"], out date);
				if (dateCorrect)
					_date = date;
			}

			return AddAlbum("", req.Form["DisplayName"],
				req.Form["Description"],
				req.Form["Status"] == "on",
				Int32.Parse(req.Form["Sort"]),
				req.Form["HasIntroPage"] == "on",
				Category,
				req.Files["Image"], lan, _date, FixedSize);
		}

		/// <summary>
		/// Create a disabled photo album by providing just the name
		/// </summary>
		/// <param name="Name">The Album Name</param>
		/// <returns></returns>
		public int AddAlbum(string Name)
		{
			return AddAlbum(null, Name, null, false, 10000, false, -1, null, Languages.Default, null, false);
		}

		/// <summary>
		/// Create a disabled photo album by providing just the name and category
		/// </summary>
		/// <param name="Name">The Album Name</param>
		/// <returns></returns>
		public int AddAlbum(string Name, int CategoryId)
		{
			return AddAlbum(null, Name, null, false, 10000, false, CategoryId, null, Languages.Default, null, false);
		}


		public int AddAlbum(string Name, string DisplayName,
			string Description,
			bool status, int Sort,
			bool HasIntroPage,
			int CategoryId, HttpPostedFile Image,
			Languages lan, DateTime? AlbumDate, bool FixedSize)
		{
			AlbumsDS _ds = new AlbumsDS();

			IDataAdapter Adp = base.GetAdapter(cte.PhotoAlbumsAdp);
			AlbumsDS.PhotoAlbumsRow row = _ds.PhotoAlbums.NewPhotoAlbumsRow();

			row.Name = StringUtils.ToURL(DisplayName);
			row.DisplayName = DisplayName;
			row.Description = Description;
			row.Status = status;
			row.Sort = Sort;
			row.HasIntroPage = HasIntroPage;
			row.CategoryId = CategoryId;
			row.DateAdded = DateTime.Now;
			row.DateModified = DateTime.Now;
			row.Language = (short)lan;

			if (AlbumDate != null)
				row.AlbumDate = AlbumDate.Value;

			_ds.PhotoAlbums.AddPhotoAlbumsRow(row);
			base.UpdateData(Adp, _ds);

			string path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.PhotoAlbums);
			path = System.IO.Path.Combine(path, string.Format("Album{0}", row.Id));


			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			Directory.CreateDirectory(Path.Combine(path, "Large"));
			Directory.CreateDirectory(Path.Combine(path, "Thumb"));
			Directory.CreateDirectory(Path.Combine(path, "Original"));

			if (Image != null && Image.ContentLength > 0)
			{
				Config cfg = new Config();

				string fileName = StringUtils.GetFileName(Image.FileName);

				string ImageName = string.Format("{0}\\{1}", path, fileName);
				string largeImageName = string.Format("{0}\\Large_{1}", path, fileName);
				string thumbImageName = string.Format("{0}\\Thumb_{1}", path, fileName);

				Image.SaveAs(ImageName);

				try
				{
					Dimension largeImageSize = new Dimension(cte.CoverPhotoDefaultSize);
					Dimension thumbImageSize = new Dimension(cte.CoverPhotoDefaultThumbSize);

					if (!string.IsNullOrWhiteSpace(cfg.GetKey(cte.CoverPhotoSize)))
						largeImageSize = new Dimension(cfg.GetKey(cte.CoverPhotoSize));
					if (!string.IsNullOrWhiteSpace(cfg.GetKey(cte.CoverPhotoThumbSize)))
						thumbImageSize = new Dimension(cfg.GetKey(cte.CoverPhotoThumbSize));


					////ImageUtils.Resize(ImageName, ImageName, _Width, _Height);
					//if (FixedSize)
					//    lw.GraphicUtils.ImageUtils.CreateThumb(ImageName, largeImageName, largeImageSize.IntWidth, largeImageSize.IntHeight, FixedSize);
					//else
					//    lw.GraphicUtils.ImageUtils.CreateThumb(ImageName, largeImageName, largeImageSize.IntWidth, largeImageSize.IntHeight);


					////TODO: add a parameter to choose between the below
					////lw.GraphicUtils.ImageUtils.Resize(ImageName, thumbImageName, Width, Height);
					//lw.GraphicUtils.ImageUtils.CreateThumb(ImageName, thumbImageName, thumbImageSize.IntWidth, thumbImageSize.IntHeight, FixedSize);

					lw.GraphicUtils.ImageUtils.Resize(ImageName, largeImageName, largeImageSize.IntWidth, largeImageSize.IntHeight);
					lw.GraphicUtils.ImageUtils.CropImage(ImageName, thumbImageName, thumbImageSize.IntWidth, thumbImageSize.IntHeight, ImageUtils.AnchorPosition.Default);
				}
				catch (Exception ex)
				{
					ErrorContext.Add("resize-image", "Unable to resize album image.<br><span class=hid>" + ex.Message + "</span>");
				}


				row.Image = fileName;
				base.UpdateData(Adp, _ds);
			}

			return row.Id;
		}
		public bool UpdateAlbum(int AlbumId, string AlbumName)
		{
			try
			{
				AlbumsDSTableAdapters.PhotoAlbumsTableAdapter adp = new AlbumsDSTableAdapters.PhotoAlbumsTableAdapter();
				DataRow album = this.GetPhotoAlbums(string.Format("Id={0}", AlbumId))[0].Row;
				album["Name"] = StringUtils.ToURL(AlbumName);
				album["DisplayName"] = AlbumName;
				adp.Update(album);
			}
			catch (Exception e)
			{
				return false;
			}
			return true;
		}

		public bool UpdateAlbum(int AlbumId, System.Web.HttpRequest req, int Category)
		{
			Languages lan = Languages.Default;

			if (!String.IsNullOrWhiteSpace(req["Language"]))
			{
				lan = (Languages)Enum.Parse(typeof(Languages), req["Language"]);
			}

			DateTime date;
			DateTime? _date = null;

			bool dateCorrect = false;

			if (!String.IsNullOrEmpty(req["Date"]))
			{
				dateCorrect = DateTime.TryParse(req["Date"], out date);
				if (dateCorrect)
					_date = date;
			}

			return UpdateAlbum(AlbumId, "", req.Form["DisplayName"],
				req.Form["Description"],
				req.Form["Status"] == "on",
				Int32.Parse(req.Form["Sort"]),
				req.Form["HasIntroPage"] == "on",
				Category,
				req.Form["DeleteImage"] == "on",
				req.Files["Image"], lan, _date);
		}
		public bool UpdateAlbum(int AlbumId, string Name, string DisplayName,
			string Description,
			bool status, int Sort,
			bool HasIntroPage, int CategoryId,
			bool DeleteImage, HttpPostedFile Image, Languages lan, DateTime? AlbumDate)
		{
			AlbumsDS _ds = new AlbumsDS();

			AlbumsDSTableAdapters.PhotoAlbumsTableAdapter adp = new AlbumsDSTableAdapters.PhotoAlbumsTableAdapter();

			DataRow album = this.GetPhotoAlbums(string.Format("Id={0}", AlbumId))[0].Row;

			album["Name"] = StringUtils.ToURL(DisplayName);
			album["DisplayName"] = DisplayName;
			album["Description"] = Description;
			album["Status"] = status;
			album["Sort"] = Sort;
			album["HasIntroPage"] = HasIntroPage;
			album["CategoryId"] = CategoryId;
			album["DateAdded"] = (DateTime)album["DateAdded"];
			album["DateModified"] = DateTime.Now;
			album["Image"] = album["Image"].ToString();
			album["Language"] = (short)lan;
			if (AlbumDate != null)
				album["AlbumDate"] = AlbumDate.Value;

			adp.Update(album);


			string path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.PhotoAlbums);
			path = System.IO.Path.Combine(path, string.Format("Album{0}", album["Id"]));

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);

				Directory.CreateDirectory(Path.Combine(path, "Original"));
				Directory.CreateDirectory(Path.Combine(path, "Large"));
				Directory.CreateDirectory(Path.Combine(path, "Thumb"));
			}


			bool del = (DeleteImage || (Image != null && Image.ContentLength > 0))
						&& album["Image"].ToString() != "";

			bool updateData = false;
			if (DeleteImage)
			{
				string fileName = Path.Combine(path, album["Image"].ToString());
				if (File.Exists(fileName))
					File.Delete(fileName);


				fileName = Path.Combine(path, "Large_" + album["Image"].ToString());
				if (File.Exists(fileName))
					File.Delete(fileName);

				fileName = Path.Combine(path, "Thumb_" + album["Image"].ToString());
				if (File.Exists(fileName))
					File.Delete(fileName);

				album["Image"] = "";
				updateData = true;
			}

			//if (!DeleteImage && Image != null && Image.ContentLength > 0)
			if (Image != null && Image.ContentLength > 0)
			{
				Config cfg = new Config();

				string fileName = StringUtils.GetFileName(Image.FileName);

				string ImageName = string.Format("{0}\\{1}", path, fileName);
				string largeImageName = string.Format("{0}\\Large_{1}", path, fileName);
				string thumbImageName = string.Format("{0}\\Thumb_{1}", path, fileName);

				Image.SaveAs(ImageName);

				if (cfg.GetKey("AlbumImage") == "on")
				{
					try
					{
						Dimension largeImageSize = new Dimension(cte.CoverPhotoDefaultSize);
						Dimension thumbImageSize = new Dimension(cte.CoverPhotoDefaultThumbSize);

						if (!string.IsNullOrWhiteSpace(cfg.GetKey(cte.CoverPhotoSize)))
							largeImageSize = new Dimension(cfg.GetKey(cte.CoverPhotoSize));
						if (!string.IsNullOrWhiteSpace(cfg.GetKey(cte.CoverPhotoThumbSize)))
							thumbImageSize = new Dimension(cfg.GetKey(cte.CoverPhotoThumbSize));

						lw.GraphicUtils.ImageUtils.Resize(ImageName, largeImageName, largeImageSize.IntWidth, largeImageSize.IntHeight);
						lw.GraphicUtils.ImageUtils.CropImage(ImageName, thumbImageName, thumbImageSize.IntWidth, thumbImageSize.IntHeight, ImageUtils.AnchorPosition.Default);
					}
					catch (Exception ex)
					{
						ErrorContext.Add("resize-image", "Unable to resize album image.<br><span class=hid>" + ex.Message + "</span>");
					}
				}

				album["Image"] = fileName;
				updateData = true;
			}

			if (updateData)
				adp.Update(album);

			return true;
		}

		public void UpdateAlbumStatus(int AlbumId, bool status)
		{
			string sql = string.Format("Update PhotoAlbums set Status={1} where Id={0}",
				AlbumId, status ? 1 : 0);

			DBUtils.ExecuteQuery(sql, cte.lib);
		}
		public void UpdateAlbumModified(int AlbumId, DateTime date)
		{
			string sql = string.Format("Update PhotoAlbums set DateModified='{1}' where Id={0}",
				AlbumId, date.ToString());

			DBUtils.ExecuteQuery(sql, cte.lib);
		}

		public void UpdateImage(int ImageId, string Caption)
		{
			string sql = string.Format("Update PhotoAlbumImages set  Caption='{0}' where Id={1}",
				StringUtils.SQLEncode(Caption), ImageId);

			DBUtils.ExecuteQuery(sql, cte.lib);
		}

		/// <summary>
		/// Deletes a PhotoAlbum from the database and all its related images
		/// TODO: Delete photo album relations
		/// </summary>
		/// <param name="AlbumId">AlbumId</param>
		/// <returns>true on success</returns>
		public bool DeleteAlbum(int AlbumId)
		{

			//disable the album, in case something went wrong
			UpdateAlbumStatus(AlbumId, false);

			string path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.PhotoAlbums);

			path = System.IO.Path.Combine(path, string.Format("Album{0}", AlbumId));

			AlbumsDS _ds = new AlbumsDS();

			string sql = String.Format("Delete from PhotoAlbums where Id={0}", AlbumId);
			DBUtils.ExecuteQuery(sql, cte.lib);

			if (Directory.Exists(path))
				Directory.Delete(path, true);


			return true;
		}

		/// <summary>
		/// Deletes a PhotoAlbum from the database and all its related images
		/// TODO: Delete photo album relations
		/// </summary>
		/// <param name="AlbumName">AlbumName</param>
		/// <returns>true on success</returns>
		public bool DeleteAlbum(string AlbumName)
		{
			DataRow album = GetAlbumDetails(AlbumName);

			if (album != null)
			{
				return DeleteAlbum((int)album["Id"]);
			}
			return false;
		}

		public DataRow AddImage(HttpRequest req)
		{
			return AddImage(Int32.Parse(req.Form["AlbumId"]),
				req.Form["Caption"],
				req.Files["Image"]);
		}



		public DataRow AddImage(int AlbumId, string Caption, string fileName, Stream BytesStream)
		{
			if (BytesStream.Length == 0)
			{
				return null;
			}

			if (!Directory.Exists(WebContext.Server.MapPath("~/temp")))
			{
				Directory.CreateDirectory(WebContext.Server.MapPath("~/temp"));
			}
			string path = WebContext.Server.MapPath("~/temp");
			path = Path.Combine(path, fileName);


			lw.Utils.IO.SaveStream(BytesStream, path);

			DataRow row = AddImage(AlbumId, Caption, path);
			File.Delete(path);
			return row;
		}

		public DataRow AddImage(int AlbumId, string Caption, HttpPostedFile Image)
		{
			if (Image == null || Image.ContentLength <= 0)
				return null;

			if (!Directory.Exists(WebContext.Server.MapPath("~/temp")))
			{
				Directory.CreateDirectory(WebContext.Server.MapPath("~/temp"));
			}
			string path = WebContext.Server.MapPath("~/temp");
			path = Path.Combine(path, Image.FileName);


			Image.SaveAs(path);

			DataRow row = AddImage(AlbumId, Caption, path);
			File.Delete(path);
			return row;

			/*

			AlbumsDS _ds = new AlbumsDS();

			IDataAdapter Adp = base.GetAdapter(cte.PhotoAlbumImagesAdp);
			AlbumsDS.PhotoAlbumImagesRow row = _ds.PhotoAlbumImages.NewPhotoAlbumImagesRow();

			row.AlbumId = AlbumId;
			row.Caption = Caption;
			row.Sort = 100;
			row.DateAdded = DateTime.Now;
			row.DateModified = DateTime.Now;

			_ds.PhotoAlbumImages.AddPhotoAlbumImagesRow(row);
			base.UpdateData(Adp, _ds);

			string path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.PhotoAlbums);
			path = System.IO.Path.Combine(path, string.Format("Album{0}", AlbumId));
			string largePath = Path.Combine(path, "Large");
			string thumbPath = Path.Combine(path, "Thumb");

			if (!Directory.Exists(thumbPath))
				Directory.CreateDirectory(thumbPath);

			if (!Directory.Exists(largePath))
				Directory.CreateDirectory(largePath);

			Config cfg = new Config();

			string ext = StringUtils.GetFileExtension(Image.FileName);
			string fileName = StringUtils.GetFriendlyFileName(Image.FileName);

			string _ImageName = string.Format("{0}_{1}.{2}", fileName, row.Id, ext);

			string ImageName = string.Format("{0}\\{1}", largePath, _ImageName);
			string thumbImageName = string.Format("{0}\\{1}", thumbPath, _ImageName);

			Image.SaveAs(ImageName);
			try
			{
				if (cfg.GetKey("AlbumsResizeLarge") == "on")
				{
					int _Width = Int32.Parse(cfg.GetKey("AlbumsLargeWidth"));
					int _Height = Int32.Parse(cfg.GetKey("AlbumsLargeHeight"));

					lw.GraphicUtils.ImageUtils.Resize(ImageName, ImageName, _Width, _Height);
				}
				if (cfg.GetKey("AlbumResizeThumbs") == "on")
				{
					int Width = Int32.Parse(cfg.GetKey("AlbumsThumbWidth"));
					int Height = Int32.Parse(cfg.GetKey("AlbumsThumbHeight"));

					//TODO: add a parameter to choose between the below
					//lw.GraphicUtils.ImageUtils.Resize(ImageName, thumbImageName, Width, Height);
					//lw.GraphicUtils.ImageUtils.CreateThumb(ImageName, thumbImageName, Width, Height);
					lw.GraphicUtils.ImageUtils.CropImage(ImageName, thumbImageName, Width, Height, ImageUtils.AnchorPosition.Default);
				}
				else
					File.Copy(ImageName, thumbImageName);
			}
			catch (Exception ex)
			{
				ErrorContext.Add("resize-image", "Unable to resize album image.<br><span class=hid>" + ex.Message + "</span>");
			}

			row.FileName = _ImageName;
			base.UpdateData(Adp, _ds);

			UpdateAlbumModified(AlbumId, DateTime.Now);

			return row;
			 * */
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="AlbumId"></param>
		/// <param name="Caption"></param>
		/// <param name="image"></param>
		/// <returns></returns>
		/// 
		/*
		<Parameters>
			<key>PhotoAlbum_Thumb_Image_Option</key>
			<value>Resize</value> or <value>Crop</value>
		</Parameters>
		*/

		public DataRow AddImage(int AlbumId, string Caption, string image)
		{
			AlbumsDS _ds = new AlbumsDS();

			IDataAdapter Adp = base.GetAdapter(cte.PhotoAlbumImagesAdp);
			AlbumsDS.PhotoAlbumImagesRow row = _ds.PhotoAlbumImages.NewPhotoAlbumImagesRow();

			row.AlbumId = AlbumId;
			row.Caption = Caption;
			row.Sort = 100;
			row.DateAdded = DateTime.Now;
			row.DateModified = DateTime.Now;

			_ds.PhotoAlbumImages.AddPhotoAlbumImagesRow(row);
			base.UpdateData(Adp, _ds);

			string path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.PhotoAlbums);
			path = System.IO.Path.Combine(path, string.Format("Album{0}", AlbumId));
			string largePath = Path.Combine(path, "Large");
			string thumbPath = Path.Combine(path, "Thumb");
			string originalPath = Path.Combine(path, "Original");


			if (!Directory.Exists(largePath))
				Directory.CreateDirectory(largePath);

			if (!Directory.Exists(thumbPath))
				Directory.CreateDirectory(thumbPath);

			if (!Directory.Exists(originalPath))
				Directory.CreateDirectory(originalPath);

			Config cfg = new Config();

			string ext = StringUtils.GetFileExtension(image);
			string fileName = StringUtils.GetFriendlyFileName(image);

			string _ImageName = string.Format("{0}_{1}.{2}", fileName, row.Id, ext);

			string ImageName = string.Format("{0}\\{1}", originalPath, _ImageName);
			string largeImageName = string.Format("{0}\\{1}", largePath, _ImageName);
			string thumbImageName = string.Format("{0}\\{1}", thumbPath, _ImageName);

			if (!Directory.Exists(originalPath))
				Directory.CreateDirectory(originalPath);

			System.IO.File.Copy(image, ImageName);

			try
			{
				Dimension largeImageSize = new Dimension(cte.ImageDefaultSize);
				Dimension thumbImageSize = new Dimension(cte.ImageDefaultThumbSize);

				if (!string.IsNullOrWhiteSpace(cfg.GetKey(cte.ImageSize)))
					largeImageSize = new Dimension(cfg.GetKey(cte.ImageSize));
				if (!string.IsNullOrWhiteSpace(cfg.GetKey(cte.ImageThumbSize)))
					thumbImageSize = new Dimension(cfg.GetKey(cte.ImageThumbSize));

				lw.GraphicUtils.ImageUtils.Resize(ImageName, largeImageName, largeImageSize.IntWidth, largeImageSize.IntHeight);

				if (!string.IsNullOrWhiteSpace(cfg.GetKey(cte.ImageThumbOption)) && cfg.GetKey(cte.ImageThumbOption) == "Crop")
					lw.GraphicUtils.ImageUtils.SmartCrop(ImageName, thumbImageName, thumbImageSize.IntWidth, thumbImageSize.IntHeight);
				//lw.GraphicUtils.ImageUtils.CropImage(ImageName, thumbImageName, thumbImageSize.IntWidth, thumbImageSize.IntHeight, ImageUtils.AnchorPosition.Default);
				else
					lw.GraphicUtils.ImageUtils.Resize(ImageName, thumbImageName, thumbImageSize.IntWidth, thumbImageSize.IntHeight);
			}
			catch (Exception ex)
			{
				ErrorContext.Add("resize-image", "Unable to resize album image.<br><span class=hid>" + ex.Message + "</span>");
			}

			row.FileName = _ImageName;
			base.UpdateData(Adp, _ds);

			UpdateAlbumModified(AlbumId, DateTime.Now);

			return row;
		}

		public bool DeleteImage(int ImageId)
		{
			DataView images = GetImages(string.Format("Id={0}", ImageId));
			if (images == null || images.Count == 0)
				return false;

			DataRow image = images[0].Row;

			string sql = "Delete From PhotoAlbumImages where Id = " + ImageId.ToString();
			DBUtils.ExecuteQuery(sql, cte.lib);

			string path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.PhotoAlbums);
			string largePath = Path.Combine(path, string.Format("Album{0}/Large", image["AlbumId"]));
			string thumbPath = Path.Combine(path, string.Format("Album{0}/Thumb", image["AlbumId"]));
			string originalPath = Path.Combine(path, string.Format("Album{0}/Original", image["AlbumId"]));
			string ImageName = string.Format("{0}\\{1}", largePath, image["FileName"]);
			string thumbImageName = string.Format("{0}\\{1}", thumbPath, image["FileName"]);
			string originalImageName = string.Format("{0}\\{1}", originalPath, image["FileName"]);

			if (File.Exists(ImageName))
				File.Delete(ImageName);

			if (File.Exists(thumbImageName))
				File.Delete(thumbImageName);

			if (File.Exists(originalImageName))
				File.Delete(originalImageName);

			return true;
		}
		public DataView GetRandomImages(string cond, int count)
		{
			string sql = "select top {0} NewId() as ran,* from PhotoAlbumImages {1} order by ran";

			sql = string.Format(sql, count, cond != "" ? " where " + cond : "");

			DataSet imagesDS = DBUtils.GetDataSet(sql, cte.lib);

			DataTable images = imagesDS.Tables[0];

			images.Columns.Add("Path", typeof(string));

			foreach (DataRow row in images.Rows)
				row["Path"] = GetImagesPath((int)row["AlbumId"]) + "/Large/" + row["FileName"].ToString();

			return new DataView(images);
		}
		public void SaveOrder(string ids)
		{
			string[] _ids = ids.Split('|');
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < _ids.Length; i++)
			{
				try
				{
					int id = Int32.Parse(_ids[i]);
					sb.Append(string.Format(
						"Update PhotoAlbumImages set Sort={0} where id={1};",
						i + 1,
						id));
				}
				catch
				{
				}
			}

			DBUtils.ExecuteQuery(sb.ToString(), cte.lib);
		}
	}
}
