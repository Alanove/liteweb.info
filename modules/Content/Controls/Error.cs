using System.Web.UI;
using lw.CTE;
using lw.WebTools;

namespace lw.Content.Controls
{
	public class Error : Message
	{
		public Error()
		{
			this.Tag = "div";
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (ErrorContext.Count == 0)
				return;

			if (this.Attributes["class"] == null)
				this.Attributes.Add("class", CssClasses.Error);

			this.Text = ErrorContext.GetAll(Format, Seperator, KeyFilter);
			base.Render(writer);
		}
	}
}
