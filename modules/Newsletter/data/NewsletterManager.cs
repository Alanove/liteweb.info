using System;
using System.Data;
using System.Text;


using lw.Data;
using lw.Utils;


namespace lw.Newsletter
{
	/// <summary>
	/// Summary description for OperatorsManager.
	/// </summary>
	public class NewsletterManager : DirectorBase
	{
		public NewsletterManager()
			: base(cte.lib)
		{
			int i = 0;
			i++;
		}
		public DataView GetUsersView(string condition)
		{
			NewsDs _ds = new NewsDs();
			IDataAdapter Adp = base.GetAdapter(cte.NewsletterViewAdp);
			return ((NewsDs)base.FillData(Adp, _ds, condition)).newsletterview.DefaultView;
		}

		public DataView GetUsers(string condition)
		{
			NewsDs _ds = new NewsDs();
			IDataAdapter Adp = base.GetAdapter(cte.NewsleterUsersAdp);
			return ((NewsDs)base.FillData(Adp, _ds, condition)).NewsletterUsers.DefaultView;
		}

		public DataView GetUsersNew(string condition)
		{
			string sql = "select Distinct * from NewsletterViewNew";
			if (!String.IsNullOrEmpty(condition))
				sql += " where  " + condition;
			return DBUtils.GetDataSet(sql, cte.lib).Tables[0].DefaultView;
		}
		
		public DataView GetUsers(int GroupId)
		{
			return GetUsersNew("GroupId=" + GroupId.ToString());
		}
		
		public bool AddUser(System.Web.HttpRequest req)
		{
			return this.AddUser(
				req["Title"], req["FirstName"],
				req["LastName"], req["Company"],
				req["Position"], req["Address"],
				req["City"], req["State"],
				req["ZipCode"], Int32.Parse(req["Country"]),
				req["Phone"], req["Email"], 
				req["Former"], req["Year"],
				req["Children"], req["School"], req["Aware"],
				-1);
		}
		
		public bool AddUser(string Title, string FirstName, string LastName, string Company,
			string Position, string Address, string City, string State, string ZipCode, 
			int Country, string Phone, string Email, string Former, string Year, 
			string Children, string School, string Aware,
			int GroupId)
		{
			System.Data.DataView dv = GetUsers(string.Format("Email='{0}'", StringUtils.SQLEncode(Email)));
			if(dv.Count > 0)
				return false;

			NewsDs _ds = new NewsDs();
			IDataAdapter adp = base.GetAdapter(cte.NewsleterUsersAdp);
			NewsDs.NewsletterUsersRow row = _ds.NewsletterUsers.NewNewsletterUsersRow();
			
			/*row.Title = Title;
			row.FirstName = FirstName;
			row.LastName = LastName;
			row.Company = Company;
			row.Position = Position;
			row.Phone = Phone;
			row.Email = Email;
			row.Former = Former;
			row.Year = Year;
			row.Children = Children;
			row.School = Int32.Parse(School);
			row.Aware = Aware;
			row.GroupId = GroupId;
			*/

			row.Email = Email;
			row.DateAdded = DateTime.Now;

			_ds.NewsletterUsers.AddNewsletterUsersRow(row);
			base.UpdateData(adp, _ds);
			return true;
		}
		
		public DataRow AddUser(string Email, string Name, int GroupId)
		{
			System.Data.DataView dv = GetUsers(string.Format("Email='{0}'", StringUtils.SQLEncode(Email)));
			if (dv.Count > 0)
				return dv[0].Row;

			NewsDs _ds = new NewsDs();
			IDataAdapter adp = base.GetAdapter(cte.NewsleterUsersAdp);
			NewsDs.NewsletterUsersRow row = _ds.NewsletterUsers.NewNewsletterUsersRow();

			row.Name = Name;
			row.Email = Email;
			row.GroupId = GroupId;

			row.DateAdded = DateTime.Now;

			_ds.NewsletterUsers.AddNewsletterUsersRow(row);
			base.UpdateData(adp, _ds);

			AddUserToGroup(row.UserId, GroupId);

			return row;
		}
		
		public bool AddUser(string Email, string Name, string phone, int GroupId)
		{
			System.Data.DataView dv = GetUsers(string.Format("Email='{0}'", StringUtils.SQLEncode(Email)));
			if (dv.Count > 0)
			{
				AddUserToGroup((int)dv[0]["UserId"], GroupId);
				return false;
			}
			NewsDs _ds = new NewsDs();
			IDataAdapter adp = base.GetAdapter(cte.NewsleterUsersAdp);
			NewsDs.NewsletterUsersRow row = _ds.NewsletterUsers.NewNewsletterUsersRow();

			row.Name = Name;
			row.Email = Email;
			row.Phone = phone;

			row.DateAdded = DateTime.Now;

			_ds.NewsletterUsers.AddNewsletterUsersRow(row);
			base.UpdateData(adp, _ds);

			AddUserToGroup(row.UserId, GroupId);

			return true;
		}
		
		public void UpdateUser(int UserId, string Email, string Name, int GroupId)
		{
			NewsDs _ds = new NewsDs();
			IDataAdapter adp = base.GetAdapter(cte.NewsleterUsersAdp);
			NewsDs.NewsletterUsersRow row = _ds.NewsletterUsers.NewNewsletterUsersRow();

			row.UserId = UserId;
			_ds.NewsletterUsers.AddNewsletterUsersRow(row);
			row.AcceptChanges();

			row.Email = Email;
			row.GroupId = GroupId;
			row.Name = Name;

			UpdateUserGroups(row.UserId, GroupId.ToString());

			base.UpdateData(adp, _ds);
		}
		
		public void UpdateUser(int UserId, string Email, string Name, string Groups)
		{
			NewsDs _ds = new NewsDs();
			IDataAdapter adp = base.GetAdapter(cte.NewsleterUsersAdp);
			NewsDs.NewsletterUsersRow row = _ds.NewsletterUsers.NewNewsletterUsersRow();

			row.UserId = UserId;
			_ds.NewsletterUsers.AddNewsletterUsersRow(row);
			row.AcceptChanges();

			row.Email = Email;
			row.Name = Name;

			base.UpdateData(adp, _ds);

			UpdateUserGroups(row.UserId, Groups);
		}
		
		public void UpdateUserGroups(int UserId, string Groups)
		{
			string sql = "delete from NewsletterGroupsUsers where UserId=" + UserId.ToString();
			DBUtils.ExecuteQuery(sql, cte.lib);

			StringBuilder sb = new StringBuilder();

			string[] groups = Groups.Split(',');
			string _temp = "insert into NewsletterGroupsUsers values ({0}, {1});\r\n";
			foreach (string groupId in groups)
			{
				sb.Append(string.Format(_temp, UserId, groupId));
			}
			DBUtils.ExecuteQuery(sb.ToString(), cte.lib);
		}
		public void AddUserToGroup(int UserId, int GroupId)
		{
			string sql = @"if not exists (select * from NewsletterGroupsUsers where UserId={0} and GroupId = {1}) 
				insert into NewsletterGroupsUsers values ({0}, {1});";
			DBUtils.ExecuteQuery(string.Format(sql, UserId, GroupId), cte.lib);
		}
		public DataView GetGroups(string condition)
		{
			NewsDs _ds = new NewsDs();
			IDataAdapter adp = base.GetAdapter(cte.NewsletterGroupsAdp);
			return ((NewsDs)base.FillData(adp, _ds, condition)).NewsletterGroups.DefaultView;
		}
		public int AddGroup(string GroupName)
		{
			System.Data.DataView dv = GetGroups(string.Format("GroupName='{0}'", StringUtils.SQLEncode(GroupName)));
			if(dv.Count > 0)
				return (int)dv[0]["GroupId"];
			NewsDs _ds = new NewsDs();
			IDataAdapter adp = base.GetAdapter(cte.NewsletterGroupsAdp);
			NewsDs.NewsletterGroupsRow row = _ds.NewsletterGroups.NewNewsletterGroupsRow();
			row.GroupName = GroupName;

			_ds.NewsletterGroups.AddNewsletterGroupsRow(row);
			base.UpdateData(adp, _ds);
			return row.GroupId;
		}
		public void UpdateGroup(int GroupId, string GroupName)
		{
			NewsDs _ds = new NewsDs();
			IDataAdapter adp = base.GetAdapter(cte.NewsletterGroupsAdp);
			NewsDs.NewsletterGroupsRow row = _ds.NewsletterGroups.NewNewsletterGroupsRow();

			row.GroupId = GroupId;
			_ds.NewsletterGroups.AddNewsletterGroupsRow(row);
			row.AcceptChanges();

			row.GroupName = GroupName;

			base.UpdateData(adp, _ds);
		}
		public bool DeleteUser(int UserId)
		{
			NewsDs _ds = new NewsDs();
			IDataAdapter adp = base.GetAdapter(cte.NewsleterUsersAdp);
			NewsDs.NewsletterUsersRow row = _ds.NewsletterUsers.NewNewsletterUsersRow();

			row.UserId = UserId;
			_ds.NewsletterUsers.AddNewsletterUsersRow(row);
			row.AcceptChanges();

			row.Delete();
			base.UpdateData(adp, _ds);
			return true;
		}
		public bool DeleteUser(string Email)
		{
			DataView users = this.GetUsers(string.Format("Email='{0}'", StringUtils.SQLEncode(Email)));
			if (users.Count > 0)
				return DeleteUser((int)users[0]["UserId"]);
			return false;
		}
		public void DeleteGroup(int GroupId)
		{
			NewsDs _ds = new NewsDs();
			IDataAdapter adp = base.GetAdapter(cte.NewsletterGroupsAdp);
			NewsDs.NewsletterGroupsRow row = _ds.NewsletterGroups.NewNewsletterGroupsRow();

			row.GroupId = GroupId;
			_ds.NewsletterGroups.AddNewsletterGroupsRow(row);
			row.AcceptChanges();

			row.Delete();
			
			base.UpdateData(adp, _ds);
		}
	}
	
}