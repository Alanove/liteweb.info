using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Caching;
using lw.CTE;
using lw.CTE.Enum;
using lw.Utils;
using lw.WebTools;

namespace lw.Content
{
	public class ContentManager
	{
		PagesDS ds;

		string config = lw.CTE.Content.Pages;
		public ContentManager()
		{
		}

		#region Page Methods
		public bool AddPage(string Name, string Title, string Description, bool Virtual, string content)
		{
			return AddPage(Name, Title, Description, Virtual, content, Languages.Default);
		}
		public bool AddPage(string Name, string Title, string Description, bool Virtual, string content, Languages language)
		{
			if (StringUtils.IsNullOrWhiteSpace(Name))
				return false;

			if (this.GetPages("Name='" + StringUtils.SQLEncode(Name) + "'").Count > 0)
			{
				ErrorContext.Add("validation", "Another page with the same name already exists");
				return false;
			}

			PagesDS.PagesRow row = DS.Pages.NewPagesRow();

			row.Name = Name;
			row.Title = Title;
			row.Description = Description;
			row.Virtual = Virtual;
			row.LastModified = DateTime.Now;
			row.Keywords = "";

			DS.Pages.Rows.Add(row);

			AcceptChanges();

			CreatePageFile(Name, content, language);

			return true;
		}
		public bool UpdatePageProperties(int PageId, string Name, string Title, string Description, string Keywords)
		{
			if (this.GetPages(string.Format("Name='{0}' And PageId<>{1}", Name, PageId)).Count > 0)
			{
				ErrorContext.Add("validation", "Another page with the same name already exists");
				return false;
			}

			PagesDS.PagesRow row = GetPage(PageId);

			row.Name = Name;
			row.Title = Title;
			row.Description = Description;
			row.LastModified = DateTime.Now;
			row.Keywords = Keywords;

			row.Table.AcceptChanges();
			AcceptChanges();

			return true;
		}

		public bool UpdatePage(int PageId, string Name, string Title, string Description, string Keywords, bool Virtual, string content)
		{
			if (this.GetPages(string.Format("Name='{0}' And PageId<>{1}", Name, PageId)).Count > 0)
			{
				ErrorContext.Add("validation", "Another page with the same name already exists");
				return false;
			}

			PagesDS.PagesRow row = GetPage(PageId);

			row.Name = Name;
			row.Title = Title;
			row.Description = Description;
			row.Virtual = Virtual;
			row.LastModified = DateTime.Now;
			row.Keywords = Keywords;

			row.Table.AcceptChanges();
			AcceptChanges();

			CreatePageFile(Name, content);

			return true;
		}

		void CreatePageFile(string name, string content)
		{
			CreatePageFile(name, content, Languages.Default);
		}
		void CreatePageFile(string name, string content, Languages language)
		{
			try
			{
				content = content.Replace("=\"/" + WebContext.Root + "/", "=\"/");
				string path = Path.Combine(WebContext.Server.MapPath(WebContext.StartDir), ConfigCte.StaticContentFolder);

				//if (language != Languages.Default)
				//path = Path.Combine(path, EnumHelper.GetDescription(language));

				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);

				path = Path.Combine(path, name.Trim() + ".htm");
				StreamWriter str = new StreamWriter(path, false, Encoding.UTF8);
				str.Write(content);
				str.Close();

				string bk = Path.Combine(WebContext.Server.MapPath(WebContext.StartDir), ConfigCte.StaticContentFolder);
				bk = Path.Combine(bk, "_backups");
				if (!Directory.Exists(bk))
				{
					Directory.CreateDirectory(bk);
				}

				bk = Path.Combine(bk, name.Trim());
				if (!Directory.Exists(bk))
				{
					Directory.CreateDirectory(bk);
				}
				DirectoryInfo _backups = new DirectoryInfo(bk);

				FileInfo[] backups = _backups.GetFiles();

				if (backups.Length >= 15)
				{
					DateTime minDate = backups[0].CreationTime;
					FileInfo wastedFile = backups[0];
					foreach (FileInfo _file in backups)
					{
						if (minDate < _file.CreationTime)
							continue;
						wastedFile = _file;
					}
					File.Delete(wastedFile.FullName);
				}
				if (File.Exists(path))
				{
					try
					{
						File.Copy(path, Path.Combine(bk, DateTime.Now.ToLongDateString() + "-" + System.Guid.NewGuid() + ".htm"), true);
					}
					catch (Exception ex)
					{
						//TODO: silent handle of error
					}
				}
			}
			catch (Exception ex)
			{
				//throw (ex);
			}
		}

		public PagesDS.PagesRow GetPage(string PageName)
		{
			return GetPage(PageName, Languages.Default);
		}
		public PagesDS.PagesRow GetPage(string PageName, Languages language)
		{
			return GetPage(-1, PageName, true, language);
		}
		public PagesDS.PagesRow GetPage(int PageId)
		{
			return GetPage(PageId, "", false);
		}
		public PagesDS.PagesRow GetPage(int PageId, string PageName, bool GetContent)
		{
			return GetPage(PageId, PageName, GetContent, Languages.Default);
		}
		public PagesDS.PagesRow GetPage(int PageId, string PageName, bool GetContent, Languages language)
		{
			if (StringUtils.IsNullOrWhiteSpace(PageName) && PageId <= 0)
				return null;

			//if (language != Languages.Default)
				//PageName = EnumHelper.GetDescription(language) + "-" + PageName;

			string cond = "";
			if (PageId > 0)
				cond = "PageId=" + PageId.ToString();
			else
				cond = "Name='" + StringUtils.SQLEncode(PageName) + "'";

			DataView pages = GetPages(cond);
			if (pages.Count > 0)
			{
				PagesDS.PagesRow page = (PagesDS.PagesRow)pages[0].Row;
				if (GetContent)
				{
					string fileName = page.Name;

					if (page["FileName"] != System.DBNull.Value)
					{
						if (!StringUtils.IsNullOrWhiteSpace(page.FileName))
						{
							fileName = page.FileName;
						}
					}

					page.Content = Content(fileName, language);
				}
				return page;
			}
			return null;
		}
		public DataView GetPages()
		{
			return GetPages("");
		}
		public DataView GetPages(string cond)
		{
			return new DataView(DS.Pages, cond, "Name", DataViewRowState.CurrentRows);
		}

		public PagesDS.PagesRow GetPageByFileName(string fileName)
		{
			return GetPage(Path.GetFileNameWithoutExtension(fileName));
		}

		public DataView GetGroups(string cond)
		{
			return new DataView(DS.Groups, cond, "", DataViewRowState.CurrentRows);
		}
		public PagesDS.GroupsRow GetGroup(int GroupId)
		{
			DataView groups = GetGroups("GroupId=" + GroupId.ToString());
			if (groups.Count > 0)
				return (PagesDS.GroupsRow)groups[0].Row;
			return null;
		}
		public string GetGroupPath(PagesDS.GroupsRow group)
		{
			string path = group.Path;

			if (group["Parent"] != System.DBNull.Value)
			{
				return GetGroupPath(GetGroup(group.Parent)) + "/" + path;
			}
			return path;
		}

		public string GetPagePath(int PageId)
		{
			PagesDS.PagesRow page = GetPage(PageId);

			if (page != null)
			{
				if (page["Url"] != System.DBNull.Value)
				{
					return WebContext.Root + "/" + page["Url"].ToString();
				}
				if (page["GroupId"] != System.DBNull.Value)
				{
					PagesDS.GroupsRow group = GetGroup(page.GroupId);
					string groupPath = GetGroupPath(group);

					string pagePath = page.Name.Replace(StringUtils.ToURL(groupPath) + "-", "");

					return groupPath + "/" + pagePath;
				}
				
				return null;
			}

			ErrorContext.Add("page-not-found", new Exception("The Id supplied does not exist"));
			return null;
		}

		public void AcceptChanges()
		{
			try
			{
				DS.AcceptChanges();
				XmlManager.SetDataSet(config, DS);
			}
			catch (Exception Ex)
			{
				ErrorContext.Add("Pages-Save", "Could not save pages.config.<BR>" + Ex.Message);
			}
		}

		public void DeletePage(string PageName)
		{
			try
			{
				string path = Path.Combine(WebContext.Server.MapPath(WebContext.StartDir), ConfigCte.StaticContentFolder);

				path = Path.Combine(path, PageName.Trim() + ".htm");

				if (File.Exists(path))
					File.Delete(path);

				PagesDS.PagesRow row = GetPage(PageName);
				if (row.Table.Rows.Count > 0)
				{
					row.Delete();
					row.Table.AcceptChanges();
					AcceptChanges();
				}
			}
			catch (Exception ex)
			{

			}

		}

		#endregion

		#region Pages DataSet
		public PagesDS DS
		{
			get
			{
				if (ds == null)
				{
					ds = new PagesDS();
					DataSet _ds = XmlManager.GetDataSet(config);
					if (_ds != null)
						ds.Merge(_ds);
				}
				return ds;
			}
		}
		#endregion

		#region Direct Static Content

		//real content or pages
		public static string Content(string PageName)
		{
			return Content(PageName, Languages.Default);
		}
		public static string Content(string PageName, Languages language)
		{
			if (StringUtils.IsNullOrWhiteSpace(PageName))
				return "";

			string path = WebContext.Server.MapPath("~");
			path = Path.Combine(path, ConfigCte.StaticContentFolder);
			/*
			if (language != Languages.Default)
			{
				path = Path.Combine(path, EnumHelper.GetDescription(language));
			}
			 * */

			path = Path.Combine(path, PageName + ".htm");

			if (WebContext.Cache[PageName + "-" + language.ToString()] != null)
				return WebContext.Cache[PageName].ToString();

			string ret = "";
			if (System.IO.File.Exists(path))
			{
				StreamReader str = new StreamReader(path, Encoding.UTF8);
				ret = str.ReadToEnd();
				str.Close();
			}

			ret = ret.Replace("{root}", WebContext.Root);

			CacheDependency dep = new CacheDependency(path);

			WebContext.Cache.Add(PageName, ret, dep, Cache.NoAbsoluteExpiration,
				TimeSpan.FromDays(30), CacheItemPriority.Default, null);

			return ret;
		}

		
		/// <summary>
		/// //Messages (like warning or approval messages)
		/// </summary>
		/// <param name="msg">message</param>
		/// <returns>Content of the message</returns>
		public static string Message(string msg)
		{
			return Message(msg, Languages.Default);
		}

		/// <summary>
		/// //Messages (like warning or approval messages)
		/// </summary>
		/// <param name="msg">message</param>
		/// <param name="language">Language for the message</param>
		/// <returns>Content of the message</returns>
		public static string Message(string msg, Languages language)
		{
			string path = Path.Combine(WebContext.Server.MapPath(WebContext.StartDir), ConfigCte.MessagesFolder);
			/*
			if (language != Languages.Default)
			{
				path = Path.Combine(path, EnumHelper.GetDescription(language));
			}
			 * */

			path = Path.Combine(path, msg + ".htm");

			if (WebContext.Cache[msg + "-" + language.ToString()] != null)
				return WebContext.Cache[msg].ToString();

			string ret = "";

			if (System.IO.File.Exists(path))
			{
				StreamReader str = new StreamReader(path, Encoding.UTF8);
				ret = str.ReadToEnd();
				ret = ret.Replace("{Root}", WebContext.Root);
				str.Close();
			}

			ret = StringUtils.AddSup(ret);

			CacheDependency dep = new CacheDependency(path);

			int cacheTime = 1;

			string obj = Config.GetFromWebConfig("ContentCachTime");
			if (obj != null)
			{
				cacheTime = Int32.Parse(obj);
			}

			WebContext.Cache.Add(msg, ret, dep, Cache.NoAbsoluteExpiration,
				TimeSpan.FromDays(cacheTime), CacheItemPriority.Default, null);

			return ret;
		}

		//Error or validation messages
		public static string ErrorMsg(string Error)
		{
			return ErrorMsg(Error, Languages.Default);
		}

		public static string ErrorMsg(string Error, Languages language)
		{
			string path = Path.Combine(WebContext.Server.MapPath(WebContext.StartDir), ConfigCte.ErrorsFolder);
			/*
			if (language != Languages.Default)
			{
				path = Path.Combine(path, EnumHelper.GetDescription(language));
			}
			*/
			path = Path.Combine(path, Error + ".htm");

			if (WebContext.Cache[Error + "-" + language.ToString()] != null)
				return WebContext.Cache[Error].ToString();

			string ret = "";

			if (System.IO.File.Exists(path))
			{
				StreamReader str = new StreamReader(path, Encoding.UTF8);
				ret = str.ReadToEnd();
				ret = ret.Replace("{Root}", WebContext.Root);
				str.Close();
			}

			CacheDependency dep = new CacheDependency(path);

			int cacheTime = 1;

			string obj = Config.GetFromWebConfig("ContentCachTime");
			if (obj != null)
			{
				cacheTime = Int32.Parse(obj);
			}

			WebContext.Cache.Add(Error, ret, dep, Cache.NoAbsoluteExpiration,
				TimeSpan.FromDays(cacheTime), CacheItemPriority.Default, null);

			return ret;
		}
		#endregion
	}
}