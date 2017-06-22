using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Timers;
using lw.Utils;
using lw.WebTools;


namespace lw.Network
{
	public class MailQueue
	{
		public static ReaderWriterLock _lock = new ReaderWriterLock();

		private static System.Timers.Timer SendMailTimer = null;

		const int Send_Mail_Interval = 18100;

		static void _debug(string str)
		{
			//lw.WebTools.ErrorHandler.Log(string.Format("{0} - {1}", DateTime.Now, str));
		}

		/// <summary>
		/// Adds MailMessage to mailing queue and calls proccess queue
		/// </summary>
		/// <param name="Msg"><see cref="MailMessage"/></param>
		public static void AddToQueue(MailMessage Msg)
		{
			try
			{
				//We don't need a writer lock here as it will slow down the user response
				//_lock.AcquireWriterLock(-1);

				SerializableMailMessage serializableMsg = new SerializableMailMessage(Msg);

				_debug("Calling Add to queue");

				using (var sr = new MemoryStream())
				{
					var bformatter = new BinaryFormatter();

					sr.Seek(0, SeekOrigin.Begin);
					bformatter.Serialize(sr, serializableMsg);

					string path = WebContext.Server.MapPath(cte.MailQueuePath);

					if (!Directory.Exists(path))
						Directory.CreateDirectory(path);

					string mailTo = Msg.To.First().Address;

					string fileName = string.Format("{0}-{1}.{2}", StringUtils.ToURL(Msg.Subject), StringUtils.ToURL(mailTo), cte.MailExtension);

					if (!File.Exists(Path.Combine(path, fileName)))
					{
						lw.Utils.IO.SaveMemoryStream(sr, Path.Combine(path, fileName));
					}
					InitTimer();
				}
			}
			finally
			{
				//_lock.ReleaseWriterLock();
			}
		}

		/// <summary>
		/// Initializes the timer that will send emails
		/// </summary>
		public static void InitTimer()
		{
			//if (SendMailTimer != null)
			//{
			//	_debug("Timer already created, disposing of old timer");

			//	SendMailTimer.Stop();
			//	SendMailTimer.Dispose();
			//	SendMailTimer = null;
			//}

			_debug("Init timer");

			if (SendMailTimer == null)
			{
				try
				{
					SendMailTimer = new System.Timers.Timer(Send_Mail_Interval);
					SendMailTimer.Elapsed += new ElapsedEventHandler(ProccessQueue);
					SendMailTimer.Enabled = true;


				}
				catch (Exception ex)
				{
					lw.WebTools.ErrorHandler.HandleError("Fail to init timer", ex);
				}
			}
			lw.WebTools.ErrorHandler.Log(string.Format("Timer Call: {0} - {1}", DateTime.Now, SendMailTimer.GetType().Name));
		}

		/// <summary>
		/// Proccesses the mailing queue
		/// Stops if no mails are found
		/// Adter stop the re-initialazation happens in <seealso cref="AddToQueue"/>
		/// this function is called from Cache
		/// </summary>
		public static void ProccessQueue(object obj, ElapsedEventArgs e)
		{
			_debug("Calling Add to ProccessQueue");

			ProccessQueue();
		}


		/// <summary>
		/// Proccesses the mailing queue
		/// Stops if no mails are found
		/// Adter stop the re-initialazation happens in <seealso cref="AddToQueue"/>
		/// </summary>
		public static void ProccessQueue()
		{
			try
			{
				_debug("waiting for writer locl");
				_lock.AcquireWriterLock(-1);

				string path = System.Web.Hosting.HostingEnvironment.MapPath(cte.MailQueuePath);

				DirectoryInfo mailQueueDir = new DirectoryInfo(path);

				if (!mailQueueDir.Exists)
					return;

				FileInfo[] files = mailQueueDir.GetFiles("*." + cte.MailExtension);

				_debug("write permission granted");


				//stops the thread
				if (files.Count() > 0)
				{

					SendMailTimer.Stop();

					var sortedFiles = files.OrderBy(f => f.CreationTime);

					FileInfo mailFile = sortedFiles.First();

					using (var ms = IO.ReadMemoryStream(mailFile))
					{
						var bformatter = new BinaryFormatter();
						ms.Seek(0, SeekOrigin.Begin);
						var originalSerializableMessage = (SerializableMailMessage)bformatter.Deserialize(ms);
						var originalMessage = originalSerializableMessage.GetOriginalMailObject();

						try
						{
							_debug("sending mail");

							Smtp.Send(originalMessage);

							SendCompletedCallback(mailFile.FullName, false);
						}
						catch (Exception Ex)
						{
							lw.WebTools.ErrorHandler.HandleError("Fail to send email async", Ex);

							SendCompletedCallback(mailFile.FullName, true);
						}
						//removing the message from memory after send
						originalMessage.Dispose();
					}
				}
				else
				{
					if (SendMailTimer != null)
					{
						SendMailTimer.Dispose();
						SendMailTimer = null;
					}
				}
			}
			catch(Exception Ex)
			{
				lw.WebTools.ErrorHandler.HandleError("Fail to send email async", Ex);
			}
			finally
			{
				_lock.ReleaseWriterLock();
				if(SendMailTimer != null)
					SendMailTimer.Start();
			}
		}


		private static void SendCompletedCallback(string token, bool hasError)
		{
			// Get the unique identifier for this asynchronous operation.
			FileInfo mailFile = new FileInfo(token);

			_debug("call back  " + hasError.ToString());

			if (hasError)
			{
				//should log errors

				string path = WebContext.Server.MapPath(cte.MailQueuePath);

				path = Path.Combine(path, "error");

				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);

				try
				{
					mailFile.MoveTo(Path.Combine(path, mailFile.Name));
					_debug("Has Error, " + path + " " + mailFile.Name);
				}
				catch(Exception ex)
				{
					lw.WebTools.ErrorHandler.HandleError("Fail to move mail file.", ex);

					try
					{
						mailFile.Delete();
					}
					catch (Exception e)
					{
						lw.WebTools.ErrorHandler.HandleError("Fail to delete mail file.", e);


						if (SendMailTimer != null)
						{
							SendMailTimer.Stop();
							SendMailTimer = null;
						}

					}

				}
				return;
			}
			else
			{
				try
				{
					_debug("deleting file, " + mailFile.Name);
					mailFile.Delete();
					InitTimer();
				}
				catch (Exception ex)
				{
					lw.WebTools.ErrorHandler.HandleError("Fail to delete mail file.", ex);


					if (SendMailTimer != null)
					{
						SendMailTimer.Stop();
						SendMailTimer = null;
					}

				}
			}
		}

		static SmtpClient _smtp = null;
		public static SmtpClient Smtp
		{
			get
			{
				if (_smtp == null)
				{
					_smtp = new SmtpClient();
					string SmtpNetworkUser = "";

					/* for old websites */
					string host = Config.GetFromWebConfig(lw.CTE.Email.SmtpServer);
					if (host != null && host != "")
						_smtp.Host = host;

					string port = Config.GetFromWebConfig(lw.CTE.Email.SmtpServerPort);
					if (port != null && port != "")
						_smtp.Port = Int32.Parse(port);

					SmtpNetworkUser = Config.GetFromWebConfig(lw.CTE.Email.SmtpNetworkUser);
					string SmtpNetworkPass = Config.GetFromWebConfig(lw.CTE.Email.SmtpNetworkPass);

					if (SmtpNetworkUser != null && SmtpNetworkUser != "")
						_smtp.Credentials = new System.Net.NetworkCredential(SmtpNetworkUser, SmtpNetworkPass);

					string SSL = Config.GetFromWebConfig(lw.CTE.Email.SmtpSSL);
					if (!String.IsNullOrWhiteSpace(SSL) && SSL.ToLower() == "true")
					{
						_smtp.EnableSsl = true;
					}
				}
				return _smtp;
			}
		}
	}
}