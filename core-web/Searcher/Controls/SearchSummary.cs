using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using lw.Data;
using lw.DataControls;

namespace lw.Searcher.Controls
{
	/// <summary>
	/// Displays the count of rows in a specified data source
	/// </summary>
	public class SearchSummary : System.Web.UI.WebControls.Literal
	{
		string source = "";
		bool _bound = false;

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;


			SearchResultsDataSource dataSrc = null;

			if (!String.IsNullOrWhiteSpace(source))
			{
				Control ctrl = Page.FindControl(source);
				if (ctrl != null)
					dataSrc = ctrl as SearchResultsDataSource;
			}
			else
			{
				dataSrc = this.Parent as SearchResultsDataSource;
			}

			if (dataSrc != null)
			{
				switch (dataSrc.RowsCount)
				{
					case 0:
						this.Text = String.Format(NoResultsFormat, dataSrc.Query);
						break;
					case 1:
						this.Text = String.Format(OneResultsFormat, dataSrc.Query);
						break;
					default:
						this.Text = String.Format(MultipleResultsFormat, dataSrc.Query, dataSrc.RowsCount);
						break;
				}
			}
			
			base.DataBind();
		}


		string _NoResultsFormat = "{0}";
		public string NoResultsFormat
		{
			get
			{
				return _NoResultsFormat;
			}
			set
			{
				_NoResultsFormat = value;
			}
		}

		string _OneResultsFormat = "{0}";
		public string OneResultsFormat
		{
			get
			{
				return _OneResultsFormat;
			}
			set
			{
				_OneResultsFormat = value;
			}
		}

		string _MultipleResultsFormat = "{0}";
		public string MultipleResultsFormat
		{
			get
			{
				return _MultipleResultsFormat;
			}
			set
			{
				_MultipleResultsFormat = value;
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
