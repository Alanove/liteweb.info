using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using lw.Utils;

namespace lw.Members.Controls
{
	public class MembersPrivacyLock : WebControl
	{
		string _for = "";


		public override void RenderBeginTag(System.Web.UI.HtmlTextWriter writer)
		{
			//base.RenderBeginTag(writer);
		}

		public override void RenderEndTag(System.Web.UI.HtmlTextWriter writer)
		{
			//base.RenderEndTag(writer);
		}


		public override void DataBind()
		{
			if (string.IsNullOrWhiteSpace(For))
			{
				For = DataBinder.Eval(NamingContainer, "DataItem.Property").ToString();
			}

			base.DataBind();
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			DataRow member = lw.Members.Security.User.LoggedInUser(this);

			int privacy = (int)member["Privacy"];
			int test = 0;

			PrivacySettingsManager psMgr = new PrivacySettingsManager();

			DataView view = new DataView(psMgr.Table, "Property='" + For + "'", "", DataViewRowState.CurrentRows);

			if(view.Count > 0)
				test = (int)view[0]["Value"];

			writer.Write("<div class=\"privacy-lock\">");
		//	writer.Write("<a href=\"#privacy\">Privacy</a>");
			writer.Write("<ul>");

			foreach (PrivacyOptions option in Enum.GetValues(typeof(PrivacyOptions)))
			{
				writer.Write(string.Format("<li><input type=\"radio\" name=\"privacy_{2}\" value=\"{0}\" id=\"{2}_{0}\" {3}/>"+
					"<label for=\"{2}_{0}\">{1}</label></li>", 
						option.ToString(), 
						EnumHelper.GetDescription(option),
						For,
						IsSelected(option, test, privacy)? " checked": ""
					));
			}
			writer.Write("</ul>");
			writer.Write("</div>");

			base.Render(writer);
		}

		bool IsSelected(PrivacyOptions option, int param, int privacy)
		{
			//0 = all settings are open for everyone
			//
			//if (option == PrivacyOptions.Everyone && privacy == 0)
				//return true;

			//for only me we multiply by 2
			//initialized as only everyone
			int test = 0;
			if (option == PrivacyOptions.OnlyMe)
			{
				test = (int)Math.Pow(2, param * 2);
			}
			else if (option == PrivacyOptions.Friends)
			{
				test = (int)Math.Pow(2, param);
			}

			return (privacy & test) == test;
		}


		#region properties

		public string For
		{
			get
			{
				return _for;
			}
			set
			{
				_for = value;
			}
		}
		#endregion
	}
}
