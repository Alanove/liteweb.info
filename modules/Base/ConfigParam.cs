
using lw.WebTools;

namespace lw.Base
{

	/// <summary>
	/// returns the virtual directory root
	/// muyst be used instead of "~" as ~ sometimes returns the virtual directory path instead of the absolute path.
	/// </summary>
	public class ConfigParam : System.Web.UI.WebControls.Literal
	{
		public override void DataBind()
		{
			Config conf = new Config();
			this.Text = lw.Utils.StringUtils.AddSup(conf.GetKey(Key));

			base.DataBind();
		}
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			base.Render(writer);
		}

		string key = "";
		public string Key
		{
			get
			{
				return key;
			}
			set
			{
				key = value;
			}
		}
	}
}
