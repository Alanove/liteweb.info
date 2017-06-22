using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using lw.Members;
using lw.Network;
using lw.Threading;
using lw.WebTools;

namespace lw.Downloads.Threads
{
	public class WeeklyDownloadsActivity : ThreadingBase
	{
		public override void Action()
		{
			WebContext.CreateHttpContext();

			MembersManager mMgr = new MembersManager();
			Downloads dMgr = new Downloads();

			DataView members = mMgr.GetMembers("").DefaultView;



			string protocol = WebContext.Protocol;
			string serverName = WebContext.ServerName;
			string root = WebContext.Root;

			foreach (DataRowView drv in members)
			{

				DataView downloads = dMgr.GetDownloadsByNetwork((int)drv["MemberId"], DateTime.Now.AddDays(-7)).DefaultView;

				if (downloads.Count > 0)
				{
					StringBuilder str = new StringBuilder();

					str.Append("<ul>");

					foreach (DataRowView dr in downloads)
					{
						str.Append(string.Format("<li><a href=\"{0}://{1}{2}/DownloadHandler.ashx?FileId={3}\" />{4}</a></li>",
							protocol, /*serverName*/  "66.162.120.92" /* this ip is used only for licensing website */, root, dr["DownloadId"], dr["Title"]));
					}

					str.Append("</ul>");

					try
					{
						Mail m = new Mail("Weekly Downloads");

						m.Data = new NameValueCollection();
						m.Data["Downloads"] = str.ToString();
						m.To = (string)drv["Email"];

						m.Send();
					}
					catch
					{
					}
				}
			}
		}

	}
}
