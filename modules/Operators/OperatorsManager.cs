using System;
using System.Data;

using lw.Data;
using lw.Members;
using lw.Utils;
using lw.CTE.Enum;
using lw.WebTools;
using lw.Content;
using lw.CTE;


namespace lw.Operators
{
	/// <summary>
	/// Summary description for OperatorsManager.
	/// </summary>
	public class OperatorsManager:DirectorBase
	{
		public OperatorsManager()
			: base(cte.LibraryName)
		{
			
		}
		public DataView GetOperatorsV(string condition)
		{
			string sql = "select * from OperatorsView";
			if (!String.IsNullOrEmpty(condition))
				sql = sql + " where " + condition;
			return new DataView(DBUtils.GetDataSet(sql, cte.LibraryName).Tables[0]);
		}

		public DataView GetOperators(string condition)
		{
			return GetOperatorsV(condition);
		}


		public int AddOperator(
			string Email, 
			string AlternateEmail, 
			string FirstName, 
			string LastName, 
			int? GroupId, 
			string UserName, string Password,
			string SecretQuestion, string SecretAnswer, int? status, 
			string style)
		{
			MembersManager mMgr = new MembersManager();

			MembersDs.MembersRow member = mMgr.AddMember(UserName, 
				Email, AlternateEmail,
				Password, SecretAnswer, 
				SecretAnswer, "", FirstName, "",
				LastName,
 				Gender.Unknown,
				null,
				status, 0, "", "", 
				lw.WebTools.WebContext.Profile.UserId, 
				true);

			if (member == null)
				return -1;
			
			CreateOperatorProfile(member.MemberId, GroupId, style);
			
			return member.MemberId;
		}
		public void CreateOperatorProfile(int MemberId, int? GroupId, string Style)
		{
			string sql = "Delete from OperatorProfile where MemberId={0};insert into OperatorProfile(MemberId, GroupId, Style) values ({0}, {1}, '{2}')";
			sql = string.Format(sql, MemberId, GroupId, StringUtils.SQLEncode(Style));

			DBUtils.ExecuteQuery(sql, cte.LibraryName);
		}

        public void DeleteOperatorProfile(int MemberId)
        {
            string sql = "Delete from OperatorProfile where MemberId={0}";
            sql = string.Format(sql, MemberId);

            DBUtils.ExecuteQuery(sql, cte.LibraryName);
        }

		public int UpdateOperator(
			int MemberId,
			string Email, string AlternateEmail,
			string FirstName, string LastName,
			int GroupId,
			string UserName, string SecretQuestion, string SecretAnswer, int status,
			string style, int ChangedBy, bool JoinNewsletter,
			string PrivateComments, string Comments)
		{
			DataView dv = GetOperators("MemberId=" + MemberId);
			MembersManager mMgr = new MembersManager();

			string pass = mMgr.DecryptPassword((string)dv[0]["Password"]);

			return UpdateOperator(MemberId, Email, AlternateEmail, FirstName, LastName, GroupId, UserName, pass,
				SecretQuestion,  SecretAnswer,  status, style, ChangedBy, JoinNewsletter, PrivateComments, Comments);
		}
		public int UpdateOperator(
			int MemberId,
			string Email, string AlternateEmail,
			string FirstName, string LastName,
			int GroupId,
			string UserName, string Password,
			string SecretQuestion, string SecretAnswer, int status,
			string style, int ChangedBy, bool JoinNewsletter, 
			string PrivateComments, string Comments)
		{
			MembersManager mMgr = new MembersManager();
			int ret = mMgr.UpdateMember(MemberId, 
				UserName, Password, 
				Email, AlternateEmail,
				SecretQuestion, SecretAnswer, "", FirstName, "", LastName, null, null,
				status, lw.WebTools.WebContext.Profile.UserId, 
				JoinNewsletter, PrivateComments, Comments
				);
			CreateOperatorProfile(MemberId, GroupId, style);

			if(ret == -1)
				ErrorContext.Add("UsernameEmailExists", ContentManager.ErrorMsg(Errors.UsernameEmailExists));
			return ret;
		}
		public DataView GetOperatorGroups(string condition)
		{
			OperatorsDataSet OpData = new OperatorsDataSet();
			IDataAdapter Adp = base.GetAdapter(cte.AdbT_OperatorGroups);
			return ((OperatorsDataSet)base.FillData(Adp,OpData,condition)).OperatorGroups.DefaultView;
		}

		public bool AddOperatorGroup(string GroupName)
		{
			System.Data.DataView dv = GetOperatorGroups("GroupName='" + StringUtils.SQLEncode(GroupName) + "'");
			if(dv.Count>0)
				return false;
			OperatorsDataSet OpData = new OperatorsDataSet();
			IDataAdapter Adp = base.GetAdapter(cte.AdbT_OperatorGroups);
			OperatorsDataSet.OperatorGroupsRow row = OpData.OperatorGroups.NewOperatorGroupsRow();
			row.GroupName = GroupName;
			row.XmlFile = GroupName;
			OpData.OperatorGroups.AddOperatorGroupsRow(row);
			base.UpdateData(Adp,OpData);
			return true;
		}
		public void UpdateOperatorGroup(int GroupId,string GroupName,short Status)
		{
			OperatorsDataSet OpData = new OperatorsDataSet();
			IDataAdapter Adp = base.GetAdapter(cte.AdbT_OperatorGroups);
			OperatorsDataSet.OperatorGroupsRow row = OpData.OperatorGroups.NewOperatorGroupsRow();
			row.GroupId = GroupId;
			OpData.OperatorGroups.AddOperatorGroupsRow(row);
			row.AcceptChanges();
			row.GroupName=GroupName;
			row.XmlFile = GroupName;

			base.UpdateData(Adp,OpData);
		}
		public void UpdateOperatorGroup(string GroupId,string GroupName,short Status)
		{
			this.UpdateOperatorGroup(Int32.Parse(GroupId),GroupName,Status);
		}
		public void DeleteOperator(int OperatorId)
		{
			MembersManager mMgr = new MembersManager();
			mMgr.DeleteMember(OperatorId);
		}
		public void DeleteOperatorGroup(int GroupId,string path)
		{
			System.Data.DataView dv = this.GetOperatorGroups("GroupId="+GroupId.ToString());
			path = lw.WebTools.WebContext.Server.MapPath(CTE.Folders.ManagerSchemas + "/" + dv[0]["GroupName"].ToString());

			OperatorsDataSet OpData = new OperatorsDataSet();
			IDataAdapter Adp = base.GetAdapter(cte.AdbT_OperatorGroups);
			OperatorsDataSet.OperatorGroupsRow row = OpData.OperatorGroups.NewOperatorGroupsRow();
			row.GroupId = GroupId;
		
			OpData.OperatorGroups.AddOperatorGroupsRow(row);
			row.AcceptChanges();
			row.Delete();
			base.UpdateData(Adp,OpData);
			
			if(System.IO.File.Exists(path))
			{
				System.IO.File.Delete(path);
			}
		}
		public void DeleteOperatorGroup(string GroupId,string path)
		{
			this.DeleteOperatorGroup(Int32.Parse(GroupId),path);
		}
	}

}
