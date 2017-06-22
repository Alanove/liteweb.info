using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using lw.Network;
using lw.Threading;
using lw.WebTools;

namespace lw.Members.Threads
{
	public class WeeklyLoginActivity : ThreadingBase
	{

		public override void Action()
		{
			WebContext.CreateHttpContext();

			MemberLoginActivity mlMgr = new MemberLoginActivity();

			DataView members = mlMgr.GetMembersActivity(string.Format("LoggedInDate >='{0}'", DateTime.Now.AddDays(-7)));
			members.Sort = "MemberId Asc, LoggedInDate Desc";

			Mail m = new Mail("Login Activity");

			int MemberID = -1;
			bool _bound = false;
			string emailTo = "";

			StringBuilder str = new StringBuilder("<table><tr><th>Ip Address</th><th>Login Date</th></tr>");

			foreach (DataRowView drv in members)
			{
				if (!_bound)
				{
					MemberID = (int)drv["MemberId"];
					_bound = true;
				}

				if ((int)drv["MemberId"] != MemberID)
				{
					str.Append("</table>");
					try
					{
						m.Data = new NameValueCollection();
						m.Data["Result"] = str.ToString();
						m.To = emailTo;
						m.Send();
					}
					catch
					{
					}

					MemberID = (int)drv["MemberId"];
					str = new StringBuilder("<table><tr><th>Ip Address</th><th>Login Date</th></tr>");
					str.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", drv["IpAddress"], drv["LoggedInDate"]));
					emailTo = (string)drv["Email"];
				}
				else
				{
					str.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", drv["IpAddress"], drv["LoggedInDate"]));
					emailTo = (string)drv["Email"];
				}

			}

			str.Append("</table>");
			try
			{
				m.Data = new NameValueCollection();
				m.Data["Result"] = str.ToString();
				m.To = emailTo;
				m.Send();
			}
			catch
			{
			}
		}

	}
}
