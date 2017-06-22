using System.Web.UI;
using System.Web.UI.WebControls;


namespace lw.Members.Controls
{
	public class Login : System.Web.UI.WebControls.Login
	{
		protected override void Render(HtmlTextWriter writer)
		{
			WebControl div = new WebControl(HtmlTextWriterTag.Div);

			LayoutTemplate.InstantiateIn(div);

			Controls.Clear();
			Controls.Add(div);

			div.CopyBaseAttributes(this);
			div.CssClass = this.CssClass;
			div.RenderControl(writer);
		}
	}
}
