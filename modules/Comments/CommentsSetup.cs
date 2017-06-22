using System.Data.Linq;

using lw.Data;

namespace lw.Comments
{
	public class CommentsSetup : LINQManager
	{
		/// <summary>
		/// Custom Permissions 2 = AutoApprove
		/// </summary>

		public CommentsSetup()
			: base(cte.lib)
		{
		}


		#region public functions

		public void CreateCommentsTableWithMembers(string tableName, string relationField, int adminId)
		{
			int? ret = null;
			DataContext.CreateCommentsTable_Members(tableName, relationField, adminId, ret);
		}
		public void CreateCommentsTableNoMembers(string tableName, string relationField, int adminId)
		{
			int? ret = null;
			DataContext.CreateCommentsTable_No_Members(tableName, relationField, adminId, ret);
		}

		#endregion

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
