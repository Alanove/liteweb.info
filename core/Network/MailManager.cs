using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Net.Mail;
using System.Text.RegularExpressions;

using lw.WebTools;
using lw.CTE;
using lw.Utils;



namespace lw.Network
{
	/// <summary>
	/// Summary description for MailManager.
	/// </summary>
	public class MailManager
	{
		public SmtpClient smtp;

		string SmtpNetworkUser = "";

		/* 
mail now takes credentials from web.config automatic configuration

<system.net>
	<mailSettings>
		<smtp from="xx@domain.com" deliveryMethod="Network">
			<network host="localhost" port="25" password="***" userName="***" />
		</smtp>
	</mailSettings>
</system.net>
		*/
		public MailManager()
		{
			smtp = new SmtpClient();

			/* for old websites */
			string host = Config.GetFromWebConfig(lw.CTE.Email.SmtpServer);
			if (host != null && host != "")
				smtp.Host = host;

			string port = Config.GetFromWebConfig(lw.CTE.Email.SmtpServerPort);
			if (port != null && port != "")
				smtp.Port = Int32.Parse(port);

			SmtpNetworkUser = Config.GetFromWebConfig(lw.CTE.Email.SmtpNetworkUser);
			string SmtpNetworkPass = Config.GetFromWebConfig(lw.CTE.Email.SmtpNetworkPass);

			if (SmtpNetworkUser != null && SmtpNetworkUser != "")
				smtp.Credentials = new System.Net.NetworkCredential(SmtpNetworkUser, SmtpNetworkPass);

			string SSL = Config.GetFromWebConfig("EmailSSL");
			if (!String.IsNullOrWhiteSpace(SSL) && SSL.ToLower() == "true")
			{
				smtp.EnableSsl = true;
			}

			//smtp.ServicePoint.MaxIdleTime = 10;
			//smtp.ServicePoint.ConnectionLimit = 1;
		}

		public MailManager(string user, string password)
		{
			smtp = new SmtpClient();

			smtp.Credentials = new System.Net.NetworkCredential(user, password);
			smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

		}
		public void SendMail(string from, MailAddressCollection to, string subject, string file, NameValueCollection data)
		{
			SendMail(from, to, subject, file, data, null);
		}
		public void SendMail(string from, MailAddressCollection to, string subject, string file, HybridDictionary data)
		{
			SendMail(from, to, subject, file, CollectionUtils.HybridToNameValueCollection(data));
		}
		public void SendMail(string from, MailAddressCollection to, string subject, string file,
			NameValueCollection data, ArrayList attachements)
		{

			string body = LoadData(file, data);

			SendMail(from, to, subject, body, attachements);
		}
		public void SendMail(string from, MailAddressCollection to, string subject, string body)
		{
			SendMail(from, to, subject, body, new ArrayList());
		}
		public void SendMail(MailAddress from, MailAddressCollection to, string subject, string body)
		{
			SendMail(from, to, subject, body, "", new ArrayList());
		}

		public void SendMail(MailAddress from, MailAddressCollection to, 
			string subject, string bodyHtml, 
			string bodyText, ArrayList attachements
		)
		{
			SendMail(from, to, null, null,
				subject, true, bodyHtml,
				bodyText, attachements
			);
		}


		public void SendMail(
			MailAddress from, 
			MailAddressCollection to, 
			MailAddressCollection CC,
			MailAddressCollection BCC,
			string subject, 
			bool includeSiteNameInSubject,
			string bodyHtml, 
			string bodyText, ArrayList attachements
		)
		{
			Config cfg = new Config();

			MailMessage msg = new MailMessage();
			msg.From = from;
			
			foreach(MailAddress add in to)
			{
				msg.To.Add(add);
			}
			if (CC != null)
			{
				foreach (MailAddress add in CC)
				{
					msg.CC.Add(add);
				}
			}
			if (BCC != null)
			{
				foreach (MailAddress add in BCC)
				{
					msg.Bcc.Add(add);
				}
			}

			//SendAllMailsFrom used for server with custom permissions
			if (!String.IsNullOrEmpty(Config.GetFromWebConfig(lw.CTE.Email.SendAllMailsFrom)))
			{
				msg.From = new MailAddress(Config.GetFromWebConfig(lw.CTE.Email.SendAllMailsFrom), from.DisplayName);
				msg.ReplyToList.Add(from);
			}

			// ReplyToSession in case we want the reply to address to be different from the sent.
			if (WebContext.Session != null && WebContext.Session[lw.CTE.Email.ReplyToSession] != null)
			{
				if (!String.IsNullOrEmpty(WebContext.Session[lw.CTE.Email.ReplyToSession].ToString()))
					msg.ReplyToList.Add(new MailAddress(WebContext.Session[lw.CTE.Email.ReplyToSession].ToString()));
			}

			msg.Subject = subject;
			if (includeSiteNameInSubject)
			{
				
				var siteName = cfg.GetKey("SiteName");
				if (!String.IsNullOrEmpty(siteName))
				{
					if (subject.IndexOf(siteName) < 0)
						msg.Subject = cfg.GetKey("SiteName") + " - " + subject;
				}
					
			}

			if (attachements != null && attachements.Count > 0)
			{
				for (int i = 0; i < attachements.Count; i++)
				{
					msg.Attachments.Add(new Attachment(attachements[i].ToString()));
				}
			}

			msg.Body = bodyHtml;
			msg.IsBodyHtml = true;
			msg.BodyEncoding = new UTF8Encoding();
			
			try
			{
				bool noBcc = false;
				if (WebContext.Session != null && WebContext.Session["NoBCC"] != null)
					noBcc = true;
				if (!noBcc)
				{
					string bcc = cfg.GetKey("BCC");
					string[] mails = bcc.Split(new Char[] {',', ';', '-'});
					foreach (string mail in mails)
					{
						msg.Bcc.Add(mail);
					}
				}
			}
			catch
			{
			}
			try
			{
				//WebContext.Response.Write("message sent");
				//smtp.Send(msg);

				MailQueue.AddToQueue(msg);
			}
			catch (Exception Ex)
			{
				string trace = (from.Address + "<BR>" + to + "<BR>" + subject + "<BR>" + msg.Body + "<BR>");
				trace += Ex.Message;

				ErrorContext.Add("e-mail", "Fail to send email");
				ErrorContext.Add("trace:e-mail", trace);

				throw (Ex);
			}
			for (int i = 0; i < msg.Attachments.Count; i++)
			{
				msg.Attachments[i].Dispose();
			}
			msg.Attachments.Clear();
			msg.Dispose();
			msg = null;
		}

		public void SendMail(string addressFrom, string mailTo, string subject, string mailBody)
		{
			SendMail(addressFrom, new MailAddressCollection() { new MailAddress(mailTo) }, subject, mailBody);
		}
		public void SendMail(string addressFrom, string to, string subject, string body, ArrayList attachements)
		{
			SendMail(addressFrom, new MailAddressCollection() { new MailAddress(to) }, subject, body, attachements);
		}

		public void SendMail(string addressFrom, MailAddressCollection to, string subject, string body, ArrayList attachements)
		{
			//if (SmtpNetworkUser != "")
			//	from = SmtpNetworkUser;
			Config cfg = new Config();

			if (addressFrom == null || addressFrom == "")
			{
				addressFrom = cfg.GetKey(Settings.EmailsFrom);
			}
			if (to.Count == 0)
			{
				to.Add(cfg.GetKey(Settings.EmailsTo));
			}

			string nameFrom = "";
			//format = ("alain" <alain@alainhaddad.com>) or simply the email
			if (addressFrom.Contains("<"))
			{
				nameFrom = addressFrom.Substring(1, addressFrom.IndexOf("<") - 2);
				addressFrom = addressFrom.Replace("\"" + nameFrom + "\"", "").Replace("<", "").Replace(">", "");
			}
			else
				nameFrom = addressFrom;

			MailAddress _from = new MailAddress(addressFrom, nameFrom);

			SendMail(_from, to, subject, body, "", attachements);
		}
		public MailMessage SendMailAlter(string from, MailAddressCollection to, string subject, string body)
		{
			return SendMailAlter(from, to, subject, body, false);
		}
		public MailMessage SendMailAlter(string from, MailAddressCollection to, string subject, string body, bool ReturnMsg)
		{
			//if (SmtpNetworkUser != "")
			//	from = SmtpNetworkUser;

			MailMessage msg = new MailMessage();
			msg.From = new MailAddress(from);
			msg.Subject = subject;
			msg.Body = body;

			foreach (MailAddress add in to)
			{
				msg.To.Add(add);
			}

			//System.Web.HttpContext.Current.Response.Write(string.Format("    - {0} {1} {2} {3}",
			//	from, to, subject, body));

			NameValueCollection Images = new NameValueCollection();
			int autoId = 1;
			StringBuilder ret = new StringBuilder();

			int i = body.IndexOf("src=\"");
			int j = 0;
			int len = body.Length;
			try
			{
				while (i > 0)
				{
					ret.Append(body.Substring(0, i + 5));
					body = body.Substring(i + 5);
					//WebContext.Response.Write(i.ToString() + " - <pre>" + WebContext.Server.HtmlEncode(body) + "</pre>");
					i = body.IndexOf("\"");
					string image = body.Substring(0, i);

					//ret.Append(image);

					Images.Add(string.Format("Image{0}", autoId),
						WebContext.Server.MapPath(image));

					//ret.Append("\"");

					body = body.Substring(image.Length + 1);
					image = "cid:" + string.Format("Image{0}", autoId);

					ret.Append(image);
					ret.Append("\"");

					autoId++;

					i = body.IndexOf("src=\"");

					//WebContext.Response.Write(image + "<BR />");
				}
				ret.Append(body);

				//WebContext.Response.Write(ret.ToString());
			}
			catch
			{
			}

			body = ret.ToString();

			AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");

			foreach (string key in Images.Keys)
			{
				try
				{
					LinkedResource imagelink = new LinkedResource(Images[key], "image/jpg");
					imagelink.ContentId = key;
					//imagelink.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

					htmlView.LinkedResources.Add(imagelink);
				}
				catch (Exception Ex)
				{
					WebContext.Response.Write(Ex.Message);
				}
			}

			msg.AlternateViews.Add(htmlView);

			//msg.IsBodyHtml = true;
			msg.Body = body;
			try
			{
				smtp.Send(msg);
				lw.WebTools.WebContext.Response.Write("Message sent");

				//MailQueue.AddToQueue(msg);
			}
			catch (Exception Ex)
			{
				lw.WebTools.WebContext.Response.Write("Error");
				throw (Ex);
			}
			if (!ReturnMsg)
				msg = null;

			return msg;
		}


		public MailMessage SendMultiple(string from, string to, string cc, string bcc, string subject, string body, bool ReturnMsg)
		{
			//if (SmtpNetworkUser != "")
			//	from = SmtpNetworkUser;

			MailMessage msg = new MailMessage();

			//set sender's address
			msg.From = new MailAddress(from);
			msg.Subject = subject;
			msg.Body = body;

			// Allow multiple "To" addresses to be separated by a semi-colon
			if (to.Trim().Length > 0)
			{
				foreach (string add in to.Split(';'))
				{
					msg.To.Add(new MailAddress(add));
				}
			}

			// Allow multiple "Cc" addresses to be separated by a semi-colon
			if (cc.Trim().Length > 0)
			{
				foreach (string add in cc.Split(';'))
				{
					msg.CC.Add(new MailAddress(add));
				}
			}

			// Allow multiple "Bcc" addresses to be separated by a semi-colon
			if (bcc.Trim().Length > 0)
			{
				foreach (string add in bcc.Split(';'))
				{
					msg.Bcc.Add(new MailAddress(add));
				}
			}

			//System.Web.HttpContext.Current.Response.Write(string.Format("    - {0} {1} {2} {3}",
			//	from, to, subject, body));

			NameValueCollection Images = new NameValueCollection();
			int autoId = 1;
			StringBuilder ret = new StringBuilder();

			int i = body.IndexOf("src=\"");
			int j = 0;
			int len = body.Length;
			try
			{
				while (i > 0)
				{
					ret.Append(body.Substring(0, i + 5));
					body = body.Substring(i + 5);
					//WebContext.Response.Write(i.ToString() + " - <pre>" + WebContext.Server.HtmlEncode(body) + "</pre>");
					i = body.IndexOf("\"");
					string image = body.Substring(0, i);

					//ret.Append(image);

					Images.Add(string.Format("Image{0}", autoId),
						WebContext.Server.MapPath(image));

					//ret.Append("\"");

					body = body.Substring(image.Length + 1);
					image = "cid:" + string.Format("Image{0}", autoId);

					ret.Append(image);
					ret.Append("\"");

					autoId++;

					i = body.IndexOf("src=\"");

					//WebContext.Response.Write(image + "<BR />");
				}
				ret.Append(body);

				//WebContext.Response.Write(ret.ToString());
			}
			catch
			{
			}

			body = ret.ToString();

			AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");

			foreach (string key in Images.Keys)
			{
				try
				{
					LinkedResource imagelink = new LinkedResource(Images[key], "image/jpg");
					imagelink.ContentId = key;
					//imagelink.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

					htmlView.LinkedResources.Add(imagelink);
				}
				catch (Exception Ex)
				{
					WebContext.Response.Write(Ex.Message);
				}
			}

			msg.AlternateViews.Add(htmlView);

			//msg.IsBodyHtml = true;
			msg.Body = body;
			try
			{
				smtp.Send(msg);
				lw.WebTools.WebContext.Response.Write("Message sent");

				//MailQueue.AddToQueue(msg);
			}
			catch (Exception Ex)
			{
				lw.WebTools.WebContext.Response.Write("Error");
				throw (Ex);
			}
			if (!ReturnMsg)
				msg = null;

			return msg;
		}

		public static string LoadData(string file, NameValueCollection data)
		{
			Config cfg = new Config();
			if (data == null)
				data = new NameValueCollection();
			data["SiteName"] = cfg.GetKey("SiteName");
			data["Protocol"] = WebContext.Protocol;
			data["Server"] = WebContext.ServerName;
			data["Root"] = WebContext.Root;
			data["root"] = WebContext.Root;

			string body = "";

			if (file != "")
			{
				System.IO.StreamReader s = new System.IO.StreamReader(WebContext.Server.MapPath(file));
				body = s.ReadToEnd();
				if (data != null)
				{
					foreach (string str in data.Keys)
					{
						string p = "\\{{1}(?<id>\\w+)\\}{1}";
						System.Text.RegularExpressions.Regex r =
							new System.Text.RegularExpressions.Regex(p);

						DictionnaryEvaluator d = new DictionnaryEvaluator(data);

						body = r.Replace(body, new MatchEvaluator(d.LookUp));
					}
				}
				s.Close();
			}
			return body;
		}
	}

	
}