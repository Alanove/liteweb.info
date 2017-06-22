
using lw.DataControls;
using lw.WebTools;
using lw.CTE;
using lw.Members;
using lw.Utils;

namespace lw.Members.Controls
{
	public class MemberDataProvider : DataProvider
	{
		bool _bound = false;

		public override void DataBind()
		{
			if (_bound)
				return;

			_bound = false;

			MembersManager mMgr = new MembersManager();

			if (MemberId != null)
			{
				this.DataItem = mMgr.GetMembersWithProfile(string.Format("MemberId={0}", memberId)).Rows[0];
			}
			else
			{
				if (UserName != null)
				{
					this.DataItem = mMgr.GetMembersWithProfile(string.Format("UserName='{0}'", UserName)).Rows[0];
				}
				else
					if (Security.User.LoggedIn)
					{
						this.DataItem = mMgr.GetMemberProfile(WebContext.Profile.UserId);
					}
			}


			base.DataBind();
		}


		#region Properties
		int? memberId;
		string userName;
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
		public string UserName
		{
			get
			{
				if (string.IsNullOrWhiteSpace(userName))
				{
					string obj = MyPage.GetQueryValue("UserName");

					if (!string.IsNullOrWhiteSpace(obj))
						userName = StringUtils.SQLEncode(obj);
				}
				return userName;
			}
			set
			{
				userName = value;
			}
		}

		#endregion
	}
}
