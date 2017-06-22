using System;
using System.Data;
using System.IO;
using System.Web;
using lw.CTE;
using lw.Data;
using lw.GraphicUtils;
using lw.Utils;
using lw.WebTools;

namespace lw.Articles
{
	public class NewsManager : DirectorBase
	{
		public NewsManager() : base(cte.lib)
		{
		}

        // Gets news's image path
        public static string GetImagesPath(int NewsId)
        {
            // changed WebContext.Root + "/" to WebContext.Root
            string path = WebContext.Root + CTE.Folders.NewsImages;
            if (NewsId > 0)
                path = string.Format("{0}/News{1}", path, NewsId);
            return path;
        }

        public static string GetImagesPath()
        {
            return GetImagesPath(-1);
        }

		public DataView GetNews(string cond)
		{
			NewsDs _ds = new NewsDs();
			IDataAdapter Adp = base.GetAdapter(cte.NewsAdp);
			return ((NewsDs)base.FillData(Adp, _ds, cond)).News.DefaultView;
		}

		public DataView GetNewsTypeByUniqueName(string UniqueName)
		{
			return GetNewsTypes("UniqueName=N'" + StringUtils.SQLEncode(UniqueName) + "'");
		}

		public DataView GetNewsTypeChildren(string cond)
		{
			if (cond.Trim() != "")
				cond = " Where " + cond;


			cond = string.Format("select * from NewsTypes {0}",
						cond);

			return DBUtils.GetDataSet(cond.ToString(), cte.lib).Tables[0].DefaultView;
		}

		/// <summary>
		/// Gets the news type in a datarow
		/// </summary>
		/// <param name="TypeId">the NewsType Id</param>
		/// <returns></returns>
		public DataRow GetNewsType(int TypeId)
		{
			string cond = String.Format("TypeId={0}", TypeId);

			DataView types = GetNewsTypes(cond);

			if (types.Count > 0)
				return types[0].Row;

			return null;
		}

		/// <summary>
		/// Gets the news type in a datarow
		/// </summary>
		/// <param name="NewsType">Type Name or UniqueName</param>
		/// <returns></returns>
		public DataRow GetNewsType(string NewsType)
		{
			string cond = String.Format("UniqueName=N'{0}' or Name=N'{0}'", StringUtils.SQLEncode(NewsType));

			DataView types = GetNewsTypes(cond);

			if (types.Count > 0)
				return types[0].Row;

			return null;
		}

		/// <summary>
		/// Gets the news type view in a datarow
		/// </summary>
		/// <param name="TypeId">the NewsType Id</param>
		/// <returns></returns>
		public DataRow GetNewsTypeView(int TypeId)
		{
			string cond = String.Format("TypeId={0}", TypeId);

			DataView types = GetNewsTypesView(cond);

			if (types.Count > 0)
				return types[0].Row;

			return null;
		}

		/// <summary>
		/// Gets the news type view in a datarow
		/// </summary>
		/// <param name="NewsType">Type Name or UniqueName</param>
		/// <returns></returns>
		public DataRow GetNewsTypeView(string NewsType)
		{
			string cond = String.Format("UniqueName=N'{0}' or Name=N'{0}'", StringUtils.SQLEncode(NewsType));

			DataView types = GetNewsTypesView(cond);

			if (types.Count > 0)
				return types[0].Row;

			return null;
		}

		/// <summary>
		/// Returns a list of news types depending on condition
		/// </summary>
		/// <param name="cond">Sql Condition</param>
		/// <returns></returns>
		public DataView GetNewsTypes(string cond)
		{
			NewsDs _ds = new NewsDs();
			IDataAdapter Adp = base.GetAdapter(cte.NewsTypesAdp);
			return ((NewsDs)base.FillData(Adp, _ds, cond)).NewsTypes.DefaultView;
		}

		public DataView GetNewsTypesView(string cond)
		{
			if (cond.Trim() != "")
				cond = " Where " + cond;


			cond = string.Format("select * from NewsTypes {0}",
						cond);

			return DBUtils.GetDataSet(cond.ToString(), cte.lib).Tables[0].DefaultView;
		}

		public int AddNewsType(string Name)
		{
			return AddNewsType(Name, -1);
		}

		public int AddNewsType(string Name, int ParentId)
		{
			NewsDs _ds = new NewsDs();

			IDataAdapter Adp = base.GetAdapter(cte.NewsTypesAdp);
			NewsDs.NewsTypesRow row = _ds.NewsTypes.NewNewsTypesRow();

			row.Name = Name;
			row.UniqueName = StringUtils.ToURL(Name);
			row.ParentId = ParentId;
			row.DateCreated = DateTime.Now;
			row.LastModified = DateTime.Now;

			_ds.NewsTypes.AddNewsTypesRow(row);
			base.UpdateData(Adp, _ds);

			return row.TypeId;
		}

		public void UpdateNewsType(int TypeId, string Name)
		{
			NewsDs _ds = new NewsDs();

			DataRow newsType = this.GetNewsTypes(string.Format("TypeId={0}", TypeId))[0].Row;

			IDataAdapter Adp = base.GetAdapter(cte.NewsTypesAdp);
			NewsDs.NewsTypesRow row = _ds.NewsTypes.NewNewsTypesRow();

			row.TypeId = TypeId;
			_ds.NewsTypes.AddNewsTypesRow(row);

			row.AcceptChanges();

			row.Name = Name;
			row.UniqueName = StringUtils.ToURL(Name);

			base.UpdateData(Adp, _ds);
		}

		public bool DeleteNewsType(int TypeId)
		{
			DataRow newsType = this.GetNewsTypes(string.Format("TypeId={0}", TypeId))[0].Row;

			NewsDs _ds = new NewsDs();

			IDataAdapter Adp = base.GetAdapter(cte.NewsTypesAdp);
			NewsDs.NewsTypesRow row = _ds.NewsTypes.NewNewsTypesRow();

			row.TypeId = TypeId;
			_ds.NewsTypes.AddNewsTypesRow(row);

			row.AcceptChanges();

			row.Delete();
			base.UpdateData(Adp, _ds);

			return true;
		}

		public DataView GetNewsDateView(string cond)
		{
			NewsDs _ds = new NewsDs();
			IDataAdapter Adp = base.GetAdapter(cte.NewsDateViewAdp);
			DataView view = ((NewsDs)base.FillData(Adp, _ds, cond)).NewsDateView.DefaultView;

			view.Table.Columns.Add("MonthDate", DateTime.Now.GetType());

			foreach (DataRow row in view.Table.Rows)
			{
				row["MonthDate"] = DateTime.Parse(row["NewsMonth"].ToString());
			}

			view.Table.AcceptChanges();

			view.Sort = "MonthDate desc";

			return view;
		}

		public DataView GetNewsView(string cond)
		{
			string sql = "select * from NewsView";

			if (!String.IsNullOrEmpty(cond))
				sql += " where " + cond;

			DataSet ds = DBUtils.GetDataSet(sql, cte.lib);
			DataTable newsTable = ds.Tables[0];


			System.Collections.Specialized.NameValueCollection statustext = new System.Collections.Specialized.NameValueCollection();
			statustext.Add("0", "No Display");
			statustext.Add("2", "Archive");
			statustext.Add("3", "Main Page");

			System.Collections.Specialized.NameValueCollection languages = new System.Collections.Specialized.NameValueCollection();
			languages.Add("1", "Description");
			languages.Add("2", "Events");

			newsTable.Columns.Add("StatusText", typeof(string));
			newsTable.Columns.Add("LanguageText", typeof(string));

			foreach (DataRow n in newsTable.Rows)
			{
				n["StatusText"] = statustext[n["Status"].ToString()];
				n["LanguageText"] = languages[n["NewsLanguage"].ToString()];
			}

			newsTable.AcceptChanges();

			return newsTable.DefaultView;
		}

		public DataView GetNewsViewFromQuery(int max, string sql)
		{
			string _max = "100 PERCENT";
			if (max > 0)
				_max = max.ToString();

			if (sql.Trim() != "")
				sql = " Where " + sql;

			sql = string.Format("Select top {0} * from NewsView {1} Order BY Ranking DESC, NewsDate Desc",
						_max, sql);

			DataSet _ds = new DataSet();

			System.Data.SqlClient.SqlDataAdapter adp = new System.Data.SqlClient.SqlDataAdapter(sql, Config.GetConnectionString(this.connectionStrType));
			adp.Fill(_ds);

			DataView news = _ds.Tables[0].DefaultView;

			System.Collections.Specialized.NameValueCollection statustext = new System.Collections.Specialized.NameValueCollection();
			statustext.Add("0", "No Display");
			statustext.Add("2", "Archive");
			statustext.Add("3", "Main Page");

			System.Collections.Specialized.NameValueCollection languages = new System.Collections.Specialized.NameValueCollection();
			languages.Add("1", "English");
			languages.Add("2", "Spanish");

			news.Table.Columns.Add("StatusText", typeof(string));
			news.Table.Columns.Add("LanguageText", typeof(string));

			foreach (DataRowView n in news)
			{
				n.Row["StatusText"] = statustext[n["Status"].ToString()];
				n.Row["LanguageText"] = languages[n["NewsLanguage"].ToString()];
			}

			news.Table.AcceptChanges();

			return news;
		}

		public DataRowView GetNewsDetails(int newsId)
		{
			DataView dv = this.GetNews(string.Format("NewsId={0}", newsId));
			if (dv.Count > 0)
				return dv[0];
			return null;
		}

		public string NewsPicture(int newsId)
		{
			string path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.NewsImages);

			path = System.IO.Path.Combine(path, string.Format("News{0}", newsId));

			DataRowView news = this.GetNewsDetails(newsId);
			if (news != null)
			{
				string title = lw.Utils.StringUtils.ToURL(news["Title"], "-");

				if (File.Exists(Path.Combine(path, title + ".Jpg")))
					return string.Format(
						"{0}{1}/News{2}/{3}.Jpg",
						WebContext.Root,
						Folders.NewsImages,
						newsId,
						title
					);
				else
				{
					title = lw.Utils.StringUtils.ToURL(news["Title"], "");

					//WebContext.Response.Write(Path.Combine(path, title + ".Jpg") + "<BR>");

					if (File.Exists(Path.Combine(path, title + ".Jpg")))
						return string.Format(
							"{0}{1}/News{2}/{3}.Jpg",
							WebContext.Root,
							Folders.NewsImages,
							newsId,
							title
						);
				}
			}
			return "";
		}
		public string OutsideNewsPicture(int newsId)
		{
			string path = ""; 

			string NewsImagesRoot = Config.GetFromWebConfig("NewsImagesRoot");

			if(NewsImagesRoot != null && NewsImagesRoot != "")
				path = string.Format("/{0}{1}", NewsImagesRoot, lw.CTE.Folders.NewsImages);
			else
				path = "/" + lw.CTE.Folders.NewsImages;

			path = System.IO.Path.Combine(path, string.Format("News{0}", newsId));

			DataRowView news = this.GetNewsDetails(newsId);
			if (news != null)
			{
				string title = lw.Utils.StringUtils.ToURL(news["Title"].ToString());

				if (File.Exists(Path.Combine(WebContext.Server.MapPath("~" + path), title + ".Jpg")))
					return string.Format("{0}/{1}/{2}.Jpg",
						WebContext.Root,
						path, title);
			}
			return string.Format("{0}/images/t.gif", WebContext.Root);
		}

	
		public int AddNews(System.Web.HttpRequest req)
		{
			int newstype = -1;
			if (req.Form["NewsType"] != null && req.Form["NewsType"] != "")
				newstype = Int32.Parse(req.Form["NewsType"]);

			DateTime? publishDate = null;
			if(!String.IsNullOrEmpty(req["PublishDate"]))
				publishDate = DateTime.Parse(req["PublishDate"]);


			return AddNews(req["UniqueName"], req.Form["Title"], req.Form["Header"],
					WebContext.Server.HtmlDecode(req.Form["Content"]),
					req.Form["Date"],
					newstype, short.Parse(req.Form["Language"]),
					byte.Parse(req.Form["Status"]),
					req.Files["RelatedFile"],
					req.Files["Picture"], req["AutoResizePicture"] == "on",
					publishDate);
		}

		public int AddNews(string title)
		{
			return AddNews(StringUtils.ToURL(title), title , "", "", null, 0, 0, 0, null, null, false, null);
		}

        public int AddNews(string title, byte status)
        {
            return AddNews(StringUtils.ToURL(title), title, "", "", null, 0, 0, status, null, null, false, null);
        }

		public int AddNews(string title, int NewsType)
		{
			return AddNews(StringUtils.ToURL(title), title, "", "", null, NewsType, 0, 0, null, null, false, null);
		}

		public int AddNews(string uniqueName, string title, string header, string content, string date,
					int NewsType, short Language, byte Status, 
			HttpPostedFile RelatedFile, 
			HttpPostedFile DefaultImage, bool autoResizePicture, DateTime? publishDate)
		{
			NewsDs _ds = new NewsDs();

			IDataAdapter Adp = base.GetAdapter(cte.NewsAdp);
			NewsDs.NewsRow row = _ds.News.NewNewsRow();

			row.Title = title;
			row.Header = header;
			row.NewsText = content;
			if (date != null && date != "")
				row.NewsDate = DateTime.Parse(date);
			else
				row.NewsDate = DateTime.Now;

			if (String.IsNullOrEmpty(uniqueName))
			{
				uniqueName = StringUtils.ToURL(title);
			}

			row.NewsType = NewsType;
			row.NewsLanguage = Language;
			row.Status = Status;
			row.DateAdded = DateTime.Now;
			row.DateModified = DateTime.Now;
			row.CreatorId = WebContext.Profile.UserId;
			row.ModifierId = WebContext.Profile.UserId;

			row.Views = 0;
			row.Ranking = 0;
			row.UserRating = 0;

            if (publishDate != null)
                row.PublishDate = publishDate.Value;
            else
                row.PublishDate = DateTime.Now;
			row.NewsFile = "";
			row.ThumbImage = "";
			row.LargeImage = "";
			row.MediumImage = "";
			
			_ds.News.AddNewsRow(row);
			base.UpdateData(Adp, _ds);

			string path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.NewsImages);

			path = System.IO.Path.Combine(path, string.Format("News{0}", row.NewsId));


			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			Directory.CreateDirectory(Path.Combine(path, "Original"));
			Directory.CreateDirectory(Path.Combine(path, "Large"));
			Directory.CreateDirectory(Path.Combine(path, "Thumb"));

		

			if (RelatedFile != null && RelatedFile.ContentLength > 0)
			{
				string ext = StringUtils.GetFileExtension(RelatedFile.FileName);

				string fileName = StringUtils.GetFriendlyFileName(RelatedFile.FileName);

				if (title.Trim() == "")
					row.Title = fileName;

				path = WebContext.Server.MapPath("~/");
				path = Path.Combine(path, Folders.NewsFile);

				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);

				fileName = string.Format("{0}_{1}.{2}",
					fileName, row.NewsId, ext);

				try
				{
					RelatedFile.SaveAs(Path.Combine(path, fileName));
				}
				catch (IOException ex)
				{
					throw new Exception("IO ERROR, Could not save related news file: " + ex.Message);
				}
				catch (Exception ex1)
				{
					throw ex1;
				}
				row.NewsFile = fileName;
				base.UpdateData(Adp, _ds);
			}

			try
			{
				string sql = string.Format("Update News set UniqueName=N'{0}' where NewsId={1}",
					uniqueName, row.NewsId);
				DBUtils.ExecuteQuery(sql, cte.lib);
			}
			catch(Exception Ex)
			{
				ErrorContext.Add("unique-name", "cannot update unique name. " + Ex.Message);
			}
            if (DefaultImage != null && DefaultImage.ContentLength > 0)
				UpdateDefaultImage(row.NewsId, NewsType, DefaultImage, autoResizePicture, false, uniqueName);

			// search need to be checked for the path cannot be value null

	///		Search.NewsSearch.AddToIndex(row.NewsId);

			return row.NewsId;
		}

		public void UpdateDefaultImage(int newsId, int typeId, HttpPostedFile defaultImage, 
			bool autoResize, bool deleteOld, string uniqueName)
		{
			string path = Folders.NewsImages;
			path = Path.Combine(path, string.Format("News{0}", newsId));
			path = lw.WebTools.WebContext.Server.MapPath("~/" + path);
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			
			/* Used to render a anonymous file with the guid even if there was no image selected in the input
			 * 
			 * 
			path = Path.Combine(path, System.Guid.NewGuid().ToString() + Path.GetExtension(defaultImage.FileName));
			defaultImage.SaveAs(path);
			
			 * If they were an image to be deleted and being replaced by another image,
			 * It used to send the path empty so the function will delete the image and not save the new image
			 * 
			if (deleteOld || defaultImage.ContentLength == 0)
				path = "";
			*/ 

			// NEW

			if (!string.IsNullOrEmpty(defaultImage.FileName))
			{
				path = Path.Combine(path, uniqueName + Path.GetExtension(defaultImage.FileName));
				defaultImage.SaveAs(path);
			}

			if (deleteOld)
				path = "";

			UpdateDefaultImage(newsId, typeId, path, autoResize, deleteOld);
		}


		public void UpdateDefaultImage(int newsId, int typeId, string newsImage,
			bool autoResize, bool deleteOld)
		{
			LINQ.NewsManager lnMgr = new LINQ.NewsManager();
			LINQ.NewsDetail newsDetail = lnMgr.GetNews(newsId);

			string thumbName = newsDetail.ThumbImage;
			string largeName = newsDetail.LargeImage;
			string mediumName = newsDetail.MediumImage;

			string path = Folders.NewsImages;
			path = Path.Combine(path, string.Format("News{0}", newsId));
			path = lw.WebTools.WebContext.Server.MapPath("~/" + path);
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			deleteOld = deleteOld || !String.IsNullOrEmpty(newsImage);

			if (deleteOld && !StringUtils.IsNullOrWhiteSpace(newsDetail.LargeImage))
			{
				if (File.Exists(Path.Combine(path, newsDetail.LargeImage)))
				{
					File.Delete(Path.Combine(path, newsDetail.LargeImage));
				}
				if (!StringUtils.IsNullOrWhiteSpace(newsDetail.ThumbImage) && File.Exists(Path.Combine(path, newsDetail.ThumbImage)))
				{
					File.Delete(Path.Combine(path, newsDetail.ThumbImage));
				}
				if (!StringUtils.IsNullOrWhiteSpace(newsDetail.MediumImage) && File.Exists(Path.Combine(path, newsDetail.MediumImage)))
				{
					File.Delete(Path.Combine(path, newsDetail.MediumImage));
				}

				newsDetail.ThumbImage = "";
				newsDetail.LargeImage = "";
				newsDetail.MediumImage = "";
			}
			if (File.Exists(newsImage))
			{
				string largePath = Path.Combine(path, "Large");
				if (!Directory.Exists(largePath))
					Directory.CreateDirectory(largePath);

				string thumbPath = Path.Combine(path, "Thumb");
				if (!Directory.Exists(thumbPath))
					Directory.CreateDirectory(thumbPath);


				string originalPath = Path.Combine(path, "Original");
				if (!Directory.Exists(originalPath))
					Directory.CreateDirectory(originalPath);

				string mediumPath;


				string fileName = Path.GetFileNameWithoutExtension(newsImage);
				string ext = Path.GetExtension(newsImage);
				thumbName = fileName + "-s.jpg";// +ext;
				largeName = fileName + "-l.jpg";// +ext;
				mediumName = fileName + "-m.jpg";// +ext;
				largePath = Path.Combine(path, largeName);
				thumbPath = Path.Combine(path, thumbName);
				mediumPath = Path.Combine(path, mediumName);

                if (!File.Exists(largePath))
                    File.Copy(newsImage, largePath);
				File.Delete(newsImage);

				Dimension largeSize = null, mediumSize = null, thumbSize = null;

				DataView newsType = GetNewsTypes("TypeId=" + typeId.ToString());
				if (newsType.Count > 0)
				{
					DataRowView type = newsType[0];
					
					largeSize = new Dimension(type["LargeSize"].ToString());
					mediumSize = new Dimension(type["MediumSize"].ToString());
					thumbSize = new Dimension(type["ThumbSize"].ToString());
				}

				Config cfg = new Config();

				if (!largeSize.Valid)
					largeSize = new Dimension(cfg.GetKey(lw.CTE.Settings.News_DefaultImageSize));
				if (!mediumSize.Valid)
					mediumSize = new Dimension(cfg.GetKey(lw.CTE.Settings.News_MediumImageSize));
				if (!thumbSize.Valid)
					thumbSize = new Dimension(cfg.GetKey(lw.CTE.Settings.News_ThumbImageSize));
				

			
				// Thumb get Croped for cases that use precise dimensions (Width AND Height)
				// Medium Resized depending on the the Width or the Height
				// Large Resize only if AutoResize Checked

				if (thumbSize.Valid)
				{
					string resizeType = cfg.GetKey(Settings.ThumbImageResizeType);
					
					if(resizeType.ToLower() == lw.CTE.Enum.ImageResizeType.Resize.ToString().ToLower())
						ImageUtils.Resize(largePath, thumbPath, thumbSize.IntWidth, thumbSize.IntHeight);
					else
						ImageUtils.CropImage(largePath, thumbPath, thumbSize.IntWidth, thumbSize.IntHeight, ImageUtils.AnchorPosition.Default);
				}
				if (mediumSize.Valid)
				{
					string resizeType = cfg.GetKey(Settings.MediumImageResizeType);

					if (resizeType.ToLower() == lw.CTE.Enum.ImageResizeType.Resize.ToString().ToLower())
						ImageUtils.Resize(largePath, mediumPath, mediumSize.IntWidth, mediumSize.IntHeight);
					else
						ImageUtils.CropImage(largePath, mediumPath, mediumSize.IntWidth, mediumSize.IntHeight, ImageUtils.AnchorPosition.Default);
				}
				if (largeSize.Valid && autoResize)
				{
					string resizeType = cfg.GetKey(Settings.News_DefaultImageResizeType);

					if (resizeType.ToLower() == lw.CTE.Enum.ImageResizeType.Crop.ToString().ToLower())
						ImageUtils.CropImage(largePath, largePath, largeSize.IntWidth, largeSize.IntHeight, ImageUtils.AnchorPosition.Default);
					else
						ImageUtils.Resize(largePath, largePath, largeSize.IntWidth, largeSize.IntHeight);
				}

				newsDetail.ThumbImage = thumbName;
				newsDetail.LargeImage = largeName;
				newsDetail.MediumImage = mediumName;
			}

			lnMgr.Save();
		}

		public DataView NewsImages(int newsId)
		{
			DataTable news = new DataTable();
			news.Columns.Add("Image", typeof(string));
			news.Columns.Add("LastModified", typeof(DateTime));

			
			string path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.NewsImages);

			path = System.IO.Path.Combine(path, string.Format("News{0}", newsId));

			path = Path.Combine(path, "Thumb");

			if (Directory.Exists(path))
			{
				DirectoryInfo Source = new DirectoryInfo(path);

				foreach (FileInfo fi in Source.GetFiles("*.jpg"))
				{
					DataRow dr = news.NewRow();

					dr["Image"] = fi.Name;
					dr["LastModified"] = fi.LastWriteTime;

					news.Rows.Add(dr);
				}
				foreach (FileInfo fi in Source.GetFiles("*.jpeg"))
				{
					DataRow dr = news.NewRow();

					dr["Image"] = fi.Name;
					dr["LastModified"] = fi.LastWriteTime;

					news.Rows.Add(dr);

				}
				foreach (FileInfo fi in Source.GetFiles("*.bmp"))
				{
					DataRow dr = news.NewRow();

					dr["Image"] = fi.Name;
					dr["LastModified"] = fi.LastWriteTime;

					news.Rows.Add(dr);

				}
				foreach (FileInfo fi in Source.GetFiles("*.gif"))
				{
					DataRow dr = news.NewRow();

					dr["Image"] = fi.Name;
					dr["LastModified"] = fi.LastWriteTime;

					news.Rows.Add(dr);

				}
				foreach (FileInfo fi in Source.GetFiles("*.png"))
				{
					DataRow dr = news.NewRow();

					dr["Image"] = fi.Name;
					dr["LastModified"] = fi.LastWriteTime;

					news.Rows.Add(dr);
				}
			}

			return new DataView(news, "", "Image", DataViewRowState.CurrentRows);
		}

		public bool UpdateNews(int newsId, HttpRequest req)
		{
			int newstype = -1;
			if (req.Form["NewsType"] != null && req.Form["NewsType"] != "")
				newstype = Int32.Parse(req.Form["NewsType"]);

			int? Ranking = null, UserRating = null, Views = null;
            short Language = 1;
            byte Status = 1;

			if(!String.IsNullOrEmpty(req["Ranking"]))
				Ranking = Int32.Parse(req["Ranking"]);

			if(!String.IsNullOrEmpty(req["UserRating"]))
				UserRating = Int32.Parse(req["UserRating"]);

			if(!String.IsNullOrEmpty(req["Views"]))
				Views = Int32.Parse(req["Views"]);

            if (!String.IsNullOrEmpty(req["Language"]))
                Language = short.Parse(req["Language"]);

            if (!String.IsNullOrEmpty(req["Status"]))
                Status = byte.Parse(req["Status"]);

			DateTime? publishDate = null;
			if (!String.IsNullOrEmpty(req["PublishDate"]))
				publishDate = DateTime.Parse(req["PublishDate"]);

			string uniqueName = req["UniqueName"];

			return this.UpdateNews(newsId, uniqueName, req.Form["Title"],
					req.Form["Header"],
					WebContext.Server.HtmlDecode(req.Form["Content"]),
					req.Form["Date"],
                    newstype, Language,
                    Status,
					(req.Form["DeleteFile"] == "on"),
					req.Files["RelatedFile"],
					req.Files["Picture"], req["AutoResizePicture"] == "on",
					req["DeleteOldPicture"] == "on",
					Ranking, UserRating, Views,
					publishDate);
		}

		public bool UpdateNews(int newsId, string uniqueName, string title, string header, string content, string date,
			int NewsType, short Language, byte Status, bool DeleteFile, 
			HttpPostedFile RelatedFile, 
			HttpPostedFile DefaultImage, bool autoResizePicture, bool deleteOldPicture, 
			int? Ranking, int? UserRating, int? Views, DateTime? publishDate)
		{
			NewsDs _ds = new NewsDs();

			DataRow news = this.GetNews(string.Format("NewsId={0}", newsId))[0].Row;

			string oldImage = NewsPicture(newsId);

			IDataAdapter Adp = base.GetAdapter(cte.NewsAdp);
			NewsDs.NewsRow row = _ds.News.NewNewsRow();

			row.NewsId = newsId;
			_ds.News.AddNewsRow(row);
			row.AcceptChanges();

			row.Title = title;

			if (String.IsNullOrEmpty(uniqueName) || (uniqueName == news["UniqueName"].ToString() && uniqueName == StringUtils.ToURL(news["Title"].ToString())))
			{
				uniqueName = StringUtils.ToURL(title);
			}

			row.UniqueName = uniqueName;
			row.Header = header;
			row.NewsText = content;
			if (date != null && date != "")
				row.NewsDate = DateTime.Parse(date);

			row.NewsType = NewsType;
			row.NewsLanguage = Language;
			row.Status = Status;
			row.NewsFile = string.Format("{0}", news["NewsFile"]);
			row.DateModified = DateTime.Now;
			row.ModifierId = WebContext.Profile.UserId;

			if (Ranking != null)
				row.Ranking = Ranking.Value;
			if (UserRating != null)
				row.UserRating = UserRating.Value;
			if (Views != null)
				row.Views = Views.Value;
			if (publishDate != null)
				row.PublishDate = publishDate.Value;

			base.UpdateData(Adp, _ds);

			string path = "";

			try
			{
				if (!StringUtils.IsNullOrWhiteSpace(oldImage) && title != news["Title"].ToString())
				{
					path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.NewsImages);

					path = System.IO.Path.Combine(path, string.Format("News{0}", newsId));

					path = Path.Combine(path, StringUtils.ToURL(title) + ".JPG");

					File.Move(WebContext.Server.MapPath(oldImage), path);

				}
			}
			catch (Exception Ex)
			{
				ErrorContext.Add("default-picture", "Could not re-copy default picture. " + Ex.Message);
			}

			path = WebContext.Server.MapPath("~/");
			path = Path.Combine(path, Folders.NewsFile);

			DeleteFile = (DeleteFile || (RelatedFile != null && RelatedFile.ContentLength > 0))
						&& !StringUtils.IsNullOrWhiteSpace(news["NewsFile"]);

			if (DeleteFile)
			{
				string fileName = Path.Combine(path, news["NewsFile"].ToString());
				if (File.Exists(fileName))
					File.Delete(fileName);

				row.NewsFile = "";
			}

			if (RelatedFile != null && RelatedFile.ContentLength > 0)
			{
				string ext = StringUtils.GetFileExtension(RelatedFile.FileName);

				string fileName = StringUtils.GetFriendlyFileName(RelatedFile.FileName);

				if (title.Trim() == "")
					row.Title = fileName;

				fileName = string.Format("{0}_{1}.{2}",
					fileName, row.NewsId, ext);

				try
				{
					if (!Directory.Exists(path))
						Directory.CreateDirectory(path);
					RelatedFile.SaveAs(Path.Combine(path, fileName));
				}
				catch (IOException ex)
				{
					throw new Exception("IO ERROR, Could not save related news file: " + ex.Message);
				}
				catch (Exception ex1)
				{
					throw ex1;
				}
				row.NewsFile = fileName;
			}

			if(row.NewsFile != string.Format("{0}", news["NewsFile"]))
				base.UpdateData(Adp, _ds);

			try
			{
				string sql = string.Format("Update News set UniqueName=N'{0}' where NewsId={1}",
					uniqueName, row.NewsId);
				DBUtils.ExecuteQuery(sql, cte.lib);
			}
			catch (Exception Ex)
			{
				ErrorContext.Add("unique-name", "cannot update unique name. " + Ex.Message);
			}

            if ((DefaultImage!= null && DefaultImage.ContentLength > 0) || deleteOldPicture)
				UpdateDefaultImage(row.NewsId, NewsType, DefaultImage, autoResizePicture, deleteOldPicture, row.UniqueName);

			//Search.NewsSearch.UpdateIndex(row.NewsId);

			return true;
		}
		public bool DeleteNews(int newsId)
		{
			string path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.NewsImages);

			path = System.IO.Path.Combine(path, string.Format("News{0}", newsId));

			if (Directory.Exists(path))
				Directory.Delete(path, true);

			DataRow news = this.GetNews(string.Format("NewsId={0}", newsId))[0].Row;

			path = WebContext.Server.MapPath("~/");
			path = Path.Combine(path, Folders.NewsFile);
			
			bool DeleteFile = news["NewsFile"] != DBNull.Value &&  string.Format("{0}", news["NewsFile"]) != "";

			if (DeleteFile)
			{
				string fileName = Path.Combine(path, news["NewsFile"].ToString());
				if (File.Exists(fileName))
					File.Delete(fileName);
			}


			/*
			NewsDs _ds = new NewsDs();
			IDataAdapter Adp = base.GetAdapter(cte.NewsAdp);
			NewsDs.NewsRow row = _ds.News.NewNewsRow();

			row.NewsId = newsId;
			_ds.News.AddNewsRow(row);
			row.AcceptChanges();

			row.Delete();
			base.UpdateData(Adp, _ds);

			 */

			string sql = string.Format("delete froM News where NewsId={0}", newsId);
			DBUtils.ExecuteQuery(sql, cte.lib);

			//Search.NewsSearch.RemoveFromIndex(newsId);

			return true;
		}

		public bool DeleteImage(string imageName, int newsId)
		{
			string path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.NewsImages);

			string LargePath = System.IO.Path.Combine(path, string.Format("News{0}/Large/{1}", newsId, imageName));
			string ThumbPath = System.IO.Path.Combine(path, string.Format("News{0}/Thumb/{1}", newsId, imageName));

			if (File.Exists(LargePath))
				File.Delete(LargePath);

			if (File.Exists(ThumbPath))
				File.Delete(ThumbPath);


			return true;
		}

		/// <summary>
		/// Saves the news as draft in the draft xml field.
		/// </summary>
		/// <param name="NewsId">NewsId</param>
		/// <param name="xml">Xml draft fields</param>
		public void SaveDraft(int NewsId, string xml)
		{
			string sql = "Update News set draft='{0}' where NewsId={1}";
			sql = String.Format(sql, StringUtils.SQLEncode(xml), NewsId);

			DBUtils.ExecuteQuery(sql, cte.lib);
		}


		/// <summary>
		/// Directly publishes the title and text of the news
		/// </summary>
		/// <param name="NewsId">NewsId</param>
		/// <param name="title">Title</param>
		/// <param name="text">Text body</param>
		public void Publish(int NewsId, string title, string text)
		{
			string sql = "Update News set title='{0}',Status=3, NewsText='{1}', draft=null where NewsId={2}";
			sql = String.Format(sql, StringUtils.SQLEncode(title), StringUtils.SQLEncode(text), NewsId);

			DBUtils.ExecuteQuery(sql, cte.lib);
		}


		/// <summary>
		/// Deletes any related draft version of the page.
		/// </summary>
		/// <param name="NewsId">NewsId</param>
		public void DiscardDraft(int NewsId)
		{
			string sql = "Update News set draft=null where NewsId={0}";
			sql = String.Format(sql, NewsId);


			DBUtils.ExecuteQuery(sql, cte.lib);
		}


		public DataView NewsFiles(int newsId)
		{
			DataTable news = new DataTable();
			news.Columns.Add("File", typeof(string));
			news.Columns.Add("LastModified", typeof(DateTime));


			string path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.NewsImages);

			path = System.IO.Path.Combine(path, string.Format("News{0}", newsId));

			path = Path.Combine(path, "Files");

			if (Directory.Exists(path))
			{
				DirectoryInfo Source = new DirectoryInfo(path);

				foreach (FileInfo fi in Source.GetFiles("*.*"))
				{
					DataRow dr = news.NewRow();

					dr["File"] = fi.Name;
					dr["LastModified"] = fi.LastWriteTime;

					news.Rows.Add(dr);
				}
			}

			return new DataView(news, "", "File", DataViewRowState.CurrentRows);
		}

		/// <summary>
		/// Increment the views of an article
		/// This method is called everytime the article is displayed
		/// </summary>
		/// <param name="ArticleId">Article or News Id</param>
		public void IncrementArticleViews(int ArticleId)
		{
			string sql = "Update News set [Views]=[Views]+1 where NewsId={0}";
			sql = String.Format(sql, ArticleId);

			DBUtils.ExecuteQuery(sql, cte.lib);
		}


		#region Static Methods

		public static string GetMediumImage(DataRow news)
		{
			if (String.IsNullOrEmpty(news["MediumImage"].ToString()))
				return "";

			string path = Path.Combine(Folders.NewsImages, "News" + news["NewsId"].ToString());
			return Path.Combine(path, news["MediumImage"].ToString());
		}

		public static string GetThumbImage(DataRow news)
		{
			if (String.IsNullOrEmpty(news["ThumbImage"].ToString()))
				return "";

			string path = Path.Combine(Folders.NewsImages, "News" + news["NewsId"].ToString());
			return Path.Combine(path, news["ThumbImage"].ToString());
		}

		public static string GetLargeImage(DataRow news)
		{
			if (String.IsNullOrEmpty(news["LargeImage"].ToString()))
				return "";

			string path = Path.Combine(Folders.NewsImages, "News" + news["NewsId"].ToString());
			return Path.Combine(path, news["LargeImage"].ToString());
		}

		#endregion 
	}
}
