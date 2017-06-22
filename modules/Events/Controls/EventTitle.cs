using lw.Base;
using lw.DataControls;
using lw.WebTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace lw.Events.Controls
{
	public class EventTitle :DataProperty
	{
		HtmlAnchor wrapper = null;
		public override void DataBind()
		{
			this.Property = "Title";
			base.DataBind();

			if (MyPage.Editable && ImEditable)
			{
				wrapper = new HtmlAnchor();

				wrapper.Attributes["data-editable"] = "true";
				wrapper.Attributes["data-id"] = ControlUtils.GetBoundedDataField(this.NamingContainer, "Id").ToString();
				wrapper.Attributes["data-type"] = "event";
			}

		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (wrapper != null)
			{
				writer.Write(string.Format("<a href=#edit-event data-editable=true data-id={0} data-type=event></a>", wrapper.Attributes["data-id"]));
			}
			base.Render(writer);
		}


		CustomPage myPage = null;
		CustomPage MyPage
		{
			get
			{
				if (myPage == null)
				{
					myPage = this.Page as CustomPage;
				}
				return myPage;
			}
		}
	}
}
