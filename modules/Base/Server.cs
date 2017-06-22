
using lw.WebTools;

namespace lw.Base
{

	/// <summary>
	/// Returns the server or domain name.
	/// </summary>
	public class Server : System.Web.UI.WebControls.Literal
	{
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write(WebContext.ServerName);
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
}
