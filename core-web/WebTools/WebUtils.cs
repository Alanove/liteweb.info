using System;
using System.Net;
using System.Web;
using System.Web.UI;
using lw.CTE;
using lw.Utils;

namespace lw.WebTools
{
	public static class WebUtils
	{
		private static System.Web.HttpServerUtility ServerUtility
		{
			get
			{
				return System.Web.HttpContext.Current.Server;
			}
		}
		public static string ServerMapPath (string FileName)
		{
			return WebUtils.GetServerMapPath(FileName);
		}
		public static string GetServerMapPath(string FileName)
		{
			return(ServerUtility.MapPath(FileName));
		}
		public static void HandleException(Exception exp)
		{
			throw new Exception(exp.Message);
		}
		

		public static string ErrorMsg(string msg)
		{
			return msg;
		}
		public static string InfoMsg(string msg)
		{
			return msg;
		}
		public static string SuccessMsg(string msg)
		{
			return msg;
		}
		
		public static string AbsolutePath
		{
			get
			{
				string _temp = WebContext.Request.ServerVariables["URL"];
				return _temp.Substring(0, _temp.LastIndexOf("/")+1);
			}
		}


		/// <summary>
		/// Returns a site relative HTTP path from a partial path starting out with a ~.
		/// Same syntax that ASP.Net internally supports but this method can be used
		/// outside of the Page framework.
		/// 
		/// Works like Control.ResolveUrl including support for ~ syntax
		/// but returns an absolute URL.
		/// </summary>
		/// <param name="originalUrl">Any Url including those starting with ~</param>
		/// <returns>relative url</returns>
		public static string ResolveUrl(string originalUrl)
		{
			if (originalUrl == null)
				return null;
			// *** Absolute path - just return
			if (originalUrl.IndexOf("://") != -1)
				return originalUrl;

			// *** Fix up image path for ~ root app dir directory
			if (originalUrl.StartsWith("~"))
			{
				string newUrl = "";
				if (HttpContext.Current != null)
					newUrl = HttpContext.Current.Request.ApplicationPath +
						 originalUrl.Substring(1).Replace("//", "/");
				else
					// *** Not context: assume current directory is the base directory
					throw new ArgumentException("Invalid URL: Relative URL not allowed.");


				// *** Just to be sure fix up any double slashes
				return newUrl;
			}
			return originalUrl;
		}

		/// <summary>
		/// This method returns a fully qualified absolute server Url which includes
		/// the protocol, server, port in addition to the server relative Url.
		/// 
		/// Works like Control.ResolveUrl including support for ~ syntax
		/// but returns an absolute URL.
		/// </summary>
		/// <param name="ServerUrl">Any Url, either App relative or fully qualified</param>
		/// <param name="forceHttps">if true forces the url to use https</param>
		/// <returns></returns>
		public static string ResolveServerUrl(string serverUrl, bool forceHttps)
		{

			// *** Is it already an absolute Url?
			if (serverUrl.IndexOf("://") > -1)
				return serverUrl;

			// *** Start by fixing up the Url an Application relative Url
			string newUrl = ResolveUrl(serverUrl);

			Uri originalUri = HttpContext.Current.Request.Url;
			newUrl = (forceHttps ? "https" : originalUri.Scheme) +
					 "://" + originalUri.Authority + newUrl;

			return newUrl;
		} 


		/// <summary>
		/// Load a url from any given link
		/// </summary>
		/// <param name="urlLink">example: http://www.google.com</param>
		/// <returns>The html content (string)</returns>
		public static string GetURLContent(string urlLink)
		{
			if (urlLink.IndexOf("//") == 0)
				urlLink = "https:" + urlLink;

			WebClient webClient = new WebClient();
			return webClient.DownloadString(urlLink);
		}


		/// <summary>
		/// Checks if the browser has cookies enabled
		/// </summary>
		public static bool IsCookiesEnabled
		{
			get
			{
				bool isCookiesEnabled = false;
				if (WebContext.Request.Cookies[Variables.TestCookieId] != null)
				{
					HttpCookie cookie = WebContext.Request.Cookies[Variables.TestCookieId];
					isCookiesEnabled = cookie.Value == Variables.TestCookieValue;
				}
				return isCookiesEnabled;
			}
		}


		/// <summary>
		/// Directly Response Writes any object in the form of a Javascript JSon object {a:[1,2,3]...}
		/// </summary>
		/// <param name="obj">Any Object</param>
		public static void JSonSerialize(object obj)
		{
			WebContext.Response.Write(StringUtils.JSonSerialize(obj));
		}

		/// <summary>
		/// Returns a variable from app.config
		/// </summary>
		/// <param name="paramName">Parameter Name</param>
		/// <returns>Parameter Value</returns>
		public static string GetFromWebConfig(string paramName)
		{
			return Config.GetFromWebConfig(paramName);
		}


		/// <summary>
		/// Returns the parameter from App.Config
		/// If the value is not found the routing value is returned
		/// </summary>
		/// <param name="cteConst">Parameter Name</param>
		/// <returns>Parameter Value</returns>
		public static string GetParameter(string cteConst)
		{
			string ret = Config.GetFromWebConfig(cteConst);

			return !String.IsNullOrWhiteSpace(ret) ? ret : cteConst;
		}


		/// <summary>
		/// Extension method that will recursively search the control's children for a control with the given ID.
		/// </summary>
		/// <param name="parent">The control who's children should be searched</param>
		/// <param name="controlID">The ID of the control to find</param>
		/// <returns></returns>
		public static Control FindControlRecursive(this Control parent, string ID)
		{
			if (!String.IsNullOrEmpty(parent.ID) && parent.ID.Equals(ID)) 
				return parent;

			System.Web.UI.Control control = null;
			foreach (System.Web.UI.Control c in parent.Controls)
			{
				control = c.FindControlRecursive(ID);
				if (control != null)
					break;
			}
			return control;
		}
	}
}
