using System;
using System.Web.UI.HtmlControls;
using lw.Comments;
using lw.Forms.Controls;
using lw.WebTools;
using System.Web.UI;
using System.Collections.Specialized;


namespace lw.Groups.Controls
{
	public class GroupCommentsForm : Form
	{
		GroupCommentsDataSource dataSrc = null;
		bool _bound = false;
		CommentsManager _cMgr;

		public GroupCommentsForm()
		{
			_cMgr = new CommentsManager();
		}

		GroupCommentsDataSource _getParent(Control el)
		{
			GroupCommentsDataSource ret = el.Parent as GroupCommentsDataSource;
			if (ret != null)
				return ret;

			if (el.Parent != null)
				return _getParent(el.Parent);

			return null;
		}

		public override void DataBind()
		{
			dataSrc = _getParent(this);

			if (dataSrc == null)
				this.Visible = false;

			if (_bound || !this.Visible)
				return;
			_bound = true;

			if (!dataSrc.HasWritePermission)
			{
				this.Visible = false;
				return;
			}

			HtmlInputHidden RelationId = new HtmlInputHidden();
			RelationId.ID = "RelateToId";
			RelationId.Value = dataSrc.RelationId.ToString();
			this.Controls.Add(RelationId);


			
			HtmlInputHidden ParentId = new HtmlInputHidden();
			ParentId.ID = "ParentId";
			ParentId.Value = dataSrc.ParentId.ToString();	
			this.Controls.Add(ParentId);


			if (this.IsPostBack && this.GetValue(ParentId) == dataSrc.ParentId.ToString() 
				&& this.GetValue(RelationId) == dataSrc.RelationId.ToString())
			{
				NameValueCollection values = this.GetValues();
				string text = values["Comment"];
				if (!String.IsNullOrWhiteSpace(text))
				{
					_cMgr.AddMemberComment(cte.CommentsTable,
						dataSrc.ParentId, dataSrc.ParentId > 0 ? -1 : dataSrc.RelationId,
						values["Subject"],
						text, WebContext.Profile.UserId, CommentType.Text);

					if (this.IsAjax)
					{
						AjaxResponse resp = new AjaxResponse();
						resp.data = string.Format(@"{{
UserName: ""{0}"",
Name: ""{1}"",
Picture: 
				
						}}", "");
					}
					else
						WebContext.Response.Redirect(WebContext.Request.RawUrl);
				}
			}


			base.DataBind();
		}
	}
}
