using System;
using lw.Network;
using lw.WebTools;

namespace lw.Error
{
	public class Handler : lw.WebTools.ErrorHandler
	{
		/// <summary>
		/// Logs and mails an error, it will automatically add the the posted requests (Form, QueryString and Files)
		/// The last server error is cathed and sent automatically
		/// </summary>
		public static Exception HandleError()
		{
			Exception ex = WebContext.Server.GetLastError();

			if (ex == null)
				return ex;


			Config cfg = new Config();

			string errorTitle = "Error in " + cfg.GetKey(lw.CTE.Settings.SiteName);

			WebContext.Server.ClearError();
			

			return HandleError(errorTitle, ex);
		}

		/// <summary>
		/// Logs and mails an error, it will automatically add the the posted requests (Form, QueryString and Files)
		/// </summary>
		/// <param name="desc">Description about what were you trying to do.</param>
		/// <param name="error">The exception object</param>
		/// <param name="url">The url (optional)</param>
		public static Exception HandleError(string desc, Exception error, string url)
		{
			Log(desc, error, url);
			Mail(desc, error, url);

			return error;
		}

		/// <summary>
		/// Logs and mails an error, it will automatically add the the posted requests (Form, QueryString and Files)
		/// </summary>
		/// <param name="error">The exception object</param>
		new public static Exception HandleError(Exception error)
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
		new public static Exception HandleError(string desc, Exception error)
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
		new public static Exception HandleError(Exception error, string url)
		{
			HandleError("", error, url);

			return error;
		}


		/// <summary>
		/// Sends en email containing information about the error
		/// the email is send to the config parameter ErrorEmail
		/// </summary>
		/// <param name="subject">Subject of the email</param>
		/// <param name="error">Generated Exception</param>
		/// <param name="url">The url</param>
		public static void Mail(string subject, Exception error, string url)
		{
			try
			{
				MailManager mMgr = new MailManager();

				Config cfg = new Config();

				string siteName = cfg.GetKey("SiteName");

				string mailTo = cfg.GetKey(CTE.parameters.Errors_Email);

				string mailBody = "<h1>" + subject + "</h1><p>" + lw.WebTools.ErrorHandler.GetErrorDetails(subject, error, url, "<br />") + "</p>";

				mMgr.SendMail(null, mailTo, subject, mailBody);
			}
			catch
			{
			}
		}
	}
}
