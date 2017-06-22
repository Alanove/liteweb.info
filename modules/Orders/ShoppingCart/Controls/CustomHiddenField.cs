

namespace lw.ShoppingCart.Controls
{
	public class CustomHiddenField : System.Web.UI.HtmlControls.HtmlInputHidden
	{
		string _name = "";
		public CustomHiddenField(string name, string value)
		{
			this._name = name;
			this.Value = value;
		}
		public override string Name
		{
			get
			{
				return _name;
			}
			set
			{
				base.Name = value;
			}
		}
	}
}
