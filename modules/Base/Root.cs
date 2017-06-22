
using lw.WebTools;

namespace lw.Base
{

	/// <summary>
	/// returns the virtual directory root
	/// muyst be used instead of "~" as ~ sometimes returns the virtual directory path instead of the absolute path.
	/// </summary>
	public class Root : System.Web.UI.WebControls.Literal
	{
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write(CurrentRoot);
			base.Render(writer);
		}

		public string CurrentRoot
		{
			get
			{
				return WebContext.Root;
			}
		}
	}
	public class ManagerRoot : System.Web.UI.WebControls.Literal
	{
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write(WebContext.ManagerRoot);
			base.Render(writer);
		}
	}
}
