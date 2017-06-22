using System.Data;
using System.Web.UI;
using lw.CTE;
using lw.WebTools;

namespace lw.Members.Controls
{
	public class ProfileLink : System.Web.UI.HtmlControls.HtmlAnchor
	{
		bool bound = false;

		public ProfileLink()
		{
			this.HRef = "#ballout";
		}
		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			this.HRef = "";

			DataRow memberRow = DataBinder.Eval(this.NamingContainer, "DataItem") as DataRow;
			if (memberRow == null)
			{
				DataRowView drv = DataBinder.Eval(this.NamingContainer, "DataItem") as DataRowView;
				if (drv == null)
				{
					this.HRef = "";
					return;
				}
				memberRow = drv.Row;
			}

			int privacy = (int)memberRow["Privacy"];
			int MemberId = (int)memberRow["MemberId"];

			PrivacySettingsManager psMgr = new PrivacySettingsManager();

			if(psMgr.CanAccess(memberRow, "VisitorsAccess", this))
			{
				Config cfg = new Config();

				this.HRef = string.Format("{0}/{1}/{2}",
					WebContext.Root,
					cfg.GetKey(MembersSettings.ProfilesDirectory),
					memberRow["UserName"]);
				this.Title = string.Format("{0}", memberRow["Name"]);
			}
			base.DataBind();
		}

		protected override void RenderBeginTag(HtmlTextWriter writer)
		{
			if (this.HRef == "")
			{
				writer.Write("<span class=\"" + this.Attributes["class"] + "\">");
				return;
			}
			writer.Write("<a href=\"" + this.HRef + "\" title=\"" + this.Title + "\" class=\"" + this.Attributes["class"] + "\">");
			//base.RenderBeginTag(writer);
		}
		protected override void RenderEndTag(HtmlTextWriter writer)
		{
			if (this.HRef == "")
			{
				writer.Write("</span>");
				return;
			}
			writer.Write("</a>");
			//base.RenderEndTag(writer);
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);
		}
	}
}
