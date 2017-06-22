using System;
using System.Data;

using lw.Data;

namespace lw.Members
{
	public class MemberLoginActivity : LINQManager
	{
		public MemberLoginActivity()
			: base(cte.lib)
		{

		}

		/// <summary>
		/// Saves the record about each member login
		/// </summary>
		/// <param name="memberId">DB Member Id</param>
		/// <param name="IpAddress"></param>
		/// <returns></returns>
		public bool AddMemberLogin(int memberId, string IpAddress)
		{

			Members_LoginActivity m = new Members_LoginActivity
			{
				MemberId = memberId,
				IpAddress = IpAddress,
				LoggedInDate = DateTime.Now
			};

			DataContext.Members_LoginActivities.InsertOnSubmit(m);
			Save();
			return true;
		}

		public DataView GetMembersActivity(string cond)
		{
			string sql;

			if (!String.IsNullOrEmpty(cond))
			{
				cond = " where " + cond;
			}

			sql = string.Format("select * from Members_LoginActivity_View {0}", cond);			

			return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0].DefaultView;
		}



		public DataView GetMemberActivity(int MemberId)
		{
			string sql;

			sql = string.Format("select * from Members_LoginActivity_View Where MemberId={0}",
						MemberId);

			return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0].DefaultView;
		}


		#region Variables


		public MembersDataContextDataContext DataContext
		{
			get
			{
				if (_dataContext == null)
					_dataContext = new MembersDataContextDataContext(Connection);
				return (MembersDataContextDataContext)_dataContext;
			}
		}

		#endregion
	}
}
