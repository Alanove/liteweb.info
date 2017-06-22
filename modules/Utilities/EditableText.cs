using System.Web.UI.WebControls;

namespace lw.Utilities
{
	public class EditableText:Literal
	{
		string _value = "";
		public EditableText()
		{
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write("<font color=red>" + Value + "</font>");
			this.Text = "";
			base.Render(writer);
		}

		public string Value
		{
			get
			{
				if (Text.Trim() != "")
					_value = Text;
				return _value;
			}
			set
			{
				_value = value;
			}
		}
	}
}
