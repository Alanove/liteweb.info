using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

using System.Web;
using System.Collections;
using System.Globalization;

using lw.Data;
using lw.Utils;
using lw.WebTools;
using lw.CTE.Enum;
using lw.CTE;


namespace lw.Comments
{
	/// <summary>
	/// Handles all the commenting system of the website
	/// </summary>
	/// <seealso cref="lw.Tags.CommentsDataSource"/>
	/// <seealso cref="lw.Tags.CommentsForm"/>
	public class CommentsManager : LINQManager
	{
		public CommentsManager()
			: base(cte.lib)
		{
		}

		/// <summary>
		/// Returns a comment table details
		/// </summary>
		/// <param name="tableName">table name like News...</param>
		/// <returns></returns>
		public Comments.Comments_Tables_View GetTableDetails(string tableName)
		{
			var q = from CommentTable in DataContext.Comments_Tables_Views
					where CommentTable.TableName == tableName
					select CommentTable;

			if (q.Count() > 0)
				return q.Single();
			return null;
		}


		/// <summary>
		/// Updates comment status
		/// </summary>
		/// <param name="tableName">related Table Name</param>
		/// <param name="commentId">the Comment id</param>
		/// <param name="stat">New Status</param>
		public void UpdateCommentStatus(string tableName, int commentId, Status stat)
		{
			string sql = "Update {0} With(RowLock) set Status={2}   where CommentId={1}";
			sql = string.Format(sql, GetTableName(tableName), commentId, (int)stat);

			DBUtils.ExecuteQuery(sql, cte.lib);
		}

		/// <summary>
		/// Updates the comment text
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="commentText"></param>
		/// <param name="commentId"></param>
		public void UpdateCommentText(string tableName, string commentText, int commentId)
		{
			string sql = "Update Comments_{0} With(RowLock) set CommentText = '{1}' Where CommentId = {2}";
			DBUtils.ExecuteQuery(string.Format(sql, StringUtils.SQLEncode(tableName), StringUtils.SQLEncode(commentText), commentId), lw.Comments.cte.lib);
		}


		/// <summary>
		/// Adds a new comment, public login required
		/// </summary>
		/// <param name="table">related table name</param>
		/// <param name="name">Name</param>
		/// <param name="website">Website</param>
		/// <param name="country">Country</param>
		/// <param name="avatar">Avatar or picture</param>
		/// <param name="commentType">Comment Type (video, picture... )</param>
		/// <returns>The ID of the new Comment</returns>
		public int AddMemberComment(string table, int parentId, int? relationId, string title, string commentText,
			int MemberId, CommentType commentType)
		{
			string sql = @"Insert into {0}  With(RowLock)  
						(ParentId, RelationId, DateCreated, DateModified, Status, AdminId, 
							Title, CommentText, MemberId, CommentType) 
						values ({1}, {2}, '{3}', '{4}', {5}, {6}, N'{7}', N'{8}', {9}, {10});
						Select @@Identity as CommentID;";


			sql = string.Format(sql,
					GetTableName(table),
					parentId, relationId,
					DateTime.Now.ToString(Globalization.DefaultDBCultureInfo),
					DateTime.Now.ToString(Globalization.DefaultDBCultureInfo),
					(int)(Status.Pending | Status.Enabled), 0,
					StringUtils.SQLEncode(title),
					StringUtils.SQLEncode(commentText),
					MemberId,
					(int)commentType);

			DataTable dt = DBUtils.GetDataSet(sql, cte.lib).Tables[0];
			return Int32.Parse(dt.Rows[0]["CommentID"].ToString());
		}


		/// <summary>
		/// Adds a new comment, public no login required
		/// </summary>d
		/// <param name="table">related table name</param>
		/// <param name="parentId">Parent Id used in replies to other comments</param>
		/// <param name="relationId">The related item like NewsId or PhotoAlbumId...</param>
		/// <param name="commentText">text</param>
		/// <param name="visitorId">Visitor Id automatic should be Profile.UserId or New Geuid()</param>
		/// <param name="name">Name</param>
		/// <param name="website">Website</param>
		/// <param name="country">Country</param>
		/// <param name="ipAddress">IP Address Request["Remote_Address"]</param>
		/// <param name="avatar">Avatar or picture</param>
		/// <param name="commentType">Comment Type (video, picture... )</param>
		/// <returns>The ID of the new Comment</returns>
		public int AddNoMemberComment(string table, int parentId, int? relationId, string title,
			string commentText,
			Guid visitorId, string name, string website, string country, string ipAddress, string avatar,
			CommentType commentType)
		{
			string sql = @"Insert into {0}  With(RowLock)  
						(ParentId, RelationId, DateCreated, DateModified, Status, AdminId, 
							Title, CommentText, VisitorID, Name, Website, Country, IPAddress, Avatar, CommentType) 
						values ({1}, {2}, '{3}', '{4}', {5}, {6}, N'{7}', '{8}', N'{9}', N'{10}', N'{11}', 
							N'{12}', '{13}', N'{14}', {15});
						Select @@Identity as CommentID";


			sql = string.Format(sql,
					GetTableName(table),
					parentId, relationId,
					DateTime.Now.ToString(Globalization.DefaultDBCultureInfo),
					DateTime.Now.ToString(Globalization.DefaultDBCultureInfo),
					0, 0,
					StringUtils.SQLEncode(title),
					StringUtils.SQLEncode(commentText), visitorId,
					StringUtils.SQLEncode(String.IsNullOrEmpty(name) ? "" : name),
					StringUtils.SQLEncode(String.IsNullOrEmpty(website) ? "" : website),
					StringUtils.SQLEncode(String.IsNullOrEmpty(country) ? "" : country),
					StringUtils.SQLEncode(ipAddress),
					StringUtils.SQLEncode(String.IsNullOrEmpty(avatar) ? "" : avatar),
					(int)commentType
				);

			DataTable dt = DBUtils.GetDataSet(sql, cte.lib).Tables[0];
			return Int32.Parse(dt.Rows[0]["CommentId"].ToString());
		}

		/// <summary>
		/// Returns comment data based on it's id
		/// </summary>
		/// <param name="commentId"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public DataSet GetComment(int commentId, string tableName)
		{
			string sql = "Select * From Comments_{0} Where CommentId = {1}";
			return DBUtils.GetDataSet(string.Format(sql, StringUtils.SQLEncode(tableName), commentId), lw.Comments.cte.lib);
		} 

		/// <summary>
		/// returns all comments related to 'relationId' optional ParentId
		/// </summary>
		/// <param name="table">table name</param>
		/// <param name="relationId">relation id </param>
		/// <param name="parentId">parent id</param>
		/// <returns></returns>
		public DataTable GetCommentsWithRelations(string table, int relationId, int parentId)
		{
			return DBUtils.GetDataSet(GetCommentsQueryWithRelations(table, relationId, parentId, "100%"), cte.lib).Tables[0];
		}

		/// <summary>
		/// Reutrns the query for comments with relations, 
		/// essential for use with lw.PagingDataSource or lw.DataSource
		/// </summary>
		/// <param name="table">table name</param>
		/// <param name="relationId">relation id</param>
		/// <param name="parentId">parent id</param>
		/// <param name="top">the number of returned comments, put 100% for all</param>
		/// <returns></returns>
		public string GetCommentsQueryWithRelations(string table, int relationId, int parentId, string top)
		{
			string sql = "select top {3} * from {0}  WITH (NOLOCK) where ParentId={1} and RelationId={2}";
			return String.Format(sql, GetTableName(table), parentId, relationId, top);
		}

		/// <summary>
		/// Reutrns the query for comments with no relations For comments with members, 
		/// essential for use with lw.PagingDataSource or lw.DataSource
		/// </summary>
		/// <param name="table">table name</param>
		/// <param name="parentId">parent id</param>
		/// <param name="top">the number of returned comments, put 100% for all</param>
		/// <returns></returns>
		public string GetMemberCommentsQueryWithNoRelations(string table, int parentId, string top)
		{
			string sql = @"select top {1} m.MemberId, m.GeuID, m.UserName, m.Status, m.FirstName, m.LastName, m.FirstName + ' ' + m.LastName as Name, m.Online, m.Picture, m.Privacy,
c.CommentId, C.ParentId, c.RelationId, c.DateCreated, c.DateModified, c.Status, c.AdminId,
c.Title,
c.CommentText, c.CommentType
From Members m WITH (NOLOCK)
Inner Join  {0} C WITH (NOLOCK)
on m.MemberId = c.MemberId
where ";
			if (parentId > 0)
			{
				sql += "ParentId=" + parentId.ToString();
			}
			else
			{
				sql += "ParentId=-1";
			}

			return String.Format(sql, GetTableName(table), top);
		}

		/// <summary>
		/// Reutrns the query for comments with relations For comments with members, 
		/// essential for use with lw.PagingDataSource or lw.DataSource
		/// </summary>
		/// <param name="table">table name</param>
		/// <param name="relationId">relation id</param>
		/// <param name="parentId">parent id</param>
		/// <param name="top">the number of returned comments, put 100% for all</param>
		/// <returns></returns>
		public string GetMemberCommentsQueryWithRelations(string table, int relationId, int parentId, string top)
		{
			string sql = @"select top {1} m.MemberId, m.GeuID, m.UserName, m.Status, m.FirstName, m.LastName, m.FirstName + ' ' + m.LastName as Name, m.Online, m.Picture, m.Privacy,
c.CommentId, C.ParentId, c.RelationId, c.DateCreated, c.DateModified, c.Status, c.AdminId,
c.Title,
c.CommentText, c.CommentType
From Members m WITH (NOLOCK)
Inner Join  {0} C WITH (NOLOCK)
on m.MemberId = c.MemberId
where ";
			if (parentId > 0)
			{
				sql += "ParentId=" + parentId.ToString();
			}
			else
			{
				sql += "ParentId=-1 and RelationId=" + relationId.ToString();
			}

			return String.Format(sql, GetTableName(table), top);
		}



		/// <summary>
		/// Reutrns the query for comments no relations envolved (ex: guest book), 
		/// essential for use with lw.PagingDataSource or lw.DataSource
		/// </summary>
		/// <param name="table">table name</param>
		/// <param name="parentId">parent id</param>
		/// <param name="top">the number of returned comments defaults to 100% (all)</param>
		/// <returns></returns>
		public string GetCommentsQueryNoRelations(string table, int parentId, string top)
		{
			string sql = "select top {2} * from {0}  WITH (NOLOCK) where ParentId={1}";
			return String.Format(sql, GetTableName(table), parentId, top);
		}

		/// <summary>
		/// returns the top one comment
		/// </summary>
		/// <param name="table"></param>
		/// <param name="relationId"></param>
		/// <param name="parentId"></param>
		/// <returns></returns>
		public DataRow GetTopComment(string table, int? relationId, int parentId)
		{
			return GetTopComment(table, relationId, parentId, null);
		}

		/// <summary>
		/// returns the top one comment with a condition
		/// </summary>
		/// <param name="table">table name</param>
		/// <param name="relationId">id of the page/news/form... that the comment is related to</param>
		/// <param name="parentId"></param>
		/// <param name="condition"></param>
		/// <returns></returns>
		public DataRow GetTopComment(string table, int? relationId, int parentId, string condition)
		{
			string cond = "";
			if (!String.IsNullOrWhiteSpace(condition))
				cond = condition;

			string sql = "select top 1 * from {0}  WITH (NOLOCK) where ParentId={1} and RelationId={2} and Status&2=2 {3}";
			sql = String.Format(sql, GetTableName(table), parentId, relationId, cond);
			DataTable dt = DBUtils.GetDataSet(sql, cte.lib).Tables[0];
			if (dt.Rows.Count > 0)
				return dt.Rows[0];
			return null;
		}


		/// <summary>
		/// returns a single discussion the top comment + the children
		/// </summary>
		/// <param name="table"></param>
		/// <param name="CommentId"></param>
		/// <returns></returns>
		public DataTable GetDiscussion(string table, int CommentId)
		{
			string sql = @"select m.MemberId, m.GeuID, m.UserName, m.Status, m.FirstName, m.LastName, m.FirstName + ' ' + m.LastName as Name, m.Online, m.Picture, m.Privacy,
c.CommentId, C.ParentId, c.RelationId, c.DateCreated, c.DateModified, c.Status, c.AdminId,
c.Title,
c.CommentText, c.CommentType
From Members m WITH (NOLOCK)
Inner Join  {0} C WITH (NOLOCK)
on m.MemberId = c.MemberId
where (CommentId={1} or ParentId={1}) and c.Status&2=2";
			sql = String.Format(sql, GetTableName(table), CommentId);
			DataTable dt = DBUtils.GetDataSet(sql, cte.lib).Tables[0];
			return dt;
		}


		/// <summary>
		/// returne the database table name
		/// </summary>
		/// <param name="table">any table</param>
		/// <returns></returns>
		string GetTableName(string table)
		{
			return "Comments_" + table;
		}

		/// <summary>
		/// Returns the number of comments related to a specific RelationId
		/// </summary>
		/// <param name="RelationId">the id of the related parent</param>
		/// <param name="TableName">Name of the table holding the comments</param>
		/// <returns></returns>
		public int GetCommentsCount(int RelationId, string TableName)
		{
			string sql = "Select Count(*) From {0} Where RelationId = {1}";
			int count = Int32.Parse(DBUtils.GetDataSet(string.Format(sql, GetTableName(TableName), RelationId), lw.Comments.cte.lib).Tables[0].Rows[0][0].ToString());
			
			return count;
		}

		/// <summary>
		/// Create and returns the associated comment folder
		/// used to store related files ex: videos, pictures, mp3 files...
		/// </summary>
		/// <param name="TableName">Table Name</param>
		/// <param name="CommentId">Comment Id</param>
		public string CommentFolder(string TableName, int CommentId)
		{
			string ret = Path.Combine("~", Folders.CommentsFolder, TableName, CommentId.ToString());
			string path = WebContext.Server.MapPath(ret);

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			return ret;
		}
		/// <summary>
		/// Delete all comments related to a specific RelationId
		/// </summary>
		/// <param name="relationId">RelationId</param>
		/// <param name="tableName">TableName to delete from</param>
		public void deleteComments(int relationId, string tableName)
		{
			try
			{
				string sql = "Delete From Comments_{1} Where RelationId = {0}";
				DBUtils.ExecuteQuery(string.Format(sql, relationId, tableName), lw.Comments.cte.lib);
			}
			catch (Exception e)
			{

			}
		}

		/// <summary>
		/// Delete single comment in specific table
		/// </summary>
		/// <param name="commentId"></param>
		/// <param name="tableName"></param>
		public void deleteComment(int commentId, string tableName)
		{
			try
			{
				string sql = "Delete from Comments_{1} where CommentId = {0}";
				DBUtils.ExecuteQuery(string.Format(sql, commentId, tableName), lw.Comments.cte.lib);
			}
			catch (Exception e)
			{

			}
		}

        /// <summary>
        /// delete a comment/discussion
        /// </summary>
        /// <param name="CommentId"></param>
        /// <returns></returns>
        public void deleteComment(int commentId)
        {
            try
            {
                string sql = "Delete from Comments_Groups where CommentId = " + commentId;
                //return String.Format(sql, GetTableName("Comments_Groups"));
                dataContext.ExecuteCommand(sql);
            }
            catch (Exception e)
            {

            }
        }

		/// <summary>
		/// Uploads and Adds the uploaded files to the comments' folder
		/// TODO: Add these file name to an xml field inside the DB so they can be used without file access
		/// </summary>
		/// <param name="TableName">Comments Table Name</param>
		/// <param name="CommentId">Comment ID</param>
		/// <returns>List of Added Files</returns>
		public ArrayList AddFiles(string TableName, int CommentId)
		{
			ArrayList ret = new ArrayList();
			string path = CommentFolder(TableName, CommentId);
			for (int i = 0; i < WebContext.Request.Files.Count; i++)
			{
				HttpPostedFile file = WebContext.Request.Files[i];
				if (file.ContentLength > 0)
				{
					string temp = Path.Combine(path, Path.GetFileName(file.FileName));
					try
					{
						file.SaveAs(WebContext.Server.MapPath(temp));
						ret.Add(temp);
					}
					catch (Exception ex)
					{
						ErrorContext.Add("comment-files", ex);
					}
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns list of files associated with the comment
		/// </summary>
		/// <param name="TableName"></param>
		/// <param name="CommentId"></param>
		/// <returns></returns>
		public ArrayList GetCommentFiles(string TableName, int CommentId)
		{
			string path = CommentFolder(TableName, CommentId);
			DirectoryInfo dir = new DirectoryInfo(WebContext.Server.MapPath(path));
			FileInfo[] files = dir.GetFiles();
			ArrayList ret = new ArrayList();

			string cPath = CommentFolder(TableName, CommentId);

			foreach (FileInfo fi in files)
			{
				ret.Add(Path.Combine(cPath, fi.Name));
			}

			return ret;
		}

		#region Variables


		public CommentsDataContext DataContext
		{
			get
			{
				if (_dataContext == null)
					_dataContext = new CommentsDataContext(Connection);
				return (CommentsDataContext)_dataContext;
			}
		}

		#endregion
	}
}

