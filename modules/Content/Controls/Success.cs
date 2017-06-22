using System.Web.UI;
using lw.CTE;
using lw.WebTools;

namespace lw.Content.Controls
{
	public class Success : Message
	{
		public Success()
		{
			this.Tag = "div";
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (SuccessContext.Count == 0)
				return;

			if (this.Attributes["class"] == null)
				this.Attributes.Add("class", CssClasses.Success);

			this.Text = SuccessContext.GetAll(Format, Seperator, KeyFilter);
			base.Render(writer);
		}
	}
}
