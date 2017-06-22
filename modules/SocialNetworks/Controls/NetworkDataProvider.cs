using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lw.CTE;
using lw.DataControls;
using lw.Networking;

namespace lw.Networking.Controls
{
	public class NetworkDataProvider : DataProvider
	{
		bool _bound = false;
		
		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			NetworksManager nMgr = new NetworksManager();

			if (NetworkId != null)
			{
				this.DataItem = nMgr.GetNetwork(NetworkId.Value);
			}
			
			base.DataBind();
		}


#region Properties
		int? networkId;
		public int? NetworkId
		{
			get
			{
				if (networkId == null)
				{
					string obj = MyPage.GetQueryValue(RoutingParameters.NetworkId);
					if (string.IsNullOrWhiteSpace(obj))
					{
						obj = MyPage.GetQueryValue("Id");
					}
					if (!string.IsNullOrWhiteSpace(obj))
						networkId = int.Parse(obj);
				}
				return networkId;
			}
			set
			{
				networkId = value;
			}
		}
#endregion
	}
}
