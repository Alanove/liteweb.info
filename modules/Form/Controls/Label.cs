using System.Web.UI;

namespace lw.Forms.Controls
{
	[ParseChildren(ChildrenAsProperties = false)]
	public class Label : System.Web.UI.WebControls.WebControl
	{
		string _for = "";
		bool bound = false;

		public Label()
			: base("label")
		{
		}

		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			Control ctrl = this.Parent.FindControl(_for);

			if (ctrl != null)
				this.Attributes["for"] = ctrl.ClientID;

			base.DataBind();
		}

		public string For
		{
			get
			{
				return _for;
			}
			set
			{
				_for = value;
			}
		}
	}

	[ParseChildren(ChildrenAsProperties = false)]
	public class ServerContainer : System.Web.UI.WebControls.Literal
	{
		string _for = "";
		bool bound = false;
		string _value = "";

		public ServerContainer()
		{
		}

		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			Control ctrl = this.Parent.FindControl(_for);

			if (ctrl != null)
			{
				string[] parents = ctrl.ClientID.Split('_');
				for (int i = 0; i < parents.Length - 1; i++)
					_value += string.Format("{0}_", parents[i]);
			}
			this.Text = _value;

			base.DataBind();
		}

		public string For
		{
			get
			{
				return _for;
			}
			set
			{
				_for = value;
			}
		}
	}
}
