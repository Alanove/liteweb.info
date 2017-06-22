using System;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace lw.DataControls
{
	/// <summary>
	/// Displays the count of rows in a specified data source
	/// </summary>
	public class DataCount : System.Web.UI.WebControls.Literal
	{
		string source = "";
		bool _bound = false;

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
				this.Text = string.Format(format, dataSrc.RowsCount);
			}
			base.DataBind();
		}


		string format = "{0}";
		public string Format
		{
			get
			{
				return format;
			}
			set
			{
				format = value;
			}
		}

		/// <summary>
		/// The ID of the related <paramref name="CustomDataSource"/>
		/// </summary>
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
