using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using lw.Data;
using lw.Utils;
using lw.WebTools;

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
             int? Privacy, bool MembershipApproval, string Description, string Mission, int? Security, GroupType Type, string LegalStatus,
            GroupStatus Status, string Phone, string Address)
        {
            var test = TestIfGroupUnique(Name, Type);

            if (test != null)
                return -1;

            Group g = new Group
            {
                Guid = System.Guid.NewGuid(),
                CreatorID = memberId,
                Name = Name,
                UniqueName = StringUtils.ToURL(Name),
                Phone = Phone,
                Address = Address,
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
                Status = (int)Status,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            DataContext.Groups.InsertOnSubmit(g);
            DataContext.SubmitChanges();
            if (Picture != null)
            {
                updateGroupPicture(g.ID, Picture, false);
            }

            UpdateGroupUniqueName(g.ID, StringUtils.ToURL(string.Format("{0}-{1}", Name, g.ID)));

            return g.ID;
        }

        public int CreateGroup(int memberId, string Name, string City, int? Region, int? Country, int? University, System.Web.HttpPostedFile Picture,
             int? Privacy, bool MembershipApproval, string Description, string Mission, int? Security, GroupType Type, string LegalStatus,
            GroupStatus Status, string Phone, string Address, string ParentUniqueName)
        {
            var test = TestIfGroupUnique(Name, Type);

            if (test != null)
                return -1;

            Group g = new Group
            {
                Guid = System.Guid.NewGuid(),
                CreatorID = memberId,
                Name = Name,
                UniqueName = StringUtils.ToURL(ParentUniqueName != null ? string.Format("{0}-{1}", ParentUniqueName, Name) : Name),
                Phone = Phone,
                Address = Address,
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
                Status = (int)Status,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            DataContext.Groups.InsertOnSubmit(g);
            DataContext.SubmitChanges();
            if (Picture != null)
            {
                updateGroupPicture(g.ID, Picture, false);
            }


            return g.ID;
        }

        public int UpdateGroup(string UniqueName, int memberId, string Name, string City, int? Region, int? Country, int? University, System.Web.HttpPostedFile Picture,
             int? Privacy, bool MembershipApproval, string Description, string Mission, int? Security, bool DeleteOldPicture, string Phone, string Address)
        {
            var group = GetGroup(UniqueName);

            //missing test if the member is not regular

            //group.Name = Name;
            //	group.City = City;
            //	group.Region = Region;
            //	group.Country = Country;
            //	group.University = University;
            group.Address = Address;
            group.Phone = Phone;
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

        public int UpdateGroupUniqueName(int GroupID, string UniqueName)
        {
            var group = GetGroup(GroupID);
            group.UniqueName = UniqueName;

            DataContext.SubmitChanges();

            return group.ID;
        }

        public int UpdateChapterInfo(string uniqueName, string name, string description, string mission, int? universityId, int? countryId, string address, string phone, int? Security)
        {
            var group = GetGroup(uniqueName);
            //	group.Name = name;
            group.Description = description;
            group.Mission = mission;
            group.Address = address;
            group.Phone = phone;
            group.University = universityId;
            group.Country = countryId;
            group.Security = Security;
            group.DateModified = DateTime.Now;
            DataContext.SubmitChanges();
            return group.ID;
        }

        public int UpdateGroup(string UniqueName, int memberId, int? Privacy, bool MembershipApproval, string Description, string Mission, int? Security, string Phone, string Address)
        {
            var group = GetGroup(UniqueName);

            group.Address = Address;
            group.Phone = Phone;
            group.Privacy = Privacy;
            group.MembershipApproval = MembershipApproval;
            group.Description = Description;
            group.Mission = Mission;
            group.Security = Security;
            group.DateModified = DateTime.Now;

            DataContext.SubmitChanges();

            return group.ID;
        }

        public bool updateGroupPicture(int groupId, System.Web.HttpPostedFile Picture, bool DeleteOldPicture)
        {
            var group = GetGroup(groupId);
            string ImageName = "";
            string path = WebContext.Server.MapPath("~/" + cte.GroupsFolder);

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

                lw.GraphicUtils.ImageUtils.Resize(path, path.Replace(".jpg", "-l.jpg"), cte.GroupPictureWidth, cte.GroupPictureHeight);
                lw.GraphicUtils.ImageUtils.Resize(path, path.Replace(".jpg", "-m.jpg"),
                    cte.MGroupPictureWidth, cte.MGroupPictureHeight);
                lw.GraphicUtils.ImageUtils.CropImage(path, path.Replace(".jpg", "-s.jpg"),
                    cte.SGroupPictureWidth, cte.SGroupPictureHeight, GraphicUtils.ImageUtils.AnchorPosition.Default);


                DeleteOldPicture = true;

            }

            if (DeleteOldPicture)
            {
                string oldpath = "";
                if (!String.IsNullOrEmpty(group.Picture))
                {
                    oldpath = Path.Combine(path, group.Picture.Replace(".jpg", "-l.jpg"));
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

            string path = WebContext.Server.MapPath("~/" + cte.GroupsFolder);

            path = Path.Combine(path, string.Format("{0}", group.Guid));

            if (File.Exists(path))
                File.Delete(path);

            DataContext.Groups.DeleteOnSubmit(group);
            DataContext.SubmitChanges();

            return true;
        }

        public void UpdateGroupStatus(int groupId, GroupStatus Status)
        {
            var group = GetGroup(groupId);

            group.Status = (int)Status;
            DataContext.SubmitChanges();
        }

        public void AddMemberToGroup(int groupId, int memberId, int Level, GroupMemberStatus Status)
        {
            GroupsMember g = new GroupsMember
            {
                GroupID = groupId,
                MemberID = memberId,
                MemberLevel = Level,
                Status = (int)Status,
                DateJoined = DateTime.Now
            };

            DataContext.GroupsMembers.InsertOnSubmit(g);
            DataContext.SubmitChanges();
        }

        public bool RemoveMemberFromGroup(int groupId, int memberId)
        {
            var g = GetGroupMember(groupId, memberId);

            if (g != null)
            {
                DataContext.GroupsMembers.DeleteOnSubmit(g);
                DataContext.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateMemberGroupRequest(int groupId, int memberId, bool request)
        {
            var g = GetGroupMember(groupId, memberId);

            if (g != null)
            {
                if (request)
                    g.Status = (int)GroupMemberStatus.Approved;
                else
                    RemoveMemberFromGroup(groupId, memberId);

                DataContext.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateMemberGroupRequest(int groupId, int memberId, GroupMemberStatus GroupStatus)
        {
            var g = GetGroupMember(groupId, memberId);

            if (g != null)
            {
                g.Status = (int)GroupStatus;

                DataContext.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateMemberGroupStatus(int groupId, int memberId, GroupMemberLevel MemberLevel)
        {
            var g = GetGroupMember(groupId, memberId);

            if (g != null)
            {
                g.MemberLevel = (int)MemberLevel;

                DataContext.SubmitChanges();
                return true;
            }
            return false;
        }

        public void UpdateGroupMembersStatus(int groupId, bool request)
        {
            string sql = "";

            if (request)
            {
                sql = string.Format("update groupsmembers set status={0} where groupid={1}",
                   (int)GroupMemberStatus.Approved, groupId);
            }
            else
            {
                sql = string.Format("update groupsmembers set status={0} where groupid={1}",
                   (int)GroupMemberStatus.Rejected, groupId);
            }
            DBUtils.ExecuteQuery(sql, cte.lib);
        }

        public Group GetGroup(int GroupId)
        {
            return DataContext.Groups.Single(temp => temp.ID == GroupId);
        }

        public Group GetGroup(string UniqueName)
        {
            var q = from g in DataContext.Groups
                    where g.UniqueName == UniqueName
                    select g;
            if (q.Count() > 0)
                return q.First();
            return null;
        }

        public GroupsView GetGroupView(string UniqueName)
        {
            var q = from g in DataContext.GroupsViews
                    where g.UniqueName == UniqueName
                    select g;
            if (q.Count() > 0)
                return q.First();
            return null;
        }
        public GroupsInfo GetGroupViewInfo(string UniqueName)
        {
            var q = from g in DataContext.GroupsInfos
                    where g.UniqueName == UniqueName
                    select g;
            if (q.Count() > 0)
                return q.First();
            return null;
        }

        public GroupsInfo GetGroupViewInfo(int GroupId)
        {
            var q = from g in DataContext.GroupsInfos
                    where g.ID == GroupId
                    select g;
            if (q.Count() > 0)
                return q.First();
            return null;
        }

        public Group TestIfGroupUnique(string Name, GroupType Type)
        {
            var q = from g in DataContext.Groups
                    where g.UniqueName == StringUtils.ToURL(Name)
                    && g.Type == (int)Type
                    select g;
            if (q.Count() > 0)
                return q.First();
            return null;
        }

        public GroupMembersCount GetGroupMembersCount(int GroupId)
        {
            return DataContext.GroupMembersCounts.Single(temp => temp.GroupID == GroupId);
        }

        public GroupsMember GetGroupMember(int GroupId, int MemberId)
        {
            var q = from g in DataContext.GroupsMembers
                    where g.GroupID == GroupId
                    && g.MemberID == MemberId
                    select g;
            if (q.Count() > 0)
                return q.First();
            return null;
        }

        public GroupsMembersView GetGroupMemberView(string UniqueName)
        {
            var q = from g in DataContext.GroupsMembersViews
                    where g.UniqueName == UniqueName
                    && g.MemberLevel == (int)GroupMemberLevel.President
                    select g;
            if (q.Count() > 0)
                return q.First();
            return null;
        }


        public GroupsMembersView GetGroupMemberView(int GroupId)
        {
            var q = from g in DataContext.GroupsMembersViews
                    where g.GroupID == GroupId
                    && g.MemberLevel == (int)GroupMemberLevel.President
                    select g;
            if (q.Count() > 0)
                return q.First();
            return null;
        }


        public GroupsMembersView GetGroupMemberView(int GroupId, int MemberId)
        {
            var q = from g in DataContext.GroupsMembersViews
                    where g.GroupID == GroupId
                    && g.MemberId == MemberId
                    select g;
            if (q.Count() > 0)
                return q.First();
            return null;
        }


        public GroupsMembersView GetGroupMemberView(string UniqueName, int MemberId)
        {
            var q = from g in DataContext.GroupsMembersViews
                    where g.UniqueName == UniqueName
                    && g.MemberId == MemberId
                    select g;
            if (q.Count() > 0)
                return q.First();
            return null;
        }

        public string GetSearchGroupsString(string name, GroupType Type, string max)
        {
            string _max = "100 PERCENT";

            if (!string.IsNullOrEmpty(max))
                _max = max.ToString();

            StringBuilder sql = new StringBuilder(string.Format("select Top {0} * from GroupsNetworkView where 1=1", _max));

            sql.Append(string.Format(" and privacy<>{0} and status={1} and Type={2}", (int)GroupPrivacy.Secret, (int)GroupStatus.Enabled, (int)Type));

            if (!String.IsNullOrEmpty(name))
                sql.Append(string.Format(" and Name like N'%{0}%'", StringUtils.SQLEncode(name)));

            //sql.Append(" order by Groups.DateCreated Desc");

            return sql.ToString();
        }

        public DataTable SearchGroups(string name, GroupType Type, string max)
        {
            return DBUtils.GetDataSet(GetSearchGroupsString(name, Type, max), cte.lib).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="max"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        /// 
        public DataTable GetGroupInfo(string UniqueName)
        {
            return GetGroupInfo(GetGroup(StringUtils.SQLEncode(UniqueName)).ID);
        }
        public DataTable GetGroupInfo(int groupId)
        {
            StringBuilder sql = new StringBuilder(string.Format("select GroupsMembersView.* from GroupsMembersView where 1=1"));

            sql.Append(string.Format(" and groupId={0}", groupId));

            sql.Append(" order by GroupsMembersView.UserName");

            return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0];
        }
        public DataTable GetGroupInfo(int groupId, string cond)
        {
            StringBuilder sql = new StringBuilder(string.Format("select GroupsMembersView.* from GroupsMembersView where 1=1"));

            sql.Append(string.Format(" and groupId={0}", groupId));

            if (!string.IsNullOrEmpty(cond))
                sql.Append(string.Format(" and {0}", cond));

            return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0];
        }


        public DataTable GetGroupInfo(string UniqueName, string cond)
        {
            StringBuilder sql = new StringBuilder(string.Format("select GroupsMembersView.* from GroupsMembersView where 1=1"));

            sql.Append(string.Format(" and UniqueName='{0}'", StringUtils.SQLEncode(UniqueName)));

            if (!string.IsNullOrEmpty(cond))
                sql.Append(string.Format(" and {0}", cond));

            return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0];
        }



        /// <summary>
        /// Get the list of members in a group
        /// </summary>
        /// <param name="UniqueName"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        /// 

        public DataTable GetGroupMembers(string UniqueName, string max, GroupMemberStatus Status)
        {
            return GetGroupMembers(GetGroup(StringUtils.SQLEncode(UniqueName)).ID, max, Status);
        }

        public string GetGroupMembersSelectCommand(string UniqueName, string max, GroupMemberStatus Status)
        {
            return GetGroupMembersSelectCommand(GetGroup(StringUtils.SQLEncode(UniqueName)).ID, max, Status, "");
        }


        public string GetGroupMembersSelectCommand(int groupId, string max, GroupMemberStatus Status)
        {
            return GetGroupMembersSelectCommand(groupId, max, Status, "");
        }


        public string GetGroupMembersSelectCommand(string UniqueName, string max, GroupMemberStatus Status, string Cond)
        {
            return GetGroupMembersSelectCommand(GetGroup(StringUtils.SQLEncode(UniqueName)).ID, max, Status, Cond);
        }

        public string GetGroupMembersSelectCommand(int groupId, string max, GroupMemberStatus Status, string Cond)
        {
            string _max = "100 PERCENT";

            if (!string.IsNullOrEmpty(max))
                _max = max.ToString();

            StringBuilder sql = new StringBuilder(string.Format("select Top {0} GroupsMembersView.* from GroupsMembersView where 1=1", _max));

            sql.Append(string.Format(" and groupId={0} and memberstatus={1} {2}", groupId, (int)Status, Cond));

            sql.Append(" order by GroupsMembersView.UserName");

            return sql.ToString();
        }

        public DataTable GetGroupMembers(int groupId, string max, GroupMemberStatus Status)
        {
            string _max = "100 PERCENT";

            if (!string.IsNullOrEmpty(max))
                _max = max.ToString();

            StringBuilder sql = new StringBuilder(string.Format("select Top {0} GroupsMembersView.* from GroupsMembersView where 1=1", _max));

            sql.Append(string.Format(" and groupId={0} and memberstatus={1}", groupId, (int)Status));

            sql.Append(" order by GroupsMembersView.UserName");

            return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0];
        }

        /// <summary>
        /// Get the managers list for a group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public DataTable GetGroupManagement(int groupId)
        {
            StringBuilder sql = new StringBuilder("select GroupsMembersView.* from GroupsMembersView where 1=1");

            sql.Append(string.Format(" and groupId={0} and memberstatus={1} and memberlevel<>{2}"
                , groupId, (int)GroupMemberStatus.Approved, (int)GroupMemberLevel.Regular));

            sql.Append(" order by GroupsMembersView.MemberLevel");

            return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0];
        }

        /// <summary>
        /// If groupid is negatif
        ///		Get The list of created, joined and pending groups for a member
        /// else
        ///		Get the view information of a single group
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="Type"></param>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        public DataTable GetMemberGroups(int memberId, GroupType Type, int GroupId)
        {
            string sql = GetMemberGroupsSelectCommand(memberId, Type, GroupId);

            return DBUtils.GetDataSet(sql, cte.lib).Tables[0];
        }


        /// <summary>
        /// If groupid is negatif
        ///		Get The list of created, joined and pending groups for a member
        /// else
        ///		Get the view information of a single group
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="Type"></param>
        /// <param name="GroupId"></param>
        /// <returns>The sql select string for membergroups</returns>
        public string GetMemberGroupsSelectCommand(int memberId, GroupType Type, int GroupId)
        {
            return GetMemberGroupsSelectCommand(memberId, Type, GroupId, null, null);
        }


        /// <summary>
        /// If groupid is negatif
        ///		Get The list of created, joined and pending groups for a member
        /// else
        ///		Get the view information of a single group
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="Type"></param>
        /// <param name="GroupId"></param>
        /// <returns>The sql select string for membergroups</returns>
        public string GetMemberGroupsSelectCommand(int memberId, GroupType Type, int GroupId, string cond)
        {
            return GetMemberGroupsSelectCommand(memberId, Type, GroupId, null, cond);
        }

        /// <summary>
        /// If groupid is negatif
        ///		Get The list of created, joined and pending groups for a member
        /// else
        ///		Get the view information of a single group
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="Type"></param>
        /// <param name="GroupId"></param>
        /// <param name="Max"></param>
        /// <returns>The sql select string for membergroups</returns>
        public string GetMemberGroupsSelectCommand(int memberId, GroupType Type, int GroupId, string max, string cond)
        {
            string _max = "100 PERCENT";

            if (!string.IsNullOrEmpty(max))
                _max = max.ToString();

            StringBuilder sql = new StringBuilder(string.Format("select Top {0} * from GroupsView where 1=1", _max));
            sql.Append(string.Format(" and MemberID={0} and Type={1}", memberId, (int)Type));

            if (GroupId > 0)
                sql.Append(string.Format(" and ID={0}", GroupId));


            if (!string.IsNullOrEmpty(cond))
                sql.Append(string.Format(" and {0}", cond));

            return sql.ToString();
        }

        /// <summary>
        /// Get the preferred chapters for a member
        /// </summary>
        /// <param name="NetworkId"></param>
        /// <param name="memberId"></param>
        /// <param name="Max"></param>
        /// <returns>The sql select string for membergroups</returns>
        public string GetMemberPreferedNetworksSelectCommand(int NetworkId, int memberId, string max)
        {
            string _max = "100 PERCENT";

            if (!string.IsNullOrEmpty(max))
                _max = max.ToString();

            StringBuilder sql = new StringBuilder(string.Format("select top {0} Networks.NetworkId, Networks.Name, Networks.UniqueName, "
            + " Networks.Description, Networks.DateCreated, Networks.LastModified, Networks.Website, Networks.Image, MemberNetworks.Prefered "
            + " from Networks inner join MemberNetworks on Networks.NetworkId = MemberNetworks.NetworkId where 1=1", _max));
            sql.Append(string.Format(" and MemberNetworks.MemberId = {0} and MemberNetworks.Prefered = 1 order by DateCreated", memberId));

            if (NetworkId > 0)
                sql.Append(string.Format(" and NetworkId={0}", NetworkId));

            return sql.ToString();
        }

        public string GetMemberChaptersWithPreferredNetwork(int memberId, string max)
        {
            string _max = " 100 PERCENT ";

            if (!string.IsNullOrEmpty(max))
                _max = max.ToString();

            StringBuilder sql = new StringBuilder(string.Format("select top {1} Networks.NetworkId, Networks.Name, Networks.UniqueName, "
                + " Networks.Description, Networks.DateCreated, Networks.LastModified, Networks.Website, Networks.Image, c.Prefered "
                + " from networks inner join "
                + " (select * from (select top 1 Networks.NetworkId, MemberNetworks.Prefered  "
                    + " from Networks inner join MemberNetworks on Networks.NetworkId = MemberNetworks.NetworkId where 1=1  "
                    + " and MemberNetworks.MemberId = {0} and MemberNetworks.Prefered = 1 order by DateCreated) a "
                    + " union "
                    + " select * from "
                    + " (select MemberNetworks.NetworkId, Prefered from MemberNetworks inner join  "
                        + " (select networkId from Networks where uniquename in( "
                            + " select uniquename from groups where groups.uniquename in "
                                + " (select uniquename from networks inner join "
                                    + " (select NetworkId, Prefered from MemberNetworks where 1=1 and MemberId = {0} and (Prefered is null or Prefered = 0))z "
                                    + " on z.NetworkId = Networks.NetworkId)))zz "
                        + " on zz.NetworkId = MemberNetworks.NetworkId where memberid={0})b "
                    + " )c "
                + " on Networks.NetworkId=c.NetworkId "
                + " order by DateCreated", memberId, _max));
            return sql.ToString();
        }

        public string GetMemberChapters(int memberId, string max)
        {
            string _max = " 100 PERCENT ";

            if (!string.IsNullOrEmpty(max))
                _max = max.ToString();

            StringBuilder sql = new StringBuilder(string.Format(@"select top {1} N.NetworkId, G.GroupName AS Name, N.UniqueName,
	            G.Description, G.Mission, N.DateCreated, N.LastModified, N.Website, N.Image 
	            from Networks AS N
	            inner join GroupsMembersView AS G on N.uniquename = G.uniquename
	            where memberid={0} and type=2
	            order by DateCreated", memberId, _max));
            return sql.ToString();
        }

        /// <summary>
        /// Get list of friends for a member not in a group that he is admin in
        /// </summary>
        /// <param name="MemberId"></param>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        public DataTable GetFriendsNotInGroup(int MemberId, int GroupId)
        {
            string sql = string.Format("select MemberId,Name as UserName,Online,Picture,Email from memberView where MemberId in (select FriendId from Friends where Status=1 and MemberId={0}) and MemberId in (select MemberId from Friends where Status=1 and FriendId={0}) and MemberId not in (select MemberId from GroupsMembers where GroupId={1}) order by Online Desc,UserName ", MemberId, GroupId);
            return DBUtils.GetDataSet(sql, cte.lib).Tables[0];
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
