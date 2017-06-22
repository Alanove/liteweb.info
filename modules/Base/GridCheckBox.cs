using System.Web.UI;

namespace lw.Base
{
	public class GridCheckBox: System.Web.UI.HtmlControls.HtmlInputCheckBox
	{
		protected string _keyField;

		public GridCheckBox()
		{
		}

		public override void DataBind()
		{
			string id = DataBinder.Eval(this.NamingContainer, "DataItem." + KeyField).ToString();	
			this.ID = id;
			this.Name = id;
			this.Value=id;
			this.Attributes["onclick"] = "GridCClick(this)";
			base.DataBind();
		}

		public string KeyField
		{
			get
			{
				return this._keyField;
			}
			set
			{
				this._keyField = value;
			}
		}

	}
}
