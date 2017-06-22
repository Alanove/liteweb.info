using System;
using System.Data;
using System.Web.UI;
using lw.CTE;
using lw.Data;
using lw.WebTools;

namespace lw.Members
{
	public class PrivacySettingsManager
	{
		DataSet ds = null;
		
		public PrivacySettingsManager()
		{
			ds = XmlManager.GetDataSet(MembersSettings.PrivacyFile);
		}

		/// <summary>
		/// Checks if a member can access the property
		/// will return the appropriate option depending Friends, EveryOne or OnlyMe
		/// </summary>
		/// <param name="property">MemberProperty ex: Email, ProfilePicture...</param>
		/// <param name="privacyValue">The value of the member privacy</param>
		/// <returns><seealso cref="PrivacyOptions"/></returns>
		public PrivacyOptions CheckMemberProperty(string property, int privacyValue)
		{
			//TODO: Check privacy depending on network or friends group...

			DataView view = new DataView(Table, "Property='" + property + "'", "", DataViewRowState.CurrentRows);
			if (view.Count == 0)
				return PrivacyOptions.Everyone;

			int value = int.Parse(view[0]["Value"].ToString());

			return CheckMemberProperty(privacyValue, value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="privacyValue"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public PrivacyOptions CheckMemberProperty(int privacyValue, int value = 1)
		{
			int testOnlyMe = (int)Math.Pow(2, value * 2);
			int testFriends = (int)Math.Pow(2, value);

			if ((privacyValue & testOnlyMe) != 0)
				return PrivacyOptions.OnlyMe;

			if ((privacyValue & testFriends) != 0)
				return PrivacyOptions.Friends;

			return PrivacyOptions.Everyone;
		}


		public void SetPrivacyProperty(int MemberId, string Property, PrivacyOptions option)
		{
			DataView view = new DataView(Table, "Property='" + Property + "'", "", DataViewRowState.CurrentRows);
			if (view.Count == 0)
				return;

			int value = int.Parse(view[0]["Value"].ToString());

			//Only me multiply by 2
			int testOnlyMe = (int)Math.Pow(2, value * 2);
			int testFriends = (int)Math.Pow(2, value);

			int addedValue = 0;

			if(option == PrivacyOptions.Friends)
				addedValue = testFriends;
			else if(option == PrivacyOptions.OnlyMe)
				addedValue = testOnlyMe;

			//string sql = "Update Members set Privacy=(Privacy& ~{0})|(Privacy& ~{1})|{2} where MemberId={3}";

			string sql = @"
Update Members set Privacy = 
case 
	When (Privacy&{0}) <> 0 then
		(Privacy& ~{0})|{2} 
	when (Privacy&{1}) <> 0 then
		(Privacy& ~{1})|{2} 
	else
		Privacy | {2}
	end
where MemberId={3}";

			sql = String.Format(sql, testOnlyMe, testFriends, addedValue, MemberId);

			DBUtils.ExecuteQuery(sql, cte.lib);
		}
		public void SetPrivacyProperty(int MemberId, string Property, string option)
		{
			SetPrivacyProperty(MemberId, Property, (PrivacyOptions)Enum.Parse(typeof(PrivacyOptions), option));
		}


		public void UpdatePrivacy(int MemberId, string name, string value)
		{
			string[] fields = name.Split('_');

			if (fields.Length == 2)
			{
				SetPrivacyProperty(MemberId, fields[1], value);
			}
		}


		public bool CanAccess(DataRow memberRow, string Property, Control ctrl)
		{
			if ((int)memberRow["MemberId"] != WebContext.Profile.UserId)
			{
				int privacy = (int)memberRow["Privacy"];

				switch (CheckMemberProperty(Property, privacy))
				{
					case PrivacyOptions.OnlyMe:
						return false;
					case PrivacyOptions.Friends:
						lw.Base.CustomPage page = ctrl.Page as lw.Base.CustomPage;

						DataTable MyFriends = FriendsManager.GetMyFriends(page);

						if (MyFriends.Select(string.Format("FriendId={0} and Status=1", memberRow["MemberId"])).Length == 0)
						{
							return false;
						}
						break;
					default:
						break;
				}
			}
			return true;
		}


		#region Properties
		public DataSet DS
		{
			get
			{
				return ds;
			}
		}
		public DataTable Table
		{
			get
			{
				return ds.Tables[0];
			}
		}
		#endregion

	}
}
