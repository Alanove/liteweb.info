using System;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;

using lw.Data;
using lw.Utils;
using lw.WebTools;
using lw.CTE;
using lw.GraphicUtils;
using lw.CTE.Enum;
using System.Data;
using lw.Pages.data;
using lw.HashTags;
using System.Text.RegularExpressions;
using System.Text;

namespace lw.Pages
{
	public class PagesManager : LINQManager
	{
		public PagesManager()
			: base(cte.lib)
		{
		}


		//public data.Page AddPage(int ParentId, string Title)
		//{
		//	return AddPage(StringUtils.ToURL(Title), )
		//}

		public data.Page AddPage(int ParentId, string Title)
		{
			return AddPage(ParentId, Title, DateTime.Now);
		}

		public data.Page AddPage(int ParentId, string Title, string PageType)
		{
			return AddPage(ParentId, StringUtils.ToURL(Title), Title, Title, "", "", PageStatus.Hidden,
				Languages.English, null,
				WebContext.Profile.UserId,
				WebContext.Profile.UserId,
				DateTime.Now,
				PageType, "");
		}




		public data.Page AddPage(int ParentId, string Title, DateTime? PublishDate)
		{
			return AddPage(ParentId, StringUtils.ToURL(Title), Title, Title, "", "", PageStatus.Hidden,
				Languages.English, null,
				WebContext.Profile.UserId,
				WebContext.Profile.UserId,
				PublishDate,
				"default", "");
		}

		/// <summary>
		/// Update properties of a Page
		/// </summary>
		/// <param name="PageId"></param>
		/// <returns></returns>
		public void UpdatePageProperties(int PageId, Dictionary<string, string> properties)
		{
			foreach (KeyValuePair<string, string> property in properties)
			{
				PageDataPropertyValue item = new PageDataPropertyValue();

				var propID = (from p in PagesData.PageDataProperties
							  where p.DataPropertyName == property.Key
							  select p).FirstOrDefault();
				if (propID != null)
				{
					var propVal = (from p in PagesData.PageDataPropertyValues
								   where p.DataPropertyID == propID.DataPropertyID
								   && p.PageID == PageId
								   select p).FirstOrDefault();

					if (propVal != null)
					{
						if (propVal.DataPropertyValue != property.Value)
						{
							propVal.DataPropertyValue = property.Value;

							PagesData.SubmitChanges();
						}
					}
					else
					{
						item.PageID = PageId;
						item.DataPropertyID = propID.DataPropertyID;
						item.DataPropertyValue = property.Value;
						PagesData.PageDataPropertyValues.InsertOnSubmit(item);
						PagesData.SubmitChanges();
					}
				}
			}
		}

		/// <summary>
		/// Adds a page with description
		/// </summary>
		/// <param name="ParentId"></param>
		/// <param name="Title"></param>
		/// <param name="SmallDescription"></param>
		/// <param name="Description"></param>
		/// <returns></returns>
		public data.Page AddPage(int ParentId, string Title, string SmallDescription, string Description)
		{
			return AddPage(ParentId, StringUtils.ToURL(Title), Title, Title, SmallDescription, Description, PageStatus.Hidden,
				Languages.English, null,
				WebContext.Profile.UserId,
				WebContext.Profile.UserId,
				DateTime.Now,
				"default", "");
		}

		public data.PageType AddPageType(string type)
		{
			return AddPageType(type, null);
		}

		public data.PageType AddPageType(string type, string thumbSize)
		{
			return AddPageType(type, thumbSize, null);
		}

		public data.PageType AddPageType(string type, string thumbSize, string mediumSize)
		{
			return AddPageType(type, thumbSize, mediumSize, null);
		}

		public data.PageType AddPageType(string Type, string thumbSize, string mediumSize, string largeSize)
		{
			PageType checkIfExist = GetPageType(Type);
			if (checkIfExist != null)
				return null;

			PageType item = new PageType();
			item.Type = Type;
			item.ThumbSize = thumbSize;
			item.MediumSize = mediumSize;
			item.LargeSize = largeSize;
			PagesData.PageTypes.InsertOnSubmit(item);
			PagesData.SubmitChanges();
			return item;
		}

		public data.PageTemplate AddPageTemplate(string title)
		{
			return AddPageTemplate(title, null);
		}

		public data.PageTemplate AddPageTemplate(string title, string filename)
		{
			PageTemplate checkIfExist = GetPageTemplate(title);
			if (checkIfExist != null)
				return null;

			PageTemplate item = new PageTemplate();
			item.Title = title;
			item.Filename = filename;
			PagesData.PageTemplates.InsertOnSubmit(item);
			PagesData.SubmitChanges();
			return item;
		}


		public data.Page AddPage(
			int? ParentId,
			string Url,
			string Title,
			string Header,
			string SmallDescription,
			string PageContent,
			PageStatus? Status,
			lw.CTE.Enum.Languages? Language,
			HttpPostedFile Image,
			int CreatedBy,
			int ModifiedBy,
			DateTime? PublishDate,
			string PageType
			)
		{
			return AddPage(ParentId, Url, Title, Header, SmallDescription, PageContent, Status, Language, Image,
				CreatedBy, ModifiedBy, PublishDate, PageType, "");
		}


		public data.Page AddPage(
			int? ParentId,
			string Url,
			string Title,
			string Header,
			string SmallDescription,
			string PageContent,
			PageStatus? Status,
			lw.CTE.Enum.Languages? Language,
			HttpPostedFile Image,
			int CreatedBy,
			int ModifiedBy,
			DateTime? PublishDate,
			string PageType,
			string HashTags
			)
		{
			return AddPage(ParentId, Url, Title, Header, SmallDescription, PageContent, Status, Language, Image,
				CreatedBy, ModifiedBy, PublishDate, PageType, HashTags, "");
		}

		public data.Page AddPage(
			int? ParentId,
			string Url,
			string Title,
			string Header,
			string SmallDescription,
			string PageContent,
			PageStatus? Status,
			lw.CTE.Enum.Languages? Language,
			HttpPostedFile Image,
			int CreatedBy,
			int ModifiedBy,
			DateTime? PublishDate,
			string PageType,
			string HashTags,
			string Keywords
			)
		{
			if (Status == null)
				Status = PageStatus.Hidden;
			if (Language == null)
				Language = lw.CTE.Enum.Languages.English;

			if (GetPage(Url) != null)
			{
				int i = 1;
				string temp;
				do
				{
					temp = Url + "_" + i.ToString();
					i++;
				}
				while (GetPage(temp) != null);
				Url = temp;
			}
			int p = 1;
			if (PageType.ToLower() != "default" && PageType != "1")
				p = Int32.Parse(PageType);

			if (SmallDescription == null || String.IsNullOrWhiteSpace(SmallDescription))
				SmallDescription = TrancateDescription(PageContent, 512);

			data.Page page = new data.Page
			{
				ParentId = ParentId,
				URL = Url,
				Title = Title,
				Header = Header,
				SmallDescription = SmallDescription,
				PageContent = PageContent,
				Status = (byte)Status,
				Language = (short)Language,
				CreatedBy = CreatedBy,
				Views = 0,
				UserRating = 0,
				Ranking = 1000,
				ModifiedBy = ModifiedBy,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now,
				PublishDate = PublishDate,
				PageType = p,
				Tags = HashTags,
				Keywords = Keywords
			};

			PagesData.Pages.InsertOnSubmit(page);
			PagesData.SubmitChanges();

			if (Image != null)
				UpdateDefaultImage(page.PageId, Image, true, true);

			HashTagsManager htMgr = new HashTagsManager();
			htMgr.UpdateTags(page.PageId, HashTagTypes.Pages, page.Title + " " + page.Header + page.PageContent + " " + page.SmallDescription + " " + page.Tags);

			return page;
		}

		public void UpdateDefaultImage(int PageId, HttpPostedFile NewImage,
		   bool AutoResize, bool DeleteOld, string CropOptionsJson)
		{
			string path = Folders.PagesFolder;
			path = Path.Combine(path, string.Format("Page_{0}", PageId));
			path = lw.WebTools.WebContext.Server.MapPath("~/" + path);
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			data.Page page = GetPage(PageId);

			if (!string.IsNullOrEmpty(NewImage.FileName))
			{
				bool value;

				if (Boolean.TryParse(Config.GetFromWebConfig("RandomImageName"), out value))
					path = Path.Combine(path, Guid.NewGuid().ToString("N").Substring(0, 16) + Path.GetExtension(NewImage.FileName));
				else
					path = Path.Combine(path, StringUtils.ToURL(page.Title) + StringUtils.GeneratePassword() + Path.GetExtension(NewImage.FileName));
				NewImage.SaveAs(path);
			}
			else
				path = "";

			DeleteOld = NewImage.ContentLength > 0 || DeleteOld;

			if (!String.IsNullOrWhiteSpace(CropOptionsJson))
			{
				CropOptions cropOption = System.Web.Helpers.Json.Decode<CropOptions>(CropOptionsJson);
				UpdateDefaultImage(PageId, path, AutoResize, DeleteOld, cropOption);
			}
			else
				UpdateDefaultImage(PageId, path, AutoResize, DeleteOld);
		}

		/// <summary>
		/// </summary>
		/// <param name="PageId"></param>
		/// <param name="NewImage"></param>
		/// <param name="AutoResize"></param>
		/// <param name="DeleteOld"></param>
		public void UpdateDefaultImage(int PageId, HttpPostedFile NewImage,
			bool AutoResize, bool DeleteOld)
		{
			UpdateDefaultImage(PageId, NewImage, AutoResize, DeleteOld, null);
		}

		/// <summary>
		/// Updates the default file for this page item.
		/// </summary>
		/// <param name="PageId">PageId</param>
		/// <param name="NewFile">The httpposted file</param>
		/// <param name="DeleteOld">Flag to delete old file</param>
		public void UpdateDefaultFile(int PageId, HttpPostedFile NewFile,
			bool DeleteOld)
		{
			string path = Folders.PagesFolder;
			path = Path.Combine(path, string.Format("Page_{0}", PageId));
			path = lw.WebTools.WebContext.Server.MapPath("~/" + path);
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			data.Page page = GetPage(PageId);

			if (!string.IsNullOrEmpty(NewFile.FileName))
			{
				path = Path.Combine(path, StringUtils.ToURL(page.Title) + StringUtils.GeneratePassword() + Path.GetExtension(NewFile.FileName));
				NewFile.SaveAs(path);
			}
			else
				path = "";

			DeleteOld = NewFile.ContentLength > 0 || DeleteOld;

			UpdateDefaultFile(PageId, path, DeleteOld);
		}


		public void DeletePageFile(int PageId)
		{
			UpdateDefaultFile(PageId, "", true);
		}

		/// <summary>
		/// Updates the default file for the page.
		/// </summary>
		/// <param name="PageId"></param>
		/// <param name="OriginalFile"></param>
		/// <param name="DeleteOld"></param>
		public void UpdateDefaultFile(int PageId, string OriginalFile,
			bool DeleteOld)
		{
			data.Page page = GetPage(PageId);
			data.PageType type = page.PageType != null ? GetPageType(page.PageType.Value) : null;

			string imagePart = Path.GetFileNameWithoutExtension(page.PageFile);
			string imageExtension = Path.GetExtension(page.PageFile);

			string path = Folders.PagesFolder;

			path = Path.Combine(path, string.Format("Page_{0}", PageId));
			path = lw.WebTools.WebContext.Server.MapPath("~/" + path);
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			string fileName = Path.Combine(path, string.Format("{0}-file{1}", imagePart, imageExtension));

			DeleteOld = DeleteOld || String.IsNullOrWhiteSpace(path) && !String.IsNullOrEmpty(page.Image);

			if (DeleteOld && !String.IsNullOrWhiteSpace(page.PageFile))
			{
				try
				{
					if (File.Exists(Path.Combine(path, page.PageFile)))
						File.Delete(Path.Combine(path, page.PageFile));
				}
				catch
				{

				}
				page.PageFile = "";
			}

			if (!String.IsNullOrWhiteSpace(OriginalFile) && File.Exists(OriginalFile))
			{
				imagePart = Path.GetFileNameWithoutExtension(OriginalFile);
				imageExtension = Path.GetExtension(OriginalFile);

				fileName = Path.Combine(path, string.Format("{0}-file{1}", imagePart, imageExtension));

				File.Move(OriginalFile, fileName);

				page.PageFile = string.Format("{0}-file{1}", imagePart, imageExtension);

			}
			PagesData.SubmitChanges();
		}

		public void UpdateDefaultImage(int PageId, string OriginalImage,
			bool AutoResize, bool DeleteOld)
		{
			UpdateDefaultImage(PageId, OriginalImage, AutoResize, DeleteOld, null);
		}

		/// <summary>
		/// Updates the default image for the page.
		/// </summary>
		public void UpdateDefaultImage(int PageId, string OriginalImage,
			bool AutoResize, bool DeleteOld, CropOptions cropOptions)
		{
			data.Page page = GetPage(PageId);
			data.PageType type = page.PageType != null ? GetPageType(page.PageType.Value) : null;

			string imagePart = Path.GetFileNameWithoutExtension(page.Image);
			string imageExtension = Path.GetExtension(page.Image);

			string path = Folders.PagesFolder;

			path = Path.Combine(path, string.Format("Page_{0}", PageId));
			path = lw.WebTools.WebContext.Server.MapPath("~/" + path);
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			string thumbName = Path.Combine(path, string.Format("{0}-t{1}", imagePart, imageExtension));
			string largeName = Path.Combine(path, string.Format("{0}-l{1}", imagePart, imageExtension));
			string mediumName = Path.Combine(path, string.Format("{0}-m{1}", imagePart, imageExtension));

			DeleteOld = DeleteOld || String.IsNullOrWhiteSpace(path) && !String.IsNullOrEmpty(page.Image);

			if (DeleteOld && !String.IsNullOrWhiteSpace(page.Image))
			{
				try
				{
					if (File.Exists(Path.Combine(path, page.Image)))
						File.Delete(Path.Combine(path, page.Image));

					if (File.Exists(Path.Combine(path, thumbName)))
						File.Delete(Path.Combine(path, thumbName));

					if (File.Exists(Path.Combine(path, mediumName)))
						File.Delete(Path.Combine(path, mediumName));

					if (File.Exists(Path.Combine(path, largeName)))
						File.Delete(Path.Combine(path, largeName));
				}
				catch
				{

				}
			}


			if (!String.IsNullOrWhiteSpace(OriginalImage) && File.Exists(OriginalImage))
			{

				imagePart = Path.GetFileNameWithoutExtension(OriginalImage);
				imageExtension = Path.GetExtension(OriginalImage);

				thumbName = Path.Combine(path, string.Format("{0}-t{1}", imagePart, imageExtension));
				largeName = Path.Combine(path, string.Format("{0}-l{1}", imagePart, imageExtension));
				mediumName = Path.Combine(path, string.Format("{0}-m{1}", imagePart, imageExtension));

				Dimension
					thumbSize = new Dimension(cte.DefaultThumbSize),
					mediumSize = new Dimension(cte.DetaultMediumSize),
					largeSize = new Dimension(cte.DetaultLargeSize);


				if (type != null)
				{
					if (!String.IsNullOrWhiteSpace(type.ThumbSize))
						thumbSize = new Dimension(type.ThumbSize);

					if (!String.IsNullOrWhiteSpace(type.MediumSize))
						mediumSize = new Dimension(type.MediumSize);

					if (!String.IsNullOrWhiteSpace(type.LargeSize))
						largeSize = new Dimension(type.LargeSize);
				}

				if (cropOptions == null)
					GraphicUtils.ImageUtils.CropImage(OriginalImage, thumbName, thumbSize.IntWidth, thumbSize.IntHeight, ImageUtils.AnchorPosition.Default);
				else
					GraphicUtils.ImageUtils.CropImage(OriginalImage, thumbName, thumbSize.IntWidth, thumbSize.IntHeight, ImageUtils.AnchorPosition.Custom, cropOptions);

				GraphicUtils.ImageUtils.Resize(OriginalImage, mediumName, mediumSize.IntWidth, mediumSize.IntHeight);
				GraphicUtils.ImageUtils.Resize(OriginalImage, largeName, largeSize.IntWidth, largeSize.IntHeight);

				page.Image = imagePart + imageExtension;
			}
			else
				page.Image = "";

			PagesData.SubmitChanges();
		}

		public void DeletePage(int PageId)
		{
			var page = PagesData.Pages.Single(p => p.PageId == PageId);
			string albumName = string.Format("{0}-{1}", page.URL, PageId);


			//No permission to edit or delete the page.
			//TODO: add notification about no permission
			if (page.EditingRoles != null && page.EditingRoles != 0 && ((page.EditingRoles.Value & WebContext.Profile.Roles) == 0))
				return;


			PagesData.Pages.DeleteOnSubmit(page);
			PagesData.SubmitChanges();


			(new lw.PhotoAlbums.PhotoAlbumsManager()).DeleteAlbum(albumName);

			string path = Path.Combine(Folders.PagesFolder, "Page_" + PageId.ToString());
			path = WebContext.Server.MapPath(Path.Combine("~/", path));
			if (Directory.Exists(path))
				Directory.Delete(path, true);
		}

		public bool DeletePageType(int TypeId)
		{
			PageType pageType = this.GetPageType(TypeId);
			var _pages = PagesData.Pages.Where(x => x.PageType == TypeId);
			foreach (var _page in _pages)
			{
				_page.PageType = null;
			}
			PagesData.PageTypes.DeleteOnSubmit(pageType);
			PagesData.SubmitChanges();
			return true;
		}

		public bool DeletePageTemplate(int TemplateId)
		{
			PageTemplate item = this.GetPageTemplate(TemplateId);
			var _pages = PagesData.Pages.Where(x => x.PageTemplate == TemplateId);
			foreach (var _page in _pages)
			{
				_page.PageTemplate = null;
			}
			PagesData.PageTemplates.DeleteOnSubmit(item);
			PagesData.SubmitChanges();
			return true;
		}

		public void DeleteImage(int PageId)
		{
			var page = PagesData.Pages.Single(p => p.PageId == PageId);

			//No permission to edit or delete the page.
			//TODO: add notification about no permission
			if (page.EditingRoles != null && page.EditingRoles != 0 && ((page.EditingRoles.Value & WebContext.Profile.Roles) == 0))
				return;


			string imagePart = Path.GetFileNameWithoutExtension(page.Image);
			string imageExtension = Path.GetExtension(page.Image);

			string path = Folders.PagesFolder;

			path = Path.Combine(path, string.Format("Page_{0}", PageId));
			path = lw.WebTools.WebContext.Server.MapPath("~/" + path);

			string thumbName = Path.Combine(path, string.Format("{0}-t{1}", imagePart, imageExtension));
			string largeName = Path.Combine(path, string.Format("{0}-l{1}", imagePart, imageExtension));
			string mediumName = Path.Combine(path, string.Format("{0}-m{1}", imagePart, imageExtension));


			if (!String.IsNullOrWhiteSpace(page.Image))
			{
				try
				{
					if (File.Exists(Path.Combine(path, page.Image)))
						File.Delete(Path.Combine(path, page.Image));

					if (File.Exists(Path.Combine(path, thumbName)))
						File.Delete(Path.Combine(path, thumbName));

					if (File.Exists(Path.Combine(path, mediumName)))
						File.Delete(Path.Combine(path, mediumName));

					if (File.Exists(Path.Combine(path, largeName)))
						File.Delete(Path.Combine(path, largeName));
				}
				catch
				{

				}
			}

			page.Image = null;
			PagesData.SubmitChanges();
		}



		/// <summary>
		/// Returns all the ansectors or parents of that page.
		/// </summary>
		/// <param name="PageId">The ID of the page</param>
		/// <returns></returns>
		public IQueryable<PageAncestors> GetPageAncestors(int? PageId)
		{
			return PagesData.Pages_GetAncestors(PageId);
		}

		/// <summary>
		/// Returns all the ansectors or parents of that page.
		/// </summary>
		/// <param name="PageId">The URL (unique name) or Title of the page.</param>
		/// <returns></returns>
		public IQueryable<PageAncestors> GetPageAncestors(string Page)
		{
			return PagesData.Pages_GetAncestors(GetPage(Page).PageId);
		}

		/// <summary>
		/// Returns all the descendents or children of that page.
		/// </summary>
		/// <param name="PageId">The URL (unique name) or Title of the page.</param>
		/// <returns></returns>
		public IQueryable GetPageDescendents(string Page)
		{
			return PagesData.Pages_GetDescendants(GetPage(Page).PageId);
		}


		public data.Pages_View GetPageView(int PageId)
		{
			var query = from p in PagesData.Pages_Views
						where
						p.PageId == PageId
						select p;
			if (query.Count() > 0)
				return query.Single();
			return null;
		}


		public data.Pages_View_Comment GetPageViewComment(int PageId)
		{
			var query = from p in PagesData.Pages_View_Comments
						where
						p.PageId == PageId
						select p;
			if (query.Count() > 0)
				return query.Single();
			return null;
		}

		/// <summary>
		/// Gets the page details including "FullURL"
		/// </summary>
		/// <param name="Keyword">Page Title or URL</param>
		/// <returns>Pages_View Row or null if page not found</returns>
		public data.Pages_View GetPageView(string Keyword)
		{
			var query = from p in PagesData.Pages_Views
						where
						p.URL == Keyword ||
						p.Title == Keyword
						select p;

			if (query.Count() > 0)
			{
				var ret = query.First();
				if (query.Count() > 1)
				{
					foreach (data.Pages_View v in query)
						if (v.URL == Keyword)
						{
							ret = v;
							break;
						}
				}
				return ret;
			}
			return null;
		}

		/// <summary>
		/// Gets the page details including "FullURL" and Comments Count
		/// </summary>
		/// <param name="Keyword">Page Title or URL</param>
		/// <returns>Pages_View Row or null if page not found</returns>
		public data.Pages_View_Comment GetPageViewComment(string Keyword)
		{
			var query = from p in PagesData.Pages_View_Comments
						where
						p.URL == Keyword ||
						p.Title == Keyword
						select p;

			if (query.Count() > 0)
			{
				var ret = query.First();
				if (query.Count() > 1)
				{
					foreach (data.Pages_View_Comment v in query)
						if (v.URL == Keyword)
						{
							ret = v;
							break;
						}
				}
				return ret;
			}
			return null;
		}


		public DataTable GetPagesFromView(string cond)
		{
			string sql = "select * from Pages_View";
			if (!String.IsNullOrWhiteSpace(cond))
			{
				sql += " where  " + cond;
			}
			sql += " Order by PublishDate DESc";

			return DBUtils.GetDataSet(sql, cte.lib).Tables[0];
		}

		public IQueryable<data.Page> GetPages(int ParentId)
		{
			return from page in PagesData.Pages
				   orderby page.Ranking descending
				   where page.ParentId == ParentId
				   select page;
		}

		public data.Page GetPage(int PageId)
		{
			var query = from p in PagesData.Pages
						where
						p.PageId == PageId
						select p;
			if (query.Count() > 0)
				return query.Single();
			return null;
		}

		public data.Page GetPage(string Keyword)
		{
			var query = from p in PagesData.Pages
						where
						p.URL == Keyword ||
						p.Title == Keyword
						select p;
			if (query.Count() > 0)
				return query.First();
			return null;
		}

		public data.PageType GetPageType(int TypeId)
		{
			var query = from p in PagesData.PageTypes
						where
						p.TypeId == TypeId
						select p;
			if (query.Count() > 0)
				return query.First();
			return null;
		}

		public data.PageType GetPageType(string type)
		{
			var query = from t in PagesData.PageTypes
						where
						t.Type == type
						select t;
			if (query.Count() > 0)
				return query.First();
			return null;
		}

		public IQueryable<data.PageType> GetPageTypes()
		{
			return from types in PagesData.PageTypes
				   orderby types.Type ascending
				   select types;
		}

		public data.PageTemplate GetPageTemplate(int TemplateId)
		{
			var query = from p in PagesData.PageTemplates
						where
						p.TemplateId == TemplateId
						select p;
			if (query.Count() > 0)
				return query.Single();
			return null;
		}

		public data.PageTemplate GetPageTemplate(string title)
		{
			var query = from t in PagesData.PageTemplates
						where
						t.Title == title
						select t;
			if (query.Count() > 0)
				return query.Single();
			return null;
		}

		public IQueryable<data.PageTemplate> GetPageTemplates()
		{
			return from items in PagesData.PageTemplates
				   orderby items.Title ascending
				   select items;
		}

		public IQueryable<data.Page> GetPages()
		{
			return from pages in PagesData.Pages
				   orderby pages.Ranking descending
				   select pages;
		}

		public IQueryable<data.Page> GetPages(string Parent)
		{
			return from pages in PagesData.Pages
				   join pagesParents in PagesData.Pages
				   on pages.ParentId equals pagesParents.PageId
				   orderby pages.Ranking descending
				   where pagesParents.Title == Parent || pagesParents.URL == Parent
				   select pages;
		}

		public void UpdatePageType(int TypeId, string Title, string ThumbSize, string MediumSize, string LargeSize)
		{
			PageType type = GetPageType(TypeId);
			type.Type = Title;
			type.ThumbSize = ThumbSize;
			type.MediumSize = MediumSize;
			type.LargeSize = LargeSize;
			PagesData.SubmitChanges();
		}

		public void UpdatePageTemplate(int TemplateId, string Title, string Filename)
		{
			PageTemplate type = GetPageTemplate(TemplateId);
			type.Title = Title;
			type.Filename = Filename;
			PagesData.SubmitChanges();
		}

		public void UpdateProperties(int PageId, string Title, string Url, string Status, DateTime? PublishDate, string SmallDescription)
		{
			if (!String.IsNullOrWhiteSpace(Url))
			{
				Page p = GetPage(PageId);
				if (!String.IsNullOrWhiteSpace(Title) && (Url == StringUtils.ToURL(p.Title) && Title != p.Title))
					Url = null;
			}
			UpdateProperties(PageId, Title, Status, null, null, PublishDate, null, null,
				null, SmallDescription, null, null, null, Url, null);
		}

		public void UpdateProperties(int PageId, string Title, string Status, DateTime? PublishDate, string SmallDescription)
		{
			UpdateProperties(PageId, Title, null, Status, PublishDate, SmallDescription);
		}


		public void UpdateProperties(int PageId, string Title, string Status, DateTime? PublishDate, string SmallDescription, string PageContent)
		{
			UpdateProperties(PageId, Title, Status, PublishDate, SmallDescription, PageContent, null);
		}


		public void UpdateProperties(int PageId, string Title, string Header, string Status, DateTime? PublishDate, string SmallDescription, string PageContent)
		{
			UpdateProperties(PageId, Title, Status, null, null, PublishDate, null, null, null, SmallDescription, PageContent, null, null, null, Header);
		}

		public void UpdateProperties(int PageId, string Title, string Status, DateTime? PublishDate, string SmallDescription, string PageContent, int? parentId)
		{
			UpdateProperties(PageId, Title, Status, null, null, PublishDate, null, null, null, SmallDescription, PageContent, parentId);
		}

		public void UpdateProperties(int PageId, string Title, string Status, short? Language, int? PageType, DateTime? PublishDate, int? Ranking, int? Views, int? UserRating, string SmallDescription, string PageContent, int? parentId)
		{
			UpdateProperties(PageId, Title, Status, Language, PageType, PublishDate, Ranking, Views, UserRating, SmallDescription, PageContent, parentId, null);
		}

		public void UpdateProperties(int PageId, string Title, string Status, short? Language, int? PageType, DateTime? PublishDate, int? Ranking, int? Views, int? UserRating, string SmallDescription, string PageContent, int? parentId, int? PageTemplate)
		{
			UpdateProperties(PageId, Title, Status, Language, PageType, PublishDate, Ranking, Views, UserRating, SmallDescription, PageContent, parentId, PageTemplate, null);
		}

		public void UpdateProperties(int PageId, string Title, string Status, short? Language, int? PageType, DateTime? PublishDate, int? Ranking, int? Views, int? UserRating, string SmallDescription, string PageContent, int? parentId, int? PageTemplate, string URL)
		{
			UpdateProperties(PageId, Title, Status, Language, PageType, PublishDate, Ranking, Views, UserRating, SmallDescription, PageContent, parentId, PageTemplate, URL, null);
		}
		public void UpdateProperties(int PageId, string Title, string Status, short? Language, int? PageType, DateTime? PublishDate, int? Ranking, int? Views, int? UserRating, string SmallDescription, string PageContent, int? parentId, int? PageTemplate, string URL, string Header)
		{
			UpdateProperties(PageId, Title, Status, Language, PageType, PublishDate, Ranking, Views, UserRating, SmallDescription, PageContent, parentId, PageTemplate, URL, Header, "");
		}

		public void UpdateProperties(int PageId, string Title, string Status, short? Language, int? PageType, DateTime? PublishDate, int? Ranking, int? Views, int? UserRating, string SmallDescription, string PageContent, int? parentId, int? PageTemplate, string URL, string Header, string Tags)
		{
			UpdateProperties(PageId, Title, Status, Language, PageType, PublishDate, Ranking, Views, UserRating, SmallDescription, PageContent, parentId, PageTemplate, URL, Header, Tags, "");
		}
		public void UpdateProperties(int PageId, string Title, string Status, short? Language, int? PageType, DateTime? PublishDate, int? Ranking, int? Views, int? UserRating, string SmallDescription, string PageContent, int? parentId, int? PageTemplate, string URL, string Header, string Tags, string Keywords)
		{
			data.Page thisPage = GetPage(PageId);

			//No permission to edit or delete the page.
			//TODO: add notification about no permission
			if (thisPage.EditingRoles != null && thisPage.EditingRoles != 0 && ((thisPage.EditingRoles.Value & WebContext.Profile.Roles) == 0))
				return;

			if (parentId.HasValue)
				thisPage.ParentId = parentId;
			thisPage.Title = Title;
			thisPage.Status = (byte)(PageStatus)Enum.Parse(typeof(PageStatus), Status);
			if (Language.HasValue)
				thisPage.Language = Language;
			if (PageType.HasValue)
				thisPage.PageType = PageType;
			if (PageTemplate.HasValue)
				thisPage.PageTemplate = PageTemplate;
			if (PublishDate.HasValue)
				thisPage.PublishDate = PublishDate;
			if (Ranking.HasValue)
				thisPage.Ranking = Ranking;
			if (Views.HasValue)
				thisPage.Views = Views;
			if (UserRating.HasValue)
				thisPage.UserRating = UserRating;
			if (!String.IsNullOrWhiteSpace(SmallDescription))
				thisPage.SmallDescription = SmallDescription;
			if (Keywords != null)
				thisPage.Keywords = Keywords;

			thisPage.ModifiedBy = WebContext.Profile.UserId;
			thisPage.DateModified = DateTime.Now;

			if (PageContent != null)
				thisPage.PageContent = PageContent;
			if (String.IsNullOrWhiteSpace(URL))
				URL = StringUtils.ToURL(Title);
			thisPage.Tags = Tags;

			var checkPage = GetPage(URL);

			if(checkPage.PageId != PageId)
			{
				int i = 1;
				string temp;
				do
				{
					temp = URL + "_" + i.ToString();
					i++;
				}
				while (GetPage(temp) != null);
				URL = temp;
			}
			
			thisPage.URL = URL;
			if (!String.IsNullOrWhiteSpace(Header))
				thisPage.Header = Header;

			HashTagsManager htMgr = new HashTagsManager();
			var page = thisPage;
			Tags = Regex.Replace(Tags, @"\s+|;|,|#", " #");
			htMgr.UpdateTags(page.PageId, HashTagTypes.Pages, page.Title + " " + page.Header + page.PageContent + " " + page.SmallDescription + " " + "#" + Tags);

			PagesData.SubmitChanges();
		}


		public string TrancateDescription(string Text, int Length)
		{
			string _text = StringUtils.Trankate(Text, Length);
			string t = Regex.Replace(_text, @"\t|\n|\r", " ");
			string trimmedText = t.Trim();
			do
			{
				trimmedText = trimmedText.Replace("  ", " ");
			}
			while (trimmedText.IndexOf("  ") > 0);

			return trimmedText;
		}


		public void UpdatePageURL(int PageId, string URL)
		{
			data.Page thisPage = GetPage(PageId);

			//No permission to edit or delete the page.
			//TODO: add notification about no permission
			if (thisPage.EditingRoles != null && thisPage.EditingRoles != 0 && ((thisPage.EditingRoles.Value & WebContext.Profile.Roles) == 0))
				return;

			thisPage.URL = URL;
			PagesData.SubmitChanges();
		}


		public void SaveDraft(int PageId, string Header, string Text)
		{
			data.Page thisPage = GetPage(PageId);

			//No permission to edit or delete the page.
			//TODO: add notification about no permission
			if (thisPage.EditingRoles != null && thisPage.EditingRoles != 0 && ((thisPage.EditingRoles.Value & WebContext.Profile.Roles) == 0))
				return;

			if (thisPage.History == null)
			{
				thisPage.History = XElement.Parse("<xml><draft><Header /><PageContent /><User /></draft></xml>");
			}
			XElement draft = thisPage.History.Element("draft");
			if (draft == null)
			{
				draft = XElement.Parse("<draft><Header /><PageContent /><User /></draft>");
				thisPage.History.Add(draft);
			}
			XElement header = draft.Element("Header");
			XElement text = draft.Element("PageContent");
			XElement user = draft.Element("User");

			if (Text.ToLower() == "<p></p>" || Text.ToLower() == "<p><br /></p>" || Text.ToLower() == "<p><br></p>")
				Text = "";

			header.Value = Header;
			text.Value = Text;
			user.Value = WebContext.Profile.dbUserName;

			thisPage.ModifiedBy = WebContext.Profile.UserId;
			thisPage.DateModified = DateTime.Now;

			PagesData.SubmitChanges();

			UpdateXML(PageId, thisPage.History.ToString());
		}

		public void UpdateXML(int PageId, string Xml)
		{
			var page = GetPage(PageId);
			//No permission to edit or delete the page.
			//TODO: add notification about no permission
			if (page.EditingRoles != null && page.EditingRoles != 0 && ((page.EditingRoles.Value & WebContext.Profile.Roles) == 0))
				return;

			string sql = string.Format("Update Pages set History=N'{1}' where PageId={0}", PageId, StringUtils.SQLEncode(Xml));
			DBUtils.ExecuteQuery(sql, cte.lib);
		}


		public void Publish(int PageId, string Header, string Text)
		{
			//
			data.Page thisPage = GetPage(PageId);

			//No permission to edit or delete the page.
			//TODO: add notification about no permission
			if (thisPage.EditingRoles != null && thisPage.EditingRoles != 0 && ((thisPage.EditingRoles.Value & WebContext.Profile.Roles) == 0))
				return;

			if (thisPage.History == null)
			{
				thisPage.History = XElement.Parse("<xml><draft><Header /><PageContent /><User /></draft></xml>");
			}

			XElement draft = thisPage.History.Element("draft");
			if (draft != null)
				draft.Remove();

			string thisPageContent = "";

			try
			{
				if (thisPage.PageContent != null && thisPage.PageContent.ToLower() != "<p></p>" || thisPage.PageContent.ToLower() != "<p><br /></p>" || thisPage.PageContent.ToLower() != "<p><br></p>")
					thisPageContent = thisPage.PageContent;
			}
			catch
			{

			}

			XElement published = new XElement("published");
			published.Add(new XElement("Header", thisPage.Header));
			published.Add(new XElement("PageContent", thisPageContent));
			published.Add(new XElement("User", WebContext.Profile.dbUserName));
			published.Add(new XElement("Date", DateTime.Now));

			thisPage.History.Add(published);

			if (Text.ToLower() == "<p></p>" || Text.ToLower() == "<p><br /></p>" || Text.ToLower() == "<p><br></p>")
				Text = "";

			thisPage.Status = (byte)PageStatus.Published;
			thisPage.Header = Header;
			thisPage.PageContent = Text;
			thisPage.ModifiedBy = WebContext.Profile.UserId;
			thisPage.DateModified = DateTime.Now;

			if (thisPage.SmallDescription == null || String.IsNullOrWhiteSpace(thisPage.SmallDescription))
				thisPage.SmallDescription = TrancateDescription(Text, 512);

			PagesData.SubmitChanges();

			UpdateXML(PageId, thisPage.History.ToString());

			HashTagsManager htMgr = new HashTagsManager();
			var page = thisPage;
			htMgr.UpdateTags(page.PageId, HashTagTypes.Pages, page.Title + " " + page.Header + page.PageContent + " " + page.SmallDescription + " " + page.Tags);
		}

		public void DiscardDraft(int PageId)
		{
			data.Page thisPage = GetPage(PageId);

			//No permission to edit or delete the page.
			//TODO: add notification about no permission
			if (thisPage.EditingRoles != null && thisPage.EditingRoles != 0 && ((thisPage.EditingRoles.Value & WebContext.Profile.Roles) == 0))
				return;


			if (thisPage.History == null)
			{
				thisPage.History = XElement.Parse("<xml><draft><Header /><PageContent /><User /></draft></xml>");
			}

			XElement draft = thisPage.History.Element("draft");
			if (draft != null)
				draft.Remove();

			thisPage.ModifiedBy = WebContext.Profile.UserId;
			thisPage.DateModified = DateTime.Now;

			PagesData.SubmitChanges();

			UpdateXML(PageId, thisPage.History.ToString());
		}

		public void UpdateModified(int PageId)
		{
			string sql = string.Format("Update Pages set DateModified = getDate() where PageId={0}", PageId);
			DBUtils.ExecuteQuery(sql, cte.lib);
		}

		/// <summary>
		/// Update the User Rating of the page
		/// Usually When the user like a page it increment by 1, and it discrement by 1 if he a page he already like.
		/// </summary>
		/// <param name="PageId">PageId</param>
		/// <param name="Rate">Rate</param>
		public void UpdatePageRating(int PageId, int Rate)
		{
			string sql = "Update Pages set [UserRating]={1} where PageId={0}";
			sql = String.Format(sql, PageId, Rate);

			DBUtils.ExecuteQuery(sql, cte.lib);
		}

		/// <summary>
		/// Increment the views of the page
		/// This method is called everytime the page is displayed
		/// </summary>
		/// <param name="PageId">PageId</param>
		public void IncrementPageViews(int PageId)
		{
			string sql = "Update Pages set [Views]=[Views]+1 where PageId={0}";
			sql = String.Format(sql, PageId);

			DBUtils.ExecuteQuery(sql, cte.lib);
		}

		public void UpdateMenuRanking(string data)
		{

			var pages = data.Split(new string[] { "^" }, StringSplitOptions.RemoveEmptyEntries);

			int rank = 100000;
			int increment = 50;

			string sql = "Update Pages set Ranking={0}, ParentId={1} where PageId={2};";
			StringBuilder sb = new StringBuilder();

			foreach (string page in pages)
			{
				string[] temp = page.Split(',');
				if (temp.Length == 1)
					sb.AppendFormat(sql, rank, -1, page);
				else
					sb.AppendFormat(sql, rank, temp[0], temp[1]);
				rank -= increment;
				sb.AppendLine();
			}

			DBUtils.ExecuteQuery(sb.ToString(), cte.lib);

			//var rootSql = "update Pages set Ranking = rank from (values ";
			//var newsSql = "update Pages set Ranking = rank, ParentId = parent from (values ";
			//var parentMax = 10000;
			//var childrenMax = 20000;
			//foreach (var p in parents)
			//{
			//	var children = p.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
			//	rootSql += "(" + children[0] + ", " + parentMax-- + "),";
			//	if (children.Length > 1)
			//	{
			//		newsSql += "(" + children[1] + ", " + childrenMax-- + ", " + children[0] + "),";
			//	}
			//}
			//if (rootSql.EndsWith(","))
			//{
			//	rootSql = rootSql.Substring(0, rootSql.Length - 1);
			//}
			//rootSql += " ) p(i, rank) where i=PageId;";
			//if (newsSql.EndsWith(","))
			//{
			//	newsSql = newsSql.Substring(0, newsSql.Length - 1);
			//}
			//newsSql += " ) p(i, rank, parent) where i=PageId;";


			//DBUtils.GetDataSet(rootSql, cte.lib);
			//DBUtils.GetDataSet(newsSql, cte.lib);
		}

		/// <summary>
		/// returns the medium image of a page
		/// </summary>
		/// <param name="PageId"></param>
		/// <param name="Image"></param>
		/// <returns></returns>
		public static string GetMediumImage(int PageId, string Image)
		{
			if (String.IsNullOrEmpty(Image))
				return "";

			string imageName = Path.GetFileNameWithoutExtension(Image);
			string extension = Path.GetExtension(Image);

			string path = Path.Combine(Folders.PagesFolder, "Page_" + PageId.ToString());
			return Path.Combine(path, imageName + "-m" + extension).Replace("\\", "/");
		}

		/// <summary>
		/// returns the thumb image of a page
		/// </summary>
		/// <param name="PageId"></param>
		/// <param name="Image"></param>
		/// <returns></returns>
		public static string GetThumbImage(int PageId, string Image)
		{
			if (String.IsNullOrEmpty(Image))
				return "";

			string imageName = Path.GetFileNameWithoutExtension(Image);
			string extension = Path.GetExtension(Image);

			string path = Path.Combine(Folders.PagesFolder, "Page_" + PageId.ToString());
			return Path.Combine(path, imageName + "-t" + extension).Replace("\\", "/");
		}

		/// <summary>
		/// Returns the large image of a page
		/// </summary>
		/// <param name="PageId"></param>
		/// <param name="Image"></param>
		/// <returns></returns>
		public static string GetLargeImage(int PageId, string Image)
		{
			if (String.IsNullOrEmpty(Image))
				return "";

			string imageName = Path.GetFileNameWithoutExtension(Image);
			string extension = Path.GetExtension(Image);

			string path = Path.Combine(Folders.PagesFolder, "Page_" + PageId.ToString());
			return Path.Combine(path, imageName + "-l" + extension).Replace("\\", "/");
		}

		public static string GetImage(int PageId, string Image)
		{
			if (String.IsNullOrEmpty(Image))
				return "";

			string path = Path.Combine(Folders.PagesFolder, "Page_" + PageId.ToString());
			return Path.Combine(path, Image).Replace("\\", "/");
		}

		/// <summary>
		/// Gets the page properties by pageId
		/// </summary>
		/// <param name="PageId"></param>
		/// <returns>PageDataProperties table contains all the configuration elements / PageDataPropertyValues contains the values of the these element combined with pageId if any</returns>
		public Dictionary<string, string> GetPageProperties(int PageId)
		{
			var x = from p in PagesData.PageDataProperties
					join v in PagesData.PageDataPropertyValues on p.DataPropertyID equals v.DataPropertyID
					where v.PageID == PageId
					select new { Key = p.DataPropertyName, Value = v.DataPropertyValue };

			return x.ToDictionary(mc => mc.Key.ToString(),
								 mc => mc.Value.ToString(),
								 StringComparer.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Gets the property Id for a specific property
		/// </summary>
		/// <param name="DataPropertyName"></param>
		/// <returns></returns>
		public int GetPageDataProperty(string DataPropertyName)
		{
			var query = from p in PagesData.PageDataProperties
						where p.DataPropertyName == DataPropertyName
						select p;

			if (query.Count() > 0)
				return query.First().DataPropertyID;

			var property = new PageDataProperty
			{
				DataPropertyName = DataPropertyName
			};


			PagesData.PageDataProperties.InsertOnSubmit(property);
			PagesData.SubmitChanges();

			return property.DataPropertyID;
		}

		/// <summary>
		/// Creates a page property and add it to the page.
		/// </summary>
		/// <param name="PageId">Page Id</param>
		/// <param name="Property">The property</param>
		/// <param name="PropertyValue">The value of the property</param>
		public void UpdatePageProperty(int PageId, string Property, string PropertyValue)
		{
			int propertyId = GetPageDataProperty(Property);

			var query = from p in PagesData.PageDataPropertyValues
						where p.DataPropertyID == propertyId
						&& p.PageID == PageId
						select p;

			if (query.Count() > 0)
			{
				if (string.IsNullOrEmpty(PropertyValue))
					DeletePageProperties(PageId, propertyId);
				else
				{
					var propertyValue = query.First();
					propertyValue.DataPropertyValue = PropertyValue;
					PagesData.SubmitChanges();
				}
			}
			else
			{
				var property = new PageDataPropertyValue
				{
					PageID = PageId,
					DataPropertyID = propertyId,
					DataPropertyValue = PropertyValue
				};

				PagesData.PageDataPropertyValues.InsertOnSubmit(property);
				PagesData.SubmitChanges();
			}
		}


		/// <summary>
		/// Get a list of the available properties for a specific property
		/// </summary>
		/// <param name="DataPropertyName"></param>
		/// <returns></returns>
		public DataTable GetAvailablePageDataProperties(string DataPropertyName)
		{
			int propertyId = GetPageDataProperty(DataPropertyName);

			string sql = string.Format("select DataPropertyValue, Count(PageId) as Count from PageDataPropertiesView where DataPropertyID={0} Group By DataPropertyValue", propertyId);
			return DBUtils.GetDataSet(sql, cte.lib).Tables[0];
		}


		/// <summary>
		/// Add properties to a Page
		/// </summary>
		/// <param name="PageId"></param>
		/// <returns></returns>
		public void AddPageProperties(int PageId, Dictionary<string, string> properties)
		{
			foreach (KeyValuePair<string, string> property in properties)
			{
				PageDataPropertyValue item = new PageDataPropertyValue();

				var propID = (from p in PagesData.PageDataProperties
							  where p.DataPropertyName == property.Key
							  select p).FirstOrDefault();
				int propertyId = -1;
				if (propID == null)
					propertyId = GetPageDataProperty(property.Key);
				else
					propertyId = propID.DataPropertyID;
				item.PageID = PageId;
				item.DataPropertyID = propertyId;
				item.DataPropertyValue = property.Value;
				PagesData.PageDataPropertyValues.InsertOnSubmit(item);
				PagesData.SubmitChanges();
			}
		}

		/// <summary>
		/// Delete properties from a Page
		/// </summary>
		/// <param name="pageId"></param>
		/// <param name="propertyId"></param>
		public void DeletePageProperties(int pageId, int propertyId)
		{
			PageDataPropertyValue item = new PageDataPropertyValue();

			var propID = (from p in PagesData.PageDataPropertyValues
						  where p.DataPropertyID == propertyId && p.PageID == pageId
						  select p).FirstOrDefault();
			PagesData.PageDataPropertyValues.DeleteOnSubmit(propID);
			PagesData.SubmitChanges();
		}

		/// <summary>
		/// Updates the access roles and edit roles of the page.
		/// </summary>
		/// <param name="PageId">The PageId of the page</param>
		/// <param name="AccessRoleNames">The Access Roles of the page</param>
		/// <param name="EditRoleNames">The Edit Roles of the page</param>
		/// <returns></returns>
		public void UpdatePageRoles(int PageId, string[] AccessRoleNames, string[] EditRoleNames)
		{
			Roles accessAllRoles = Roles.Visitor;
			Roles editAllRoles = Roles.Visitor;
			Roles accessRole;
			Roles editRole;
			data.Page thisPage = GetPage(PageId);
			if (AccessRoleNames != null)
			{
				foreach (string accessRoleName in AccessRoleNames)
				{
					accessRole = (Roles)Enum.Parse(typeof(Roles), accessRoleName);
					accessAllRoles = accessAllRoles | accessRole;
				}
				thisPage.AccessRoles = (int)accessAllRoles;
			}

			if (EditRoleNames != null)
			{
				foreach (string editRoleName in EditRoleNames)
				{
					editRole = (Roles)Enum.Parse(typeof(Roles), editRoleName);
					editAllRoles = editAllRoles | editRole;
				}
				thisPage.EditingRoles = (int)editAllRoles;
			}

			thisPage.ModifiedBy = WebContext.Profile.UserId;
			thisPage.DateModified = DateTime.Now;

			PagesData.SubmitChanges();
		}
		public void UpdatePageRoles(int PageId, int AccessRoleNames, int EditRoleNames)
		{
			data.Page thisPage = GetPage(PageId);
			thisPage.AccessRoles = AccessRoleNames;
			thisPage.EditingRoles = EditRoleNames;
			PagesData.SubmitChanges();
		}


		#region Page Likes

		public void AddLike(int MemberId, int PageId, Likes like)
		{
			var query = from p in PagesData.Page_Likes
						where p.MemberId == MemberId
						&& p.PageId == PageId
						select p;

			if (query.Count() > 0)
			{
				var l = query.First();
				l.Feeling = (int)like;
			}
			else
			{
				var l = new Page_Like
				{
					PageId = PageId,
					MemberId = MemberId,
					Feeling = (int)like
				};

				PagesData.Page_Likes.InsertOnSubmit(l);
			}

			PagesData.SubmitChanges();
		}


		public void RemoveLike(int MemberId, int PageId)
		{
			var query = from p in PagesData.Page_Likes
						where p.MemberId == MemberId
						&& p.PageId == PageId
						select p;

			if (query.Count() > 0)
			{
				PagesData.Page_Likes.DeleteAllOnSubmit(query);
				PagesData.SubmitChanges();
			}
		}


		public int GetPageLikesCount(int PageId)
		{
			string sql = "select count(Feeling) as Likes from Page_Likes where PageId=" + PageId.ToString();
			DataTable table = DBUtils.GetDataSet(sql, cte.lib).Tables[0];
			return table.Rows.Count > 0 ? (int)table.Rows[0]["Likes"] : 0;
		}


		#endregion



		#region Page Images and Media
		public PageImage AddPageImage(int PageId, string Caption, string fileName, Stream BytesStream)
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

			var row = AddPageImage(PageId, Caption, path);
			File.Delete(path);
			return row;
		}

		public PageImage AddPageImage(int PageId, string Caption, HttpPostedFile Image, int Sort = 100000)
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

			var row = AddPageImage(PageId, Caption, path, Sort);
			File.Delete(path);
			return row;
		}


		public PageImage AddPageImage(int PageId, string Caption, string image, int Sort = 100000)
		{
			var pageImage = new PageImage
			{
				PageId = PageId,
				Caption = Caption,
				Sort = Sort,
				DateAdded = DateTime.Now,
				DateModified = DateTime.Now
			};


			PagesData.PageImages.InsertOnSubmit(pageImage);
			PagesData.SubmitChanges();


			string path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.PagesFolder);
			path = System.IO.Path.Combine(path, string.Format("Page_{0}", PageId));
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

			string _ImageName = string.Format("{0}_{1}.{2}", StringUtils.ToURL(fileName), pageImage.ImageId, ext);

			string ImageName = string.Format("{0}\\{1}", originalPath, _ImageName);
			string largeImageName = string.Format("{0}\\{1}", largePath, _ImageName);
			string thumbImageName = string.Format("{0}\\{1}", thumbPath, _ImageName);

			if (!Directory.Exists(originalPath))
				Directory.CreateDirectory(originalPath);

			System.IO.File.Copy(image, ImageName);

			try
			{
				Dimension largeImageSize = new Dimension(lw.PhotoAlbums.cte.ImageDefaultSize);
				Dimension thumbImageSize = new Dimension(lw.PhotoAlbums.cte.ImageDefaultThumbSize);

				if (!string.IsNullOrWhiteSpace(cfg.GetKey(lw.PhotoAlbums.cte.ImageSize)))
					largeImageSize = new Dimension(cfg.GetKey(lw.PhotoAlbums.cte.ImageSize));
				if (!string.IsNullOrWhiteSpace(cfg.GetKey(lw.PhotoAlbums.cte.ImageThumbSize)))
					thumbImageSize = new Dimension(cfg.GetKey(lw.PhotoAlbums.cte.ImageThumbSize));

				lw.GraphicUtils.ImageUtils.Resize(ImageName, largeImageName, largeImageSize.IntWidth, largeImageSize.IntHeight);

				if (!string.IsNullOrWhiteSpace(cfg.GetKey(lw.PhotoAlbums.cte.ImageThumbOption)) && cfg.GetKey(lw.PhotoAlbums.cte.ImageThumbOption) == "Crop")
					lw.GraphicUtils.ImageUtils.SmartCrop(ImageName, thumbImageName, thumbImageSize.IntWidth, thumbImageSize.IntHeight);
				else
					lw.GraphicUtils.ImageUtils.Resize(ImageName, thumbImageName, thumbImageSize.IntWidth, thumbImageSize.IntHeight);
			}
			catch (Exception ex)
			{
				ErrorContext.Add("resize-image", "Unable to resize album image.<br><span class=hid>" + ex.Message + "</span>");
			}

			try
			{
				var dimensions = lw.GraphicUtils.Dimensions.GetDimensions(ImageName);

				pageImage.Width = dimensions.Width;
				pageImage.Height = dimensions.Height;
			}
			catch (Exception)
			{

			}
			pageImage.FileName = _ImageName;
			PagesData.SubmitChanges();

			return pageImage;
		}

		public static string GetImagesPath(int PageId)
		{
			string path = WebContext.Root + "/" + CTE.Folders.PagesFolder;
			path = string.Format("{0}/Page_{1}", path, PageId);
			return path;
		}

		public List<PageImageView> GetPageImages(int PageId)
		{
			var images = from image in PagesData.PageImages
						 where image.PageId == PageId
						 orderby image.Sort
						 select image;


			string path = GetImagesPath(PageId);

			var list = new List<PageImageView>();

			foreach (PageImage image in images)
			{
				list.Add(new PageImageView
				{
					Thumb = string.Format("{0}/Thumb/{1}", path, image.FileName),
					Large = string.Format("{0}/Large/{1}", path, image.FileName),
					Original = string.Format("{0}/Original/{1}", path, image.FileName),
					ImageId = image.ImageId,
					PageId = image.PageId,
					Sort = image.Sort,
					Caption = image.Caption,
					FileName = image.FileName,
					Width = image.Width,
					Height = image.Height
				});
			}

			return list;
		}

		public PageImageView GetPageImage(int ImageId)
		{
			var images = from im in PagesData.PageImages
						 where im.ImageId == ImageId
						 select im;

			PageImageView ret = null;


			if (images.Count() > 0)
			{
				var image = images.First();
				string path = GetImagesPath(image.PageId.Value);
				ret = new PageImageView
				{
					Thumb = string.Format("{0}/Thumb/{1}", path, image.FileName),
					Large = string.Format("{0}/Large/{1}", path, image.FileName),
					Original = string.Format("{0}/Original/{1}", path, image.FileName),
					ImageId = image.ImageId,
					PageId = image.PageId,
					Sort = image.Sort,
					Caption = image.Caption,
					FileName = image.FileName,
					Width = image.Width,
					Height = image.Height
				};
			}
			return ret;
		}

		public bool DeletePageImage(int ImageId)
		{
			PageImageView image = GetPageImage(ImageId);

			if (image == null)
				return false;

			string sql = "Delete From PageImages where ImageId = " + ImageId.ToString();
			DBUtils.ExecuteQuery(sql, cte.lib);

			var path = WebContext.Server.MapPath(image.Thumb);
			if (File.Exists(path))
				File.Delete(path);

			path = WebContext.Server.MapPath(image.Large);
			if (File.Exists(path))
				File.Delete(path);

			path = WebContext.Server.MapPath(image.Original);
			if (File.Exists(path))
				File.Delete(path);

			return true;
		}

		public void SaveImagesOrder(string ids)
		{
			string[] _ids = ids.Split('|');
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < _ids.Length; i++)
			{
				try
				{
					int id = Int32.Parse(_ids[i]);
					sb.Append(string.Format(
						"Update PageImages set Sort={0} where ImageId={1};",
						i + 1,
						id));
				}
				catch
				{
				}
			}

			DBUtils.ExecuteQuery(sb.ToString(), cte.lib);
		}

		#endregion



		#region PagesExtendedProperties



		/// <summary>
		/// Gets the page extended properties by pageId
		/// </summary>
		/// <param name="PageId"></param>
		/// <returns>PageDataProperties table contains all the configuration elements / PageDataPropertyValues contains the values of the these element combined with pageId if any</returns>
		public Dictionary<string, string> GetPageExtendedProperties(int PageId)
		{
			var x = from p in PagesData.PageExtendedDataProperties
					join v in PagesData.PageExtendedDataPropertyValues on p.DataPropertyID equals v.DataPropertyID
					where v.PageID == PageId
					select new { Key = p.DataPropertyName, Value = v.DataPropertyValue };

			return x.ToDictionary(mc => mc.Key.ToString(),
								 mc => mc.Value.ToString(),
								 StringComparer.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Gets the property Id for a specific property
		/// </summary>
		/// <param name="DataPropertyName"></param>
		/// <returns></returns>
		public int GetPageExntendedDataProperty(string DataPropertyName)
		{
			var query = from p in PagesData.PageExtendedDataProperties
						where p.DataPropertyName == DataPropertyName
						select p;

			if (query.Count() > 0)
				return query.First().DataPropertyID;

			var property = new PageExtendedDataProperty
			{
				DataPropertyName = DataPropertyName
			};


			PagesData.PageExtendedDataProperties.InsertOnSubmit(property);
			PagesData.SubmitChanges();

			return property.DataPropertyID;
		}


		/// <summary>
		/// Update properties of a Page
		/// </summary>
		/// <param name="PageId"></param>
		/// <returns></returns>
		public void UpdatePageExtendedProperties(int PageId, Dictionary<string, string> properties)
		{
			foreach (KeyValuePair<string, string> property in properties)
			{
				PageExtendedDataPropertyValue item = new PageExtendedDataPropertyValue();

				var propID = (from p in PagesData.PageExtendedDataProperties
							  where p.DataPropertyName == property.Key
							  select p).FirstOrDefault();
				if (propID != null)
				{
					var propVal = (from p in PagesData.PageExtendedDataPropertyValues
								   where p.DataPropertyID == propID.DataPropertyID
								   && p.PageID == PageId
								   select p).FirstOrDefault();

					if (propVal != null)
					{
						if (propVal.DataPropertyValue != property.Value)
						{
							propVal.DataPropertyValue = property.Value;

							PagesData.SubmitChanges();
						}
					}
					else
					{
						item.PageID = PageId;
						item.DataPropertyID = propID.DataPropertyID;
						item.DataPropertyValue = property.Value;
						PagesData.PageExtendedDataPropertyValues.InsertOnSubmit(item);
						PagesData.SubmitChanges();
					}
				}
			}
		}

		#endregion

		#region Variables

		public data.PagesDataContext PagesData
		{
			get
			{
				if (_dataContext == null)
					_dataContext = new data.PagesDataContext(this.Connection);
				return (data.PagesDataContext)_dataContext;
			}
		}

		#endregion
	}
}