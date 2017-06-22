using System;
using System.Web.UI;
using lw.Comments;
using lw.Comments.Controls;
using lw.CTE.Enum;
using lw.Utils;

namespace lw.Groups.Controls
{
	public class GroupCommentsDataSource : CommentsDataSource
	{
		CommentsManager cMgr;
		bool _hasViewPermission = true;
		bool _hasWritePermission = true;
		bool _hasDeletePermission = false;
		bool _inheritPermission = false;
		int CommentId = -2;

		public GroupCommentsDataSource()
		{
			this.DataLibrary = cte.lib;
			this.RelateTo = "ID";
			this.TableName = cte.CommentsTable;
			OrderBy = "CommentId Desc";

			cMgr = new CommentsManager();

			this.Top = "120";
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			lw.Base.CustomPage page = this.Page as lw.Base.CustomPage;

			string GroupName = page.GetQueryValue("CommentTitle");

			if (!StringUtils.IsNullOrWhiteSpace(GroupName))
			{
				String[] Split = GroupName.Split('-');
				CommentId = Int32.Parse(Split[Split.Length - 1]);
			}


			if(ChildComments)
			{
				object obj = DataBinder.Eval(this.NamingContainer, "DataItem.CommentId");

				if (obj != null)
				{
					ParentId = (int)obj;
				}
			}

			if (ParentId == -1)
			{
				object relationValue = DataBinder.Eval(this.NamingContainer, "DataItem." + cte.CommentsRelateTo);

				if (relationValue != null)
				{
					RelationId = (int)relationValue;
				}
			}
			GenerateSelectCommand(int.Parse(Top), 0);

			this.Visible = HasViewPermission;


			if (InheritPermission)
			{
				GroupCommentsDataSource parentDataSource = null;
				Control parent = this.Parent;
				do
				{
					parentDataSource = parent as GroupCommentsDataSource;
					if (parentDataSource != null)
						break;

					parent = parent.Parent;
				}
				while (parent != null);

				if (parentDataSource != null)
				{
					_hasWritePermission = parentDataSource.HasWritePermission;
					_hasViewPermission = parentDataSource.HasViewPermission;
					_hasDeletePermission = parentDataSource.HasDeletePermission;
				}
			}

			base.DataBind();

			
		}


		protected override void Render(HtmlTextWriter writer)
		{
			

			base.Render(writer);
		}

		public void GenerateSelectCommand(int top, int from)
		{
			this.SelectCommand = cMgr.GetMemberCommentsQueryWithRelations(TableName, RelationId, ParentId, top.ToString());
			this.SelectCommand += string.Format(" And C.Status&{0}={0}", (int)Status.Enabled);

			if (ChildComments)
			{
				this.SelectCommand += string.Format(" And C.CommentId>{0} Order by C.CommentId ASC", from);
			}
			else
			{
				if (from > 0)
				{
					this.SelectCommand += string.Format("And C.CommentId<{0}", from);
				}
				if (CommentId != -2)
				{
					this.SelectCommand += string.Format("And (CommentId={0} or ParentId={0})", CommentId);
				}
				this.SelectCommand += "Order by C.CommentId DESC";
			}
		}


		public bool HasViewPermission
		{
			get
			{
				return _hasViewPermission;
			}
			set
			{
				_hasViewPermission = value;
			}
		}

		public bool HasWritePermission
		{
			get
			{
				return _hasWritePermission;
			}
			set
			{
				_hasWritePermission = value;
			}
		}

		public bool HasDeletePermission
		{
			get
			{
				return _hasDeletePermission;
			}
			set
			{
				_hasDeletePermission = value;
			}
		}

		public bool InheritPermission
		{
			get
			{
				return _inheritPermission;
			}
			set
			{
				_inheritPermission = value;
			}
		}
	}
}