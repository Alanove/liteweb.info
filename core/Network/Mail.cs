using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Net.Mail;
using lw.CTE;
using lw.Utils;
using lw.WebTools;


namespace lw.Network
{
	/// <summary>
	/// Mail object, is used to send emails in the website.
	/// Before using this object make sure to update the mail parameters in web.config
	/// <![CDATA[
	/// <smtp from="from@domain.com" deliveryMethod="Network">
	///		<network host="mailhost" port="25" password="password" userName="username" />
	///	</smtp>
	/// ]]>
	/// 
	/// Note that when using this method you don't have to worry about the performance of the mail server, 
	/// as the e-mails will be prepared on the main server and then sent one by one later on using a threaded timer <seealso cref="lw.Network.MailQueue"/>
	/// </summary>
	public class Mail : IDisposable
	{
		#region Local Variables
		string _email = "", _from = "", _to = "", _subject = "", _bcc = "", _cc = "";
		NameValueCollection _data;
		MailAddressCollection _toList, _ccList, _bccList;
		ArrayList _attachements = new ArrayList();

		string _body = "", _bodyFile = "";
		DataTable _emails;
		DataRow _row;

		string _configFrom = "";
		string _configTo = "";

		#endregion

		#region Init
		void _init()
		{
			Config cfg = new Config();
			this._configFrom = cfg.GetKey("EmailsFrom");
			this._configTo = cfg.GetKey("EmailsTo");

			_emails = XmlManager.GetTableFromXmlDataSet(DataSets.Emails, "Emails");
		}
		void _initByMail()
		{
			DataRow[] rows = _emails.Select(string.Format("Email='{0}'", this.Email));
			if (rows.Length > 0)
			{
				this._row = rows[0];
				if (_row["From"] != System.DBNull.Value)
					this.From = _row["From"].ToString();
				else
					this.From = this._configFrom;

				if (_row["To"] != System.DBNull.Value)
					this.To = _row["To"].ToString();
				else
					this.To = this._configTo;				

				if (_row["Subject"] != System.DBNull.Value)
					this.Subject = _row["Subject"].ToString();

				this.Body = "";
			}
			else
			{
				throw new Exception("Incorrect Email");
			}
		}
		#endregion

		#region Constructors

		/// <summary>
		/// Mail Constructor
		/// </summary>
		public Mail()
		{
			_init();
		}
		/// <summary>
		/// Creates a Mail Object
		/// </summary>
		/// <param name="email">The email file located in "Prv/Emails" and defined in "Prv/Conf/emails.config"</param>
		/// <param name="from">The from address that this email should send from</param>
		/// <param name="to">The to address that this email should send</param>
		/// <param name="subject">Subject of the email</param>
		public Mail(string email, string from, string to, string subject)
		{
			_init();
			this.Email = email;
			this.From = from;
			this.To = to;
			this.Subject = subject;
			string path = Path.Combine("~", ConfigCte.EmailsFolder);
			path = Path.Combine(path, email + ".htm");
			this._bodyFile = path;
		}

		/// <summary>
		/// Mail Constructor
		/// </summary>
		/// <param name="email">The email defined in Emails.Config with related file in PRV/Emails</param>
		public Mail(string email)
		{
			_init();
			this.Email = email;
			this.Subject = email;

			this._initByMail();
			string path = Path.Combine("~", ConfigCte.EmailsFolder);
			path = Path.Combine(path, email + ".htm");
			this._bodyFile = path;

		}

		/// <summary>
		/// Mail Constructor
		/// </summary>
		/// <param name="email">The email defined in Emails.Config with related file in PRV/Emails</param>
		/// <param name="data">The data to be used inside the mail</param>
		public Mail(string email, NameValueCollection data)
		{
			_init();
			this.Email = email;
			this.Subject = email;
			this._initByMail();

			this.Data = data;
			string path = WebContext.Server.MapPath(WebContext.StartDir + ConfigCte.EmailsFolder);
			path += "/" + email + ".htm";
		}
		#endregion	

		#region Send Mail
		/// <summary>
		/// Sends the email to all the <seealso cref="TooList"/>, <seealso cref="CCList"/> and <seealso cref="BCCList"/>
		/// </summary>
		public void Send()
		{
			Send(false);
		}

		public void Send(bool KeepAttachementsOnServer)
		{
			MailManager mMgr = new MailManager();
			if (String.IsNullOrWhiteSpace(Body))
			{
				Body = MailManager.LoadData(this.BodyFile, this.Data);
			}
			mMgr.SendMail(
				new MailAddress(this.From, this.FromName == null? this.From: this.FromName), 
				this.ToList, 
				this.CCList, 
				this.BCCList, 
				this.Subject, 
				IncludeSiteNameInSubject,
				Body, 
				StringUtils.StripOutHtmlTags(Body),
				this.Attachements);

			if (DeleteAttachementsAfterSend)
				DeleteAttachements();
		}

		/// <summary>
		/// Deletes all the attachements related to this email
		/// </summary>
		public void DeleteAttachements()
		{
			for (int i = 0; i < _attachements.Count; i++)
			{
				File.Delete(_attachements[i].ToString());
			}
		}

		/// <summary>
		/// Send Email As Attachement File
		/// </summary>
		public void SendAsAttachement()
		{
			SendAsAttachement(DateTime.Now.ToString("s"));
		}

		public void SendAsAttachement(string str)
		{
			SendAsAttachement(str, null);
		}


		/// <summary>
		/// Send Email As Attachement File
		/// </summary>
		/// <param name="str">Value added to the file name; Default: DateTime in seconds</param>
		public void SendAsAttachement(string str, string FormLink)
		{
			MailManager mMgr = new MailManager();

			string folder = WebContext.Server.MapPath("~/temp");
			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}

			string tempFile = WebContext.Server.MapPath("~/temp/" + StringUtils.ToURL(this.Email + "-" + str) + ".htm");
			StreamWriter sr = new StreamWriter(tempFile);
			sr.Write(MailManager.LoadData(this.BodyFile, this.Data));
			sr.Close();
			sr.Dispose();
			sr = null;

			_attachements.Add(tempFile);

			MailAddress _from = new MailAddress(this.From, this.FromName == null ? this.From : this.FromName);

			if (string.IsNullOrEmpty(FormLink))
				mMgr.SendMail(_from, this.ToList, this.CCList, this.BCCList, this.Subject, true, "Please download the attachment.", "", _attachements);
			else
			{
				string _link = string.Format("Please download the attachment.<br /><p><a href=\"{0}\">Click here</a> to manage this form</p>", FormLink);
				mMgr.SendMail(_from, this.ToList, this.CCList, this.BCCList, this.Subject, true, _link, "", _attachements);
			}
			for (int i = 0; i < _attachements.Count; i++)
			{
				try
				{
					File.Delete(_attachements[i].ToString());
				}
				catch (Exception Ex)
				{
					ErrorContext.Add("attachment-delete", "Could not delete attachment: " + _attachements[i].ToString() + "<BR>" + Ex.Message);
				}
			}
			this._attachements.Clear();
		}

		/// <summary>
		/// Sends the email in text format
		/// </summary>
		public void SendText()
		{
			MailManager mMgr = new MailManager();
			mMgr.SendMail(this.From, this.ToList, this.Subject, this.Body);
		}

		public void SendText(bool includeSiteNameInSubject)
		{
			MailManager mMgr = new MailManager();
			MailAddress _from = new MailAddress(this.From, this.FromName == null ? this.From : this.FromName);
			mMgr.SendMail(_from, this.ToList, this.CCList, this.BCCList, this.Subject, includeSiteNameInSubject, this.Body, "", new ArrayList());
		}

		#endregion

		#region  Methods
		public void Attach(System.Web.HttpPostedFile file)
		{
			if (file == null || file.ContentLength == 0)
				return;

			string path = WebContext.Server.MapPath("~/PRV/temp");
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			path = Path.Combine(path, lw.Utils.StringUtils.GetFileName(file.FileName));

			file.SaveAs(path);

			Attach(path);
		}

		/// <summary>
		/// Adds an attachement to the mail.
		/// Calling this method will automatically set the flag <seealso cref="DeleteAttachementsAfterSend"/> to false, 
		/// if you still want to delete the attachement after send you can simply call <seealso cref="DeleteAttachementsAfterSend"/>
		/// </summary>
		/// <param name="fileName">The file location</param>
		public void Attach(string fileName)
		{
			_attachements.Add(fileName);
			DeleteAttachementsAfterSend = false;
		}

		public DataView GetEmails()
		{
			return new DataView(this._emails, "", "Email", DataViewRowState.CurrentRows);
		}
		public bool Create()
		{
			if (_emails.Select(string.Format("Email='{0}'", this.Email)).Length > 0)
			{
				return false;
			}
			DataRow row = _emails.NewRow();
			row["Email"] = this.Email;
			row["From"] = this.From;
			row["To"] = this.To;
			row["Subject"] = this.Subject;

			_emails.Rows.Add(row);

			AcceptChanges();

			this._row = row;


			System.IO.StreamWriter str = new System.IO.StreamWriter(WebContext.Server.MapPath(this._bodyFile));
			str.Write("");
			str.Close();

			return true;
		}
		public void Update()
		{
			this._row["From"] = this.From;
			this._row["To"] = this.To;
			this._row["Subject"] = this.Subject;
			System.IO.StreamWriter str = new System.IO.StreamWriter(WebContext.Server.MapPath(this._bodyFile));
			str.Write(this._body);
			str.Close();

			AcceptChanges();
		}
		public void AcceptChanges()
		{
			_emails.AcceptChanges();
			XmlManager.SetDataSet(DataSets.Emails, _emails.DataSet);
		}
		#endregion

		#region Properties

		bool _includeSiteNameInSubject = true;
		/// <summary>
		/// Includes the site name in the subject field.
		/// </summary>
		public bool IncludeSiteNameInSubject
		{
			get
			{
				return _includeSiteNameInSubject;
			}
			set
			{
				_includeSiteNameInSubject = value;
			}
		}

		public string BodyFile
		{
			get
			{
				return _bodyFile;
			}
			set
			{
				_bodyFile = value;
			}
		}
		public string Email
		{
			get
			{
				return _email;
			}
			set
			{
				_email = value;
			}
		}

		string _FromName = null;
		/// <summary>
		/// Specifies the from address' name of this email
		/// </summary>
		public string FromName
		{
			get
			{
				return _FromName;
			}
			set
			{
				_FromName = value;
			}
		}

		/// <summary>
		/// Specifies the from address' name of this email
		/// </summary>
		public string From
		{
			get
			{
				return _from;
			}
			set
			{
				_from = value;
			}
		}

		public MailAddressCollection ToList
		{
			get 
			{
				if (_toList == null)
				{
					_toList = new MailAddressCollection();
				}
				return _toList;
			}
			set { _toList = value; }
		}

		public MailAddressCollection CCList
		{
			get
			{
				if (_ccList == null)
				{
					_ccList = new MailAddressCollection();
				}
				return _ccList;
			}
			set { _ccList = value; }
		}

		public MailAddressCollection BCCList
		{
			get
			{
				if (_bccList == null)
				{
					_bccList = new MailAddressCollection();
				}
				return _bccList;
			}
			set { _bccList = value; }
		}

		/// <summary>
		/// sets the recipient's email address
		/// resets the ToList variable
		/// </summary>
		public string To
		{
			get
			{
				return _to;
			}
			set
			{
				_to = value;

				ToList = null;
				ToList = new MailAddressCollection();

				ToList.Add(_to);
			}
		}


		/// <summary>
		/// sets the cc email address
		/// resets the CCList variable
		/// </summary>
		public string CC
		{
			get
			{
				return _cc;
			}
			set
			{
				_cc = value;

				CCList = null;
				CCList = new MailAddressCollection();

				CCList.Add(_cc);
			}
		}

		
		public string BCC
		{
			get
			{
				return _bcc;
			}
			set
			{
				_bcc = value;

				BCCList = null;
				BCCList = new MailAddressCollection();

				BCCList.Add(_bcc);
			}
		}

		public NameValueCollection Data
		{
			get
			{
				return _data;
			}
			set
			{
				_data = value;
			}
		}
		public ArrayList Attachements
		{
			get
			{
				return _attachements;
			}
			set
			{
				_attachements = value;
			}
		}

		bool _deleteAttachementsAfterSend = true;
		/// <summary>
		/// A flag to delete the attached files after sending the email (default to true).
		/// Note that when setting this to false, attachements must be manually deleted.
		/// To delete the attachements you can also call <seealso cref="DeleteAttachements"/>
		/// </summary>
		public bool DeleteAttachementsAfterSend
		{
			get
			{
				return _deleteAttachementsAfterSend;
			}
			set
			{
				_deleteAttachementsAfterSend = value;
			}
		}
		public string Body
		{
			get
			{
				return _body;
			}
			set
			{
				_body = value;
			}
		}
		public string Subject
		{
			get
			{
				return _subject;
			}
			set
			{
				_subject = value;
			}
		}
		#endregion

		#region Dispose
		public void Dispose()
		{
			if (this.Attachements.Count > 0 && DeleteAttachementsAfterSend)
			{
				for (int i = 0; i < _attachements.Count; i++)
				{
					File.Delete(_attachements[i].ToString());
				}
			}
		}
		#endregion
	}
}