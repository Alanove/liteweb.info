using lw.Data;
using lw.DataControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace lw.Pages.Controls
{
	/// <summary>
	/// Datasource that gets the related pages of a specific page by PageId
	/// </summary>
	public class RelatedPagesDataSource : CustomDataSource
	{
		bool _bound = false;


		public RelatedPagesDataSource()
		{
			this.DataLibrary = cte.lib;
		}


		DataTable _results = null;
		/// <summary>
		/// Generates the data results by calling the related stored procedure
		/// </summary>
		DataTable Results
		{
			get
			{
				if (_results == null)
				{
					var relatedPagesProcedure = DBUtils.StoredProcedure("[RelatedPages]", cte.lib);
					DBUtils.AddCommandParameter(relatedPagesProcedure, "@PageId", SqlDbType.Int, PageId, ParameterDirection.Input);
					DBUtils.AddCommandParameter(relatedPagesProcedure, "@Max", SqlDbType.Int, Max, ParameterDirection.Input);

					SqlDataAdapter adp = new SqlDataAdapter();
					var ds = new DataSet();
					adp.SelectCommand = relatedPagesProcedure;

					adp.Fill(ds);

					_results = ds.Tables[0];
				}
				return _results;
			}
		}

		/// <summary>
		/// Gets or sets the data related to the data source
		/// </summary>
		public override object Data
		{
			get
			{
				return Results;
			}
			set
			{
				base.Data = value;
			}
		}

		/// <summary>
		/// Returns the data row count
		/// </summary>
		public override int RowsCount
		{
			get
			{
				return Results.Rows.Count;
			}
		}

		/// <summary>
		/// Returns true if the search returned any results.
		/// </summary>
		public override bool HasData
		{
			get
			{
				return RowsCount > 0;
			}
		}

		int? _max = 20;
		/// <summary>
		/// Gets or sets the maximum number of related pages.
		/// </summary>
		public int? Max
		{
			get
			{
				return _max;
			}
			set
			{
				_max = value;
			}
		}

		int? _pageId = null;
		/// <summary>
		/// Returns or sets PageId of which the class will fetch the related pages
		/// </summary>
		public int? PageId
		{
			get
			{
				if (_pageId == null)
				{
					object obj = DataBinder.Eval(this.NamingContainer, "DataItem.PageId");
					if (obj != null)
						_pageId = (int)obj;
					return _pageId;
				}
				if (_pageId == null)
				{
					object obj = MyPage.GetQueryValue("PageId");
					if (obj != null && !String.IsNullOrEmpty(obj.ToString()))
					{
						string _temp = obj.ToString().Trim();
						if(lw.Utils.Validation.IsInteger(_temp))
							_pageId = Int32.Parse(_temp);
					}
					else
					{
						obj = MyPage.GetQueryValue("Id");
						if (obj != null && !String.IsNullOrEmpty(obj.ToString()))
						{
							string _temp = obj.ToString().Trim();
							if (lw.Utils.Validation.IsInteger(_temp))
								_pageId = Int32.Parse(_temp);
						}
					}
				}
				return _pageId;
			}
			set { _pageId = value; }
		}
	}
}
