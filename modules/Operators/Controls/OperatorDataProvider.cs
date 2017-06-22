using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lw.CTE;
using lw.DataControls;
using lw.Networking;

namespace lw.Operators.Controls
{
	public class OperatorDataProvider : DataProvider
	{

		public override void DataBind()
		{
			OperatorsManager oMgr = new OperatorsManager();

			if (MemberId != null)
			{
				this.DataItem = oMgr.GetOperatorsV(string.Format("MemberId={0}", memberId)).Table.Rows[0];
			}
			
			base.DataBind();
		}


#region Properties
		int? memberId;
		public int? MemberId
		{
			get
			{
				if (memberId == null)
				{
					string obj = MyPage.GetQueryValue(RoutingParameters.MemberId);
					if (string.IsNullOrWhiteSpace(obj))
					{
						obj = MyPage.GetQueryValue("Id");
					}
					if (!string.IsNullOrWhiteSpace(obj))
						memberId = int.Parse(obj);
				}
				return memberId;
			}
			set
			{
				memberId = value;
			}
		}
		
#endregion
	}
}
