using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net.Mail;
using System.Collections.Specialized;
using System.Timers;

using lw.Members;
using lw.Downloads;
using lw.Networking;
using lw.WebTools;
using lw.Network;
using lw.Data;
using lw.Threading;

namespace lw.Downloads.Threads
{
	public class SendDownloadsWeeklyEmail: ThreadingBase
	{

		public SendDownloadsWeeklyEmail()
			: base(RepeatPattern.Weekly, new DateTime(2012, 5, 17, 12, 09, 0))
		{
		}

		public override void Action()
		{
			MembersManager mMgr = new MembersManager();
			Downloads dMgr = new Downloads();

			DataView members = mMgr.GetMembers("").DefaultView;

			Mail m = new Mail("Weekly Downloads");

			string protocol = WebContext.Protocol;
			string serverName = WebContext.ServerName;
			string root = WebContext.Root;

			foreach (DataRowView drv in members)
			{
				DataView downloads = dMgr.GetDownloadsByNetwork((int)drv["MemberId"], DateTime.Now.AddDays(-1)).DefaultView;

				if (downloads.Count > 0)
				{
					StringBuilder str = new StringBuilder();

					str.Append("<ul>");

					foreach (DataRowView dr in downloads)
					{
						str.Append(string.Format("<li><a href=\"{0}://{1}{2}/DownloadHandler.ashx?FileId={3}\" />{4}</a></li>",
							protocol, serverName, root, dr["DownloadId"], dr["Title"]));
					}

					str.Append("</ul>");

					m.Data = new NameValueCollection();
					m.Data["Downloads"] = str.ToString();
					m.To = (string)drv["Email"];

					m.Send();
				}
			}
		}

	}
}
