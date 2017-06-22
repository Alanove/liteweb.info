using System.Web.UI;
using lw.CTE;
using lw.WebTools;

namespace lw.Content.Controls
{
	public class Warning : Message
	{
		public Warning()
		{
			this.Tag = "div";
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (WarningContext.Count == 0)
				return;

			if (this.Attributes["class"] == null)
				this.Attributes.Add("class", CssClasses.Warning);

			this.Text = WarningContext.GetAll(Format, Seperator, KeyFilter);
			base.Render(writer);
		}
	}
}
