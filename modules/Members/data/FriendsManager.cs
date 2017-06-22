using System;
using System.Data;
using lw.Data;
using lw.Network;
using lw.WebTools;


namespace lw.Members
{
	public class FriendsManager : MembersManager
	{
		public FriendsManager()
		{
		}

		public DataTable GetFriendsRawData(int MemberId)
		{
			string sql = string.Format("select * from Friends where FriendId={0} or MemberId={0}", MemberId);
			return DBUtils.GetDataSet(sql, cte.lib).Tables[0];
		}

		public DataTable GetFriends(int MemberId)
		{
			string sql = string.Format("select MemberId,Name,Online,Picture,UserName, StatusMessage, LastLogin from memberView where MemberId in (select FriendId from Friends where Status=1 and MemberId={0}) and MemberId in (select MemberId from Friends where Status=1 and FriendId={0})  order by Online Desc,FirstName ASC ", MemberId);
			return DBUtils.GetDataSet(sql, cte.lib).Tables[0];
		}
		public DataTable GetInbox(int MemberId)
		{
            //string sql = string.Format("WITH cte AS (select  MemberId,Name,Online,Picture,UserName, StatusMessage, LastLogin, TimeStamp, Message, Geuid, chat_conversation_messages.ConversationId, ConversationType, chat_conversation_messages.MessageId from memberView" +
            //    " inner join chat_conversation_users_view on chat_conversation_users_view.UserId1 = MemberView.MemberId left join chat_conversation_messages on chat_conversation_messages.FromId = MemberId and chat_conversation_messages.ConversationId = chat_conversation_users_view.ConversationId" +
            //    " where chat_conversation_users_view.UserId2 = {0} and chat_conversation_users_view.ConversationType=1 and MemberId in (select FriendId from Friends where Status=1 and MemberId={0}) and MemberId in (select MemberId from Friends where Status=1 and FriendId={0}))" +
            //    " SELECT MemberId,Name,Online,Picture,UserName, StatusMessage, TimeStamp, Message FROM (SELECT *, ROW_NUMBER() OVER (Partition BY Geuid, MemberId Order By TimeStamp desc) AS RowNum FROM cte) T1 WHERE T1.RowNum = 1", MemberId);
            string sql = string.Format("WITH cte AS (select  MemberId,Name,Online,Picture,UserName, StatusMessage, LastLogin, TimeStamp, Message, Geuid, chat_conversation_messages.ConversationId, ConversationType, chat_conversation_messages.MessageId from memberView" +
                " inner join chat_conversation_users_view on chat_conversation_users_view.UserId1 = MemberView.MemberId left join chat_conversation_messages on chat_conversation_messages.FromId = MemberId and chat_conversation_messages.ConversationId = chat_conversation_users_view.ConversationId" +
                " where chat_conversation_users_view.UserId2 = {0} and chat_conversation_users_view.ConversationType=1)" +
                " SELECT MemberId,Name,Online,Picture,UserName, StatusMessage, TimeStamp, Message FROM (SELECT *, ROW_NUMBER() OVER (Partition BY Geuid, MemberId Order By TimeStamp desc) AS RowNum FROM cte) T1 WHERE T1.RowNum = 1", MemberId);
			return DBUtils.GetDataSet(sql, cte.lib).Tables[0];
		}
        public DataTable GetInboxOmitEmpty(int MemberId)
		{
			string sql = string.Format("WITH cte AS (select  MemberId,Name,Online,Picture,UserName, StatusMessage, LastLogin, TimeStamp, Message, Geuid, chat_conversation_messages.ConversationId, ConversationType, chat_conversation_messages.MessageId "
	        + " from MemberView "
	        +" left join chat_conversation_users_view on chat_conversation_users_view.UserId1 = MemberView.MemberId  "
		        +" left join chat_conversation_messages on chat_conversation_messages.FromId = MemberId  "
			        +" and chat_conversation_messages.ConversationId = chat_conversation_users_view.ConversationId  "
		        +"		where MemberView.MemberId in (select UserId1 from chat_conversation_users_view  "
	        +" inner join chat_conversation_messages on chat_conversation_users_view.ConversationId=chat_conversation_messages.ConversationId "
	        +"	where userid1={0} or userid2={0})  "
            //+ "  and MemberId in "
            //+ "		(select FriendId from Friends where Status=1 and MemberId={0})  "
            //+ "			and MemberId in (select MemberId from Friends where Status=1 and FriendId={0}) "
            + ") SELECT MemberId,Name,Online,Picture,UserName, StatusMessage, TimeStamp, Message FROM (SELECT *, ROW_NUMBER() OVER (Partition BY Geuid, MemberId Order By TimeStamp desc) AS RowNum FROM cte) T1 WHERE T1.RowNum = 1 ", MemberId);
			return DBUtils.GetDataSet(sql, cte.lib).Tables[0];
		}
		public DataTable GetChatHistory(int conversationId, int max)
		{
			string sql = string.Format("select * from (select top {0} * from chat_conversation_messages where chat_conversation_messages.ConversationId = {1} order by TimeStamp desc)t order by t.TimeStamp asc", max, conversationId);
			return DBUtils.GetDataSet(sql, cte.lib).Tables[0];
		}
		public int GetFriendsCount(int MemberId)
		{
			string sql = string.Format("select count(*) as FriendsCount from memberView where MemberId in (select FriendId from Friends where Status=1 and MemberId={0}) and MemberId in (select MemberId from Friends where Status=1 and FriendId={0})", MemberId);
			return (int)DBUtils.GetDataSet(sql, cte.lib).Tables[0].Rows[0]["FriendsCount"];
		}

		public bool AddFriend(int MemberId, string FriendId, bool notify)
		{
			MembersDs.MembersRow member = GetMemberByGuId(FriendId);
			if (member != null)
				return AddFriend(MemberId, member.MemberId, notify);
			return false;
		}
		public bool AddFriend(int MemberId, int FriendId, bool notify)
		{
			string sql = String.Format("select * from Friends where MemberId={0} and FriendId={1}",
				MemberId, FriendId);
			DataTable friends = DBUtils.GetDataSet(sql, cte.lib).Tables[0];
			if (friends.Rows.Count > 0)
			{
				sql = string.Format("Update Friends set Status={0} where MemberId={1} and FriendId={2}",
					(int)FriendStatus.Approved, MemberId, FriendId);
				DBUtils.ExecuteQuery(sql, cte.lib);
			}
			else
			{
				sql = string.Format("Insert into Friends (MemberId, FriendId, Status, DateAdded) values ({0}, {1}, {2}, getdate())",
				MemberId, FriendId, (int)FriendStatus.Approved);
				DBUtils.ExecuteQuery(sql, cte.lib);
			}

			sql = String.Format("select * from Friends where MemberId={0} and FriendId={1}",
				FriendId, MemberId);
			friends = DBUtils.GetDataSet(sql, cte.lib).Tables[0];
			if (friends.Rows.Count == 0)
			{
				sql = string.Format("Insert into Friends (MemberId, FriendId, Status, DateAdded) values ({0}, {1}, {2}, getdate())",
				FriendId, MemberId, (int)FriendStatus.Pending);
				DBUtils.ExecuteQuery(sql, cte.lib);
			}


			MembersDs.MembersRow member = GetMember(MemberId);
			MembersDs.MembersRow friend = GetMember(FriendId);

			if (notify)
			{
				Config cfg = new Config();
				Mail mail = new Mail("friend-request");
				mail.Data = new System.Collections.Specialized.NameValueCollection();
				mail.Data["MemberId"] = member["Geuid"].ToString();
				mail.Data["MemberName"] = member.FirstName + " " + member.LastName;
				mail.Data["MemberEmail"] = member.Email;
				mail.Data["Picture"] = (member["Picture"] != System.DBNull.Value ? member["Picture"] : "").ToString();
				mail.Data["FriendId"] = friend["Geuid"].ToString();
				mail.Data["FriendName"] = friend.FirstName + " " + friend.LastName;
				mail.Data["FriendEmail"] = friend.Email;
				mail.Data["FriendPicture"] = (friend["Picture"] != System.DBNull.Value ? friend["Picture"] : "").ToString();
				if (!String.IsNullOrWhiteSpace(friend.Email))
					mail.To = friend.Email;
				if (!String.IsNullOrWhiteSpace(friend.AlternateEmail))
					mail.To = friend.AlternateEmail;

				mail.Subject = member.FirstName + " " + member.LastName + " Added you on " + cfg.GetKey("SiteName");
				mail.Send();
			}
			return true;
		}

		public bool AcceptFriendShip(int MemberId, string FriendId, bool notify)
		{
			MembersDs.MembersRow member = GetMemberByGuId(FriendId);
			if (member != null)
				return AcceptFriendShip(MemberId, member.MemberId, notify);
			return false;
		}
		public bool AcceptFriendShip(int MemberId, int FriendId, bool notify)
		{
			string sql = String.Format("select * from Friends where MemberId={0} and FriendId={1}",
				MemberId, FriendId);
			DataTable friends = DBUtils.GetDataSet(sql, cte.lib).Tables[0];
			if (friends.Rows.Count > 0)
			{
				sql = string.Format("Update Friends set Status={0} where MemberId={1} and FriendId={2}",
					(int)FriendStatus.Approved, MemberId, FriendId);
				DBUtils.ExecuteQuery(sql, cte.lib);
			}

			MembersDs.MembersRow member = GetMember(MemberId);
			MembersDs.MembersRow friend = GetMember(FriendId);

			if (notify)
			{
				Config cfg = new Config();
				Mail mail = new Mail("friend-accepted");
				mail.Data = new System.Collections.Specialized.NameValueCollection();
				mail.Data["MemberId"] = member["Geuid"].ToString();
				mail.Data["MemberName"] = member.FirstName + " " + member.LastName;
				mail.Data["MemberEmail"] = member.Email;
				mail.Data["Profile"] = member.UserName;
				mail.Data["Picture"] = (member["Picture"] != System.DBNull.Value ? member["Picture"] : "").ToString();
				mail.Data["FriendId"] = friend["Geuid"].ToString();
				mail.Data["FriendName"] = friend.FirstName + " " + friend.LastName;
				mail.Data["FriendEmail"] = friend.Email;
				mail.Data["FriendPicture"] = (friend["Picture"] != System.DBNull.Value ? friend["Picture"] : "").ToString();
				mail.To = friend.Email;

				if (!String.IsNullOrWhiteSpace(friend.AlternateEmail))
					mail.ToList.Add(friend.AlternateEmail);

				mail.Subject = member.FirstName + " " + member.LastName + " Accepted your Request on " + cfg.GetKey("SiteName");
				mail.Send();
			}
			return true;
		}
		public bool DeclineFriendship(int MemberId, string FriendId, bool notify)
		{
			MembersDs.MembersRow member = GetMemberByGuId(FriendId);
			if (member != null)
				return DeclineFriendship(MemberId, member.MemberId, notify);
			return false;
		}
		public bool DeclineFriendship(int MemberId, int FriendId, bool notify)
		{
			string sql = "Update Friends set Status={0} where MemberId={1} and FriendId={2}; Update Friends set Status={0} where MemberId={2} and FriendId={1}";
			sql = string.Format(sql, (int)FriendStatus.Deleted, MemberId, FriendId);
			DBUtils.ExecuteQuery(sql, cte.lib);


			MembersDs.MembersRow member = GetMember(MemberId);
			MembersDs.MembersRow friend = GetMember(FriendId);

			if (notify)
			{
				Config cfg = new Config();
				Mail mail = new Mail("friend-declined");
				mail.Data = new System.Collections.Specialized.NameValueCollection();
				mail.Data["MemberId"] = member["Geuid"].ToString();
				mail.Data["MemberName"] = member.FirstName + " " + member.LastName;
				mail.Data["MemberEmail"] = member.Email;
				mail.Data["Profile"] = member.UserName;
				mail.Data["Picture"] = (member["Picture"] != System.DBNull.Value ? member["Picture"] : "").ToString();
				mail.Data["FriendId"] = friend["Geuid"].ToString();
				mail.Data["FriendName"] = friend.FirstName + " " + friend.LastName;
				mail.Data["FriendEmail"] = friend.Email;
				mail.Data["FriendPicture"] = (friend["Picture"] != System.DBNull.Value ? friend["Picture"] : "").ToString();
				mail.To = friend.Email;
				if (!String.IsNullOrWhiteSpace(friend.AlternateEmail))
					mail.ToList.Add(friend.AlternateEmail);
				mail.Subject = member.FirstName + " " + member.LastName + " Declined your Request on " + cfg.GetKey("SiteName");
				mail.Send();
			}
			return true;
		}


		public static DataTable GetMyFriends(lw.Base.CustomPage page)
		{
			DataTable MyFriends = null;
			if (page.PageContext["Friends"] != null)
			{
				MyFriends = page.PageContext["Friends"] as DataTable;
			}
			if (MyFriends == null)
			{
				FriendsManager fMgr = new FriendsManager();
				MyFriends = fMgr.GetFriendsRawData(WebContext.Profile.UserId);
				page.PageContext["Friends"] = MyFriends;
			}

			return MyFriends;
		}

		public bool CheckFriendship(int MemberId, int FriendId, FriendStatus Status)
		{
			string sql = "select * from Friends where MemberId={0} and FriendId={1} and Status={2}";
			sql = string.Format(sql, MemberId, FriendId, (int)Status);
			DataTable friends = DBUtils.GetDataSet(sql, cte.lib).Tables[0];

			if (friends.Rows.Count > 0)
				return true;
			return false;
		}
	}
}
