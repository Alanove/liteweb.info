

using lw.WebTools;

namespace lw.Members.Controls
{
	public class ProfileProperty : System.Web.UI.WebControls.Literal
	{
		string _property = "";
		string _format = "{0}";
		bool _bound = false;
		
		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			this.Text = string.Format(_format, WebContext.Profile.GetPropertyValue(Property));
			base.DataBind();
		}

		public string Property
		{
			get { return _property; }
			set { _property = value; }
		}
		public string Format
		{
			get { return _format; }
			set { _format = value; }
		}
	}
}
