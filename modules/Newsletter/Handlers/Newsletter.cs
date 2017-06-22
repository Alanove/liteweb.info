using System;
using System.Web;

using lw.Utils;
using lw.WebTools;

namespace lw.Newsletter.Handlers
{
	/// <summary>
	/// Action to add, remove, update newsletter entries
	/// parameters as query string
	/// create or add, email to add an email address
	/// update, email, oldEmail to update the email
	/// remove, delete, unsubscribe, email to remove the email
	/// by default should redirect to the latest newsletter set in th Parameters
	/// </summary>
	public class Newsletter : IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			string email = context.Request.QueryString["email"];
			string action = context.Request.QueryString["action"];

			if (email != null && email != "")
				email = email.Trim();


			if (action == null)
				action = "";

			NewsletterManager nMgr = new NewsletterManager();

			switch (action)
			{
				case "create":
				case "add":
					if (Validation.IsEmail(email))
					{
						try
						{
							nMgr.AddUser(email, "", 10);
						}
						catch (Exception Ex)
						{
							context.Response.Write(Ex.Message);
							context.Response.End();
						}
						context.Response.Write("success");
					}
					else
						context.Response.Write("Incorrect Email Address");
					break;
				case "update":
					string oldEmail = context.Request.QueryString["oldEmail"];
					try
					{
						nMgr.DeleteUser(oldEmail);
					}
					catch (Exception Ex)
					{
						context.Response.Write(Ex.Message);
						context.Response.End();
					}
					if (Validation.IsEmail(email))
					{
						try
						{
							nMgr.AddUser(email, "", 10);
							context.Response.Write("success");
						}
						catch (Exception Ex)
						{
							context.Response.Write(Ex.Message);
							context.Response.End();
						}
					}
					else
						context.Response.Write("Incorrect Email Address");
					break;
				case "remove":
				case "delete":
				case "unsubscribe":
					try
					{
						nMgr.DeleteUser(email);
						context.Response.Write("success");
					}
					catch
					{
						context.Response.Write("Error, could not delete your email");
					}
					break;
				default:
					Config cfg = new Config();
					string latestNewsletter = cfg.GetKey(CTE.Settings.LatestNewsletter);
					if (!String.IsNullOrWhiteSpace(latestNewsletter))
						WebContext.Response.Redirect(latestNewsletter);
					else
						WebContext.Response.Redirect("~/");
					break;
			}

		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

	}
}
