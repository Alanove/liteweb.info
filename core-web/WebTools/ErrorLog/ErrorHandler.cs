using System;
using System.IO;
using System.Text;
using lw.CTE;

namespace lw.WebTools
{
	public class ErrorHandler
	{
		/// <summary>
		/// Logs an error, it will automatically add the the posted requests (Form, QueryString and Files)
		/// </summary>
		/// <param name="desc">Description about what were you trying to do.</param>
		/// <param name="error">The exception object</param>
		/// <param name="url">The url (optional)</param>
		public static Exception HandleError(string desc, Exception error, string url)
		{
			Log(desc, error, url);

			return error;
		}

		/// <summary>
		/// Logs and mails an error, it will automatically add the the posted requests (Form, QueryString and Files)
		/// </summary>
		/// <param name="error">The exception object</param>
		public static Exception HandleError(Exception error)
		{
			string url = string.Format("{0}://{1}/{2}",
				WebContext.Protocol,
				WebContext.ServerName,
				WebContext.Request.RawUrl);

			HandleError("", error, url);

			return error;
		}

		/// <summary>
		/// Logs and mails an error, it will automatically add the the posted requests (Form, QueryString and Files)
		/// </summary>
		/// <param name="desc">Description about what were you trying to do.</param>
		/// <param name="error">The exception object</param>
		public static Exception HandleError(string desc, Exception error)
		{
			string url = string.Format("{0}://{1}/{2}",
				WebContext.Protocol,
				WebContext.ServerName,
				WebContext.Request.RawUrl);

			HandleError(desc, error, url);

			return error;
		}

		/// <summary>
		/// Logs and mails an error, it will automatically add the the posted requests (Form, QueryString and Files)
		/// </summary>
		/// <param name="error">The exception object</param>
		/// <param name="url">The url (optional)</param>
		public static Exception HandleError(Exception error, string url)
		{
			HandleError("", error, url);

			return error;
		}

		/// <summary>
		/// Creates a log entry into the logs folder in the website
		/// </summary>
		/// <param name="message">The log message</param>
		public static void Log(string message)
		{
			string fileName = string.Format("{0}-{1}.txt", DateTime.Now.Year, DateTime.Now.Month);

			string errorsPath = string.Format("{0}/{1}", "~/", Folders.ErrorLogs);

			errorsPath = WebContext.Server.MapPath(errorsPath);

			if (!Directory.Exists(errorsPath))
			{
				Directory.CreateDirectory(errorsPath);
			}

			fileName = Path.Combine(errorsPath, fileName);

			StreamWriter sw = null;

			try
			{
				sw = new StreamWriter(fileName, true); ;
				sw.WriteLine(message);
			}
			finally
			{
				if (sw != null)
					sw.Close();
			}
		}

		/// <summary>
		/// Creates a log entry from an error.
		/// This function is automatically called from a Page when any error occurs on the website, it also adds the URL automatically.
		/// </summary>
		/// <param name="desc">Description of the error</param>
		/// <param name="error">The exception</param>
		/// <param name="url">The url generating the error</param>
		public static void Log(string desc, Exception error, string url)
		{
			Log(GetErrorDetails(desc, error, url, Environment.NewLine));
		}

		/// <summary>
		/// Builds the error and returns the error in an organized string that can be added to files.
		/// </summary>
		/// <param name="desc">Description of the error</param>
		/// <param name="error">The exception</param>
		/// <param name="url">The url generating the error</param>
		/// <param name="sep">The seperator can be <!--<br />--> or <see cref="System.Environment.NewLine"/></param>
		/// <returns></returns>
		public static string GetErrorDetails(string desc, Exception error, string url, string sep)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(sep);
			sb.Append("----");
			sb.Append(sep);
			sb.Append(desc);
			sb.Append(sep);
			sb.Append("----");
			sb.Append(sep);
			sb.Append(string.Format("Date: {0}", DateTime.Now));
			sb.Append(sep);
			sb.Append(string.Format("URL: {0}", url));
			sb.Append(sep);
			sb.Append(string.Format("Query String: {0}", WebContext.Request.QueryString.ToString()));
			sb.Append(sep);
			if (WebContext.Request.UrlReferrer != null)
			{
				sb.Append(string.Format("Referrer: {0}", WebContext.Request.UrlReferrer.ToString()));
			}
			else
			{
				sb.Append("Referrer: Direct Access");
			}
			sb.Append(sep);
			sb.Append(string.Format("Post: {0}", WebContext.Request.Form.ToString()));
			sb.Append(sep);
			sb.Append(string.Format("Files: {0}", WebContext.Request.Files.ToString()));
			sb.Append(sep);
			sb.Append(sep);
			sb.Append("--- Exception --- ");
			sb.Append(sep); 
			sb.Append(string.Format("Message: {0}", error.Message));
			sb.Append(sep);
			sb.Append(error.StackTrace);
			sb.Append(sep);
			sb.Append("-------------------------------------------------------");
			sb.Append(sep);
			sb.Append("");
			sb.Append(sep);
			sb.Append(sep);

			return sb.ToString();
		}
	}
}
