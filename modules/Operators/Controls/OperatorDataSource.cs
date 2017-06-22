using System;
using System.Data;
using System.Web.UI;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Networking;
using lw.Utils;
using lw.WebTools;
using lw.CTE;

namespace lw.Operators.Controls
{
	public class OperatorsDataSource : CustomDataSource
	{
		bool _bound = false;
		bool _networkBound = false;
		string _networkName = null;

		OperatorsManager oMgr;
		
		public OperatorsDataSource()
		{
			this.DataLibrary = cte.LibraryName;
			OrderBy = "MemberId Desc";
			oMgr = new OperatorsManager();
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;			

			this.SelectCommand = string.Format("Select * from operatorsview where 1=1");
			
			if (NetworkBound)
			{
				NetworkRelations networkRelations = new NetworkRelations();
				SelectCommand += " and " + networkRelations.GetRelationQueryByNetwork(cte.NetworkTable, cte.NetworkRelateToField, Int32.Parse(networkName));
			}

			if (CMSMode != null && CMSMode.Value)
			{
				string q = WebContext.Request["q"];
				string _network = WebContext.Request["NetworkId"];
				string _group = WebContext.Request["GroupId"];

				if (!string.IsNullOrWhiteSpace(q))
					this.SelectCommand += string.Format(" and (Username like '%{0}%' or Email like '%{0}%' or FirstName like '%{0}%' or LastName like '%{0}%')", StringUtils.SQLEncode(q));
				
				if (!String.IsNullOrWhiteSpace(_network))
				{
					this.SelectCommand += string.Format(" and MemberId in (select MemberId from MemberNetworks where NetworkId={0})", Int32.Parse(_network));
				}

				if (!String.IsNullOrWhiteSpace(_group))
				{
					this.SelectCommand += string.Format(" and MemberId in (select MemberId from OperatorProfile where GroupId={0})", Int32.Parse(_group));
				}
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

		
	}
}