using System;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace lw.DataControls
{
	public class CustomDataList : DataList
	{
		string source = "";
		bool _bound = false;
		bool _hasData = false;

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;


			CustomDataSource dataSrc = null;


			if (!String.IsNullOrWhiteSpace(source))
			{
				Control ctrl = Page.FindControl(source);
				if (ctrl != null)
					dataSrc = ctrl as CustomDataSource;
			}
			else
			{
				dataSrc = this.Parent as CustomDataSource;
			}

			if (dataSrc != null)
			{
				DataSource = dataSrc.Data;
				_hasData = dataSrc.HasData;
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

		public bool HasData
		{
			get
			{
				return _hasData;
			}
		}
	}
}
