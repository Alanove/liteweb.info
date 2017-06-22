using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;


using lw.Data;
using lw.Utils;
using lw.WebTools;
using lw.Comments;
using lw.CTE.Enum;
using lw.DataControls;

namespace lw.Comments.Controls
{
	public class CommentsDataSource : CustomDataSource
	{
		int _relationId = -1;
		int _parentId = -1;
		string _tableName = "";
		public bool _bound = false;
		bool _noRelations = false;
		string _relateTo = "";
		bool _childComments = false;
		string top = "100 PERCENT";
		bool _membersOnly = false;
		CommentsManager cMgr;
		int CommentId = -2;

		public CommentsDataSource()
		{
			this.DataLibrary = cte.lib;
			OrderBy = "CommentId Desc";

			cMgr = new CommentsManager();
		}

		public override void DataBind()
		{
			if (!_bound)
			{
				_bound = true;

				lw.Base.CustomPage page = this.Page as lw.Base.CustomPage;

				string GroupName = page.GetQueryValue("CommentTitle");

				if (!StringUtils.IsNullOrWhiteSpace(GroupName))
				{
					String[] Split = GroupName.Split('-');
					CommentId = Int32.Parse(Split[Split.Length - 1]);
				}

				if (!NoRelations)
				{
					Comments.Comments_Tables_View table = cMgr.GetTableDetails(TableName);
					if (table == null)
						return;

					object relationValue = ControlUtils.GetBoundedDataField(this.NamingContainer, table.RelationField);
					if (MembersOnly)
						this.SelectCommand = cMgr.GetMemberCommentsQueryWithRelations(TableName, (int)relationValue, -1, Top);
					else
					this.SelectCommand = cMgr.GetCommentsQueryWithRelations(TableName, (int)relationValue, -1, Top);
				}
				else
				{
					if (MembersOnly)
					{

						if (ChildComments)
						{
							object obj = DataBinder.Eval(this.NamingContainer, "DataItem.CommentId");

							if (obj != null)
							{
								ParentId = (int)obj;
							}
						}

						this.SelectCommand = cMgr.GetMemberCommentsQueryWithNoRelations(TableName, ParentId, Top);
						this.SelectCommand += string.Format(" And C.Status&{0}={0}", (int)Status.Enabled);

						if (ChildComments)
						{
							this.SelectCommand += string.Format(" And C.CommentId>{0} Order by C.CommentId ASC", 0);
						}
						else
						{
							if (CommentId != -2)
							{
								this.SelectCommand += string.Format("And (CommentId={0} or ParentId={0})", CommentId);
							}
						}
					}
					else
						this.SelectCommand = cMgr.GetCommentsQueryNoRelations(TableName, -1, Top);
				}
				//TODO: Display Disabled Comments in case an administrator is logged in
				if (!MembersOnly)
				{
					//this.SelectCommand += String.Format(" And Status&{0}={0}", (int)Status.Enabled);

					if (!EnablePaging)
					{
						this.SelectCommand += " Order By DateCreated DESC";
					}
				}

			}
			base.DataBind();
		}


		public string TableName
		{
			get { return _tableName; }
			set { _tableName = value; }
		}
		public int RelationId
		{
			get { return _relationId; }
			set { _relationId = value; }
		}
		public int ParentId
		{
			get
			{
				return _parentId;
			}
			set
			{
				_parentId = value;
			}
		}
		public bool NoRelations
		{
			get { return _noRelations; }
			set { _noRelations = value; }
		}
		public string RelateTo
		{
			get { return _relateTo; }
			set { _relateTo = value; }
		}
		public string Top
		{
			get { return top; }
			set { top = value; }
		}
		public bool ChildComments
		{
			get
			{
				return _childComments;
			}
			set
			{
				_childComments = value;
			}
		}
		public bool MembersOnly
		{
			get { return _membersOnly; }
			set { _membersOnly = value; }
		}
	}
}

