using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

using lw.Data;
using lw.WebTools;
using lw.Utils;
using lw.CTE.Enum;

namespace lw.Groups
{
	public class GroupsManager : LINQManager
	{
		public GroupsManager()
			: base(cte.lib)
		{
		}

		/// <summary>
		/// Create A Group
		/// </summary>
		/// 

		public int CreateGroup(int memberId, string Name, string City, int? Region, int? Country, int? University, System.Web.HttpPostedFile Picture,
			 int? Privacy, bool MembershipApproval, string Description, string Mission, int? Security, GroupType Type, string LegalStatus)
		{
			var test = TestIfGroupUnique(Name);

			if (test != null)
				return -1;

			Group g = new Group
			{
				Guid = System.Guid.NewGuid(),
				CreatorID = memberId,
				Name = Name,
				UniqueName = StringUtils.ToURL(Name),
				City = City,
				Region = Region,
				Country = Country,
				University = University,
				Privacy = Privacy,
				MembershipApproval = MembershipApproval,
				Description = Description,
				Mission = Mission,
				Security = Security,
				Type = (int)Type,
				LegalStatus = LegalStatus,
				Status = (int)lw.CTE.Enum.GroupStatus.Pending,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now
			};

			DataContext.Groups.InsertOnSubmit(g);
			DataContext.SubmitChanges();

			updateGroupPicture(g.ID, Picture, false);

			return g.ID;
		}

		public int UpdateGroup(string UniqueName, int memberId, string Name, string City, int? Region, int? Country, int? University, System.Web.HttpPostedFile Picture,
			 int? Privacy, bool MembershipApproval, string Description, string Mission, int? Security, bool DeleteOldPicture)
		{
			var group = GetGroup(UniqueName);

			if (group.CreatorID != memberId)
				return -1;

			//group.Name = Name;
			group.City = City;
			group.Region = Region;
			group.Country = Country;
			group.University = University;
			group.Privacy = Privacy;
			group.MembershipApproval = MembershipApproval;
			group.Description = Description;
			group.Mission = Mission;
			group.Security = Security;
			group.DateModified = DateTime.Now;

			DataContext.SubmitChanges();

			updateGroupPicture(group.ID, Picture, DeleteOldPicture);

			return group.ID;	
		}

		public bool updateGroupPicture(int groupId, System.Web.HttpPostedFile Picture, bool DeleteOldPicture)
		{
			var group = GetGroup(groupId);
			string ImageName = "";
			string path = WebContext.Server.MapPath("~/" + lw.Groups.cte.GroupsFolder);

			path = Path.Combine(path, string.Format("{0}", group.Guid));

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			if (Picture != null && Picture.ContentLength > 0)
			{
				ImageName = string.Format("{0}.jpg",
					System.Guid.NewGuid().ToString().Substring(16));

				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}

				path = Path.Combine(path, ImageName);

				Picture.SaveAs(path);

				lw.GraphicUtils.ImageUtils.Resize(path, path, lw.Groups.cte.GroupPictureWidth, lw.Groups.cte.GroupPictureHeight);
				lw.GraphicUtils.ImageUtils.CropImage(path, path.Replace(".jpg", "-m.jpg"),
					lw.Groups.cte.MGroupPictureWidth, lw.Groups.cte.MGroupPictureHeight, GraphicUtils.ImageUtils.AnchorPosition.Default);
				lw.GraphicUtils.ImageUtils.CropImage(path, path.Replace(".jpg", "-s.jpg"),
					lw.Groups.cte.SGroupPictureWidth, lw.Groups.cte.SGroupPictureHeight, GraphicUtils.ImageUtils.AnchorPosition.Default);


				DeleteOldPicture = true;

			}

			if (DeleteOldPicture)
			{
				string oldpath = "";
				if (!String.IsNullOrEmpty(group.Picture))
				{
					oldpath = Path.Combine(path, group.Picture);
					if (File.Exists(oldpath))
						File.Delete(oldpath);

					oldpath = Path.Combine(path, group.Picture.Replace(".jpg", "-m.jpg"));
					if (File.Exists(oldpath))
						File.Delete(oldpath);

					oldpath = Path.Combine(path, group.Picture.Replace(".jpg", "-s.jpg"));
					if (File.Exists(oldpath))
						File.Delete(oldpath);
				}
				group.Picture = ImageName;
			}
			DataContext.SubmitChanges();
			return true;
		}


		public bool DeleteGroup(int groupId)
		{
			var group = GetGroup(groupId);

			string path = WebContext.Server.MapPath("~/" + lw.Groups.cte.GroupsFolder);

			path = Path.Combine(path, string.Format("{0}", group.Guid));

			if (File.Exists(path))
				File.Delete(path);

			DataContext.Groups.DeleteOnSubmit(group);
			DataContext.SubmitChanges();

			return true;
		}

		public void AddMemberToGroup(int groupId, int memberId, GroupMemberLevel Level, GroupMemberStatus Status)
		{
			GroupsMember g = new GroupsMember
			{
				GroupID = groupId,
				MemberID = memberId,
				MemberLevel = (int)Level,
				Status = (int)Status,
				DateJoined = DateTime.Now
			};

			DataContext.GroupsMembers.InsertOnSubmit(g);
			DataContext.SubmitChanges();
		}

		public Group GetGroup(int GroupId)
		{
			return DataContext.Groups.Single(temp => temp.ID == GroupId);
		}		

		public Group GetGroup(string UniqueName)
		{
			var q = from g in DataContext.Groups
					where g.UniqueName == StringUtils.SQLEncode(UniqueName)
					select g;
			if (q.Count() > 0)
				return q.First();
			return null;
		}

		public Group TestIfGroupUnique(string Name)
		{
			var q = from g in DataContext.Groups
					where g.UniqueName == StringUtils.SQLEncode(StringUtils.ToURL(Name))
					select g;
			if (q.Count() > 0)
				return q.First();
			return null;
		}

		public DataTable SearchGroups(string name)
		{
			StringBuilder sql = new StringBuilder("select Groups.* from Groups where 1=1");

			sql.Append(string.Format(" and privacy<>{0} and status={1}", (int)GroupPrivacy.Secret, (int)GroupStatus.Enabled));

			if (!String.IsNullOrEmpty(name))
				sql.Append(string.Format(" and (Name like N'%{0}%' or Description like N'%{0}%')", StringUtils.SQLEncode(name)));

			sql.Append(" order by Groups.Name");

			return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0];
		}

		public DataTable GetMyGroups(int memberId, GroupType Type)
		{
			StringBuilder sql = new StringBuilder("select GroupsView.* from GroupsView where 1=1");

			sql.Append(string.Format(" and MemberId={0} and Type={1}", memberId, (int)Type));

			sql.Append(" order by GroupsView.Name");

			return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0];
		}


		public DataTable GetGroupMembers(string UniqueName)
		{
			return GetGroupMembers(GetGroup(StringUtils.SQLEncode(UniqueName)).ID);
		}

		public DataTable GetGroupMembers(int groupId)
		{
			StringBuilder sql = new StringBuilder("select GroupsMembersView.* from GroupsMembersView where 1=1");

			sql.Append(string.Format(" and groupId={0} and memberstatus={1}", groupId, (int)GroupMemberStatus.Approved));

			sql.Append(" order by GroupsMembersView.UserName");

			return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0];
		}
/*
		public DataTable GetGroup(string UniqueName)
		{
			StringBuilder sql = new StringBuilder("select * from Groups where 1=1");

			sql.Append(string.Format(" and privacy<>{0} and status={1}", (int)lw.CTE.Enum.GroupPrivacy.Secret, (int)lw.CTE.Enum.GroupStatus.Enabled));
			sql.Append(string.Format(" and UniqueName= N'{0}'", StringUtils.SQLEncode(UniqueName)));

			return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0];
		}
*/
		public DataTable GetMemberGroups(int memberId)
		{
			StringBuilder sql = new StringBuilder("select GroupsMembers.* from GroupsMembers where 1=1");

			sql.Append(string.Format(" and MemberID={0}", memberId));

			return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0];
		}

		#region Variables


		public GroupsDataContextDataContext DataContext
		{
			get
			{
				if (_dataContext == null)
					_dataContext = new GroupsDataContextDataContext(Connection);
				return (GroupsDataContextDataContext)_dataContext;
			}
		}

		#endregion

	}
}
