using System;
using System.Data;
using System.Web.UI;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Networking;
using lw.Utils;
using lw.WebTools;
using lw.CTE;
using lw.Data;

namespace lw.Members.Controls
{
	public class MembersDataSource : CustomDataSource
	{
		bool _bound = false;
		bool _networkBound = false;
		bool _client = false;
		string _networkName = null;
		string _groupName = null;

		MembersManager mMgr;

		public MembersDataSource()
		{
			this.DataLibrary = cte.lib;
			OrderBy = "MemberId Desc";
			mMgr = new MembersManager();
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;			

			this.SelectCommand = string.Format("Select * from MemberView where 1=1");
			
			if (NetworkBound)
			{
				NetworkRelations networkRelations = new NetworkRelations();
				SelectCommand += " and " + networkRelations.GetRelationQueryByNetwork(cte.NetworkTable, cte.NetworkRelateToField, Int32.Parse(networkName));
			}
			if (Client)
			{
				SelectCommand += " and MemberId not in (select MemberId from OperatorProfile)";
			}
			else if (!StringUtils.IsNullOrWhiteSpace(groupName))
			{
				string sql = string.Format("Select GroupId from OperatorGroups Where GroupName = '{0}'", groupName.ToString());
				DataSet ds = DBUtils.GetDataSet(sql, cte.lib);
				SelectCommand += string.Format(" and MemberId in (select MemberId from OperatorProfile where GroupId = {0})", Int32.Parse(ds.Tables[0].Rows[0]["GroupId"].ToString()));
			}
			if (CMSMode != null && CMSMode.Value)
			{
				string q = WebContext.Request["q"];
                string networkId = WebContext.Request["NetworkId"];
                string groupId = WebContext.Request["GroupId"];
                
                NetworkRelations networkRelations = new NetworkRelations();
                
                if (!string.IsNullOrEmpty(networkId))
                    SelectCommand += " and " + networkRelations.GetRelationQueryByNetwork(cte.NetworkTable, cte.NetworkRelateToField, Int32.Parse(networkId));
                
                if (!String.IsNullOrEmpty(groupId))
                    SelectCommand += String.Format(" and MemberId in (select MemberId from GroupsMembers where GroupId={0})", groupId);

				if (!string.IsNullOrWhiteSpace(q))
					this.SelectCommand += string.Format(" and (Username like '%{0}%' or FullName like '%{0}%' or Email like '%{0}%')", StringUtils.SQLEncode(q));
			}

			if (!EnablePaging)
			{
				this.SelectCommand += " Order By " + this.OrderBy;
			}

			base.DataBind();
		}

				
		NetworksManager nMgr = new NetworksManager();
		
		public string networkName
		{
			get
			{
				if (StringUtils.IsNullOrWhiteSpace(_networkName))
				{
					string uniqueName = MyPage.GetQueryValue(RoutingParameters.Network);

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
		/// <summary>
		/// Makes the list network bound returns only the items that are related to the current member's network
		/// </summary>
		public bool NetworkBound
		{
			get { return _networkBound; }
			set { _networkBound = value; }
		}

		public bool Client
		{
			get { return _client; }
			set { _client = value; }
		}

		public string groupName
		{
			get
			{
				return _groupName;
			}

			set { _groupName = value; }
		}
		
	}
}