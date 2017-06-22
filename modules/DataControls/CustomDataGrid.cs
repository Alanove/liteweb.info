using System.Web.UI;
using System.Web.UI.WebControls;
using lw.WebTools;


namespace lw.DataControls
{
	public class CustomDataGrid : DataGrid
	{
		string source = "";
		bool _bound = false;

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;
			if (source != "")
			{
				Control ctrl = this.FindControlRecursive(source);
				if (ctrl != null)
				{
					CustomDataSource dataSrc = ctrl as CustomDataSource;
					if (dataSrc != null)
					{
						DataSource = dataSrc.Data;
					}
				}
			}
			base.DataBind();
		}

		public string Source
		{
			get
			{
				return Source;
			}
			set
			{
				source = value;
			}
		}
	}
}
