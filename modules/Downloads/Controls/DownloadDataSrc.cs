using System;
using System.Data;
using System.Text;
using System.Web.UI;

using lw.Base;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Members;
using lw.Networking;
using lw.Utils;
using lw.WebTools;

namespace lw.Downloads.Controls
{
	public class DownloadDataSrc : CustomDataSource
	{
		bool _bound = false;
		string _top = "100 PERCENT";
		DownloadStatus status = DownloadStatus.All;
		bool _networkBound = false;
		bool _nonMemberNetworkBound = false;
		string _networkName = null;
		bool _displaySinceLastLogin = false;
		string type = "";
		string _condition = "";
		string _orderBy = "Sort ASC, DownloadId Desc";

		public DownloadDataSrc()
		{
			this.DataLibrary = cte.lib;
			OrderBy = _orderBy;
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			if (CMSMode == false && !CMSMode.Value && !this.MyPage.Editable)
			{

				StringBuilder cond = new StringBuilder();

				if (!String.IsNullOrWhiteSpace(Type))
					cond.Append(string.Format(" And (Type='{0}' or UniqueName='{0}')", StringUtils.SQLEncode(Type)));

				if (status != DownloadStatus.All)
					cond.Append(string.Format(" And Status={0}", (int)Status));

				if (NetworkBound)
				{
					NetworkRelations networkRelations = new NetworkRelations();
					cond.Append(" and " + networkRelations.GetRelationQueryByMember(cte.NetworkRelationTable, cte.NetworkRelateToField, WebContext.Profile.UserId));
				}

				if (NonMemberNetworkBound)
				{
					NetworkRelations networkRelations = new NetworkRelations();
					cond.Append(" and " + networkRelations.GetRelationQueryByNetwork(cte.NetworkRelationTable, cte.NetworkRelateToField, Int32.Parse(networkName)));
				}

				if (!string.IsNullOrWhiteSpace(Condition))
				{
					cond.Append(string.Format(" And " + Condition));
				}

				if (DisplaySinceLastLogin)
				{
					System.Data.DataRow member = lw.Members.Security.User.LoggedInUser(this, false) as System.Data.DataRow;
					MemberLoginActivity mlMgr = new MemberLoginActivity();
					DataView dv = mlMgr.GetMemberActivity((int)member["MemberId"]);
					dv.Sort = "Id Desc";
					if (dv.Count >= 2)
						cond.Append(string.Format(" and DateAdded>='{0}'", dv[1]["LoggedInDate"]));
				}

				this.SelectCommand = string.Format(" select Top {0} d.*,  '{1}/' + d.UniqueName + '/' + d.FileName as DownloadLink, FileSize/1024 as KB from DownloadsView d where Status<>{2} {3}",
					Top,
					Downloads.DownloadsVR,
					(int)DownloadStatus.Disabled,
					cond.ToString()
					);

				if (!EnablePaging)
				{
					this.SelectCommand += " Order By " + OrderBy;
				}
			}
			else
			{
				System.Web.HttpRequest Request = WebContext.Request;

				string cond = "1=1 ";

				if (!string.IsNullOrWhiteSpace(Condition))
				{
					cond += string.Format(" And " + Condition);
				}
				if (!String.IsNullOrEmpty(Request["q"]))
				{
					cond += string.Format(" and (Title like '%{0}%' or FileName like '%{0}%')", StringUtils.SQLEncode(Request["q"]));
				}

				if (!String.IsNullOrEmpty(Request["NetworkId"]))
				{
					cond += string.Format(" and DownloadId in (select DownloadId from DownloadsNetwork where NetworkId = {0})", Int32.Parse(Request["NetworkId"]));
				}

				if (!String.IsNullOrWhiteSpace(Type))
					cond += (string.Format(" And (Type='{0}' or UniqueName='{0}')", StringUtils.SQLEncode(Type)));

				if (!String.IsNullOrEmpty(Request["TypeId"]))
				{
					string _type = Request["TypeId"];
					cond += string.Format(" and DownloadType={0}", Int32.Parse(_type));
				}
				if (!String.IsNullOrEmpty(Request["Status"]))
				{
					string _Status = Request["Status"];
					cond += string.Format(" and Status={0}", Int32.Parse(_Status));
				}

				this.SelectCommand = string.Format(" select top {3} d.*,  '{1}/' + d.UniqueName + '/' + d.FileName as DownloadLink, FileSize/1024 as KB from DownloadsView d where {2}",
					"100%",
					Downloads.DownloadsVR,
					cond,
					Top
					);


				if (!EnablePaging)
				{
					this.SelectCommand += " Order By " + OrderBy;
				}

			}
			base.DataBind();
		}

		public string Top
		{
			get { return _top; }
			set { _top = value; }
		}

		public string Condition
		{
			get { return _condition; }
			set { _condition = value; }
		}
		public DownloadStatus Status
		{
			get
			{
				return status;
			}
			set
			{
				status = value;
			}
		}
		public string Type
		{
			get
			{
				if (StringUtils.IsNullOrWhiteSpace(type))
				{
					CustomPage page = this.Page as CustomPage;
					if (page != null)
						type = page.GetQueryValue(cte.TypeQueryStringName);
					if (type == null)
					{
						try
						{
							object obj = DataBinder.Eval(this.NamingContainer, "DataItem.Type");
							if (obj != null)
								type = (string)obj;
						}
						catch (Exception Ex)
						{ }
					}
				}
				return type;
			}
			set { type = value; }
		}

		/// <summary>
		/// Makes the list network bound returns only the items that are related to the current member's network
		/// </summary>
		public bool NetworkBound
		{
			get { return _networkBound && WebContext.Profile.dbUserName != lw.CTE.Admin.SuperAdmin; }
			set { _networkBound = value; }
		}

		/// <summary>
		/// Makes the list network bound returns only the items that are related to the current nonMember network
		/// </summary>
		public bool NonMemberNetworkBound
		{
			get { return _nonMemberNetworkBound; }
			set { _nonMemberNetworkBound = value; }
		}

		/// <summary>
		/// Makes the list network bound returns only the items that are related to the current network
		/// </summary>
		NetworksManager nMgr = new NetworksManager();

		public string networkName
		{
			get
			{
				if (StringUtils.IsNullOrWhiteSpace(_networkName))
				{
					lw.Base.CustomPage page = this.Page as lw.Base.CustomPage;
					string uniqueName = page.GetQueryValue("Network");

					if (!StringUtils.IsNullOrWhiteSpace(uniqueName))
					{
						_networkName = StringUtils.SQLEncode(uniqueName);
					}
				}
				lw.Networking.Network net = nMgr.GetNetwork(_networkName);
				return net.NetworkId.ToString();
			}
			set { _networkName = value; }
		}

		public bool DisplaySinceLastLogin
		{
			get { return _displaySinceLastLogin; }
			set { _displaySinceLastLogin = value; }
		}
	}
}