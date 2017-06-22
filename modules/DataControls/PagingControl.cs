using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;


using lw.WebTools;

namespace lw.DataControls
{
	public class PagingControl : Repeater
	{
		int pageSize = 10;
		string source = "";
		int padding = 10;
		int _currentPage = 1;
		int halfPadding;
		bool _render = false;
		bool _bound = false;
		string _previousText = "&laquo; Previous &laquo;";
        string _nextText = "&raquo; Next &raquo;";
        string _previousClass = "paging-previous";
        string _nextClass = "paging-next";

		public PagingControl()
		{
			halfPadding = padding / 2;
		}

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
				DataSource = BuildPages(dataSrc);
			}


			base.DataBind();
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (this.Items.Count > 1)
				base.Render(writer);
		}

		object BuildPages(CustomDataSource src)
		{
			object data = src.Data;

			decimal _decPageCount = (decimal)src.RowsCount / (decimal)src.PageSize;
			int PageCount = src.RowsCount / src.PageSize;
			if (_decPageCount > PageCount)
				PageCount++;

			pageSize = src.PageSize;

			bool hasNext = false;
			bool hasPrevious = false;

			if (CurrentPage > 1 && PageCount > 1)
				hasPrevious = true;

			if (CurrentPage < PageCount)
				hasNext = true;

			int _start = CurrentPage - halfPadding;

			if (_start < 1)
				_start = 1;

			int _end = _start + halfPadding * 2 - 1;

			if (_start == 2)
				_start = 1;

			if (_end > PageCount)
				_end = PageCount;

			if (_end + 1 == PageCount)
				_end = PageCount;

			if (_end == PageCount)
				_start = _end - 2 * halfPadding > 1 ? _end - 2 * halfPadding : 1;

			DataTable dt = new DataTable();
			dt.Columns.Add("Page");
			dt.Columns.Add("Class");
			dt.Columns.Add("Text");
			dt.Columns.Add("PageParam");
			dt.Columns.Add("PageSizeParam");
			dt.Columns.Add("PageSize");
			dt.AcceptChanges();

			DataRow dr = dt.NewRow();

			if (hasPrevious)
			{
				dr["Page"] = _currentPage - 1;
				dr["Text"] = _previousText;
                dr["Class"] = _previousClass;
				dt.Rows.Add(dr);
			}

			if (_start > 3 && PageCount > padding + 3)
			{
				dr = dt.NewRow();
				dr["Page"] = 1;
				dr["Text"] = 1;
				dr["Class"] = "paging-first";
				dt.Rows.Add(dr);

				dr = dt.NewRow();
				dr["Page"] = _currentPage - padding + 1;
				dr["Text"] = "...";
				dr["Class"] = "paging-neutre";
				dt.Rows.Add(dr);
			}
			else
				_start = 1;

			bool AddEnd = false;
			if (_end + 3 < PageCount)
				AddEnd = true;
			else
				_end = PageCount;

			int i;
			for (i = _start; i <= _end; i++)
			{
				dr = null;
				dr = dt.NewRow();

				if (i == _currentPage)
					dr["Class"] = "paging-current";

				dr["Page"] = i;
				dr["Text"] = i;
				dt.Rows.Add(dr);
			}

			if (AddEnd)
			{
				dr = dt.NewRow();
				dr["Page"] = _currentPage + padding - 1;
				dr["Text"] = "...";
				dr["Class"] = "paging-neutre";
				dt.Rows.Add(dr);

				dr = dt.NewRow();
				dr["Page"] = PageCount;
				dr["Text"] = PageCount;
				dr["Class"] = "paging-last";
				dt.Rows.Add(dr);

			}

			if (hasNext)
			{
				dr = dt.NewRow();
				dr["Page"] = _currentPage + 1;
				dr["Text"] = _nextText;
                dr["Class"] = _nextClass;
				dt.Rows.Add(dr);
			}

			foreach (DataRow row in dt.Rows)
			{
				row["PageParam"] = lw.CTE.DataCte.PagingParam;
				row["PageSizeParam"] = lw.CTE.DataCte.PagingSizeParam;
				row["PageSize"] = pageSize;
			}
			dt.AcceptChanges();
			return dt;
		}

		int CurrentPage
		{
			get
			{
				if (_currentPage == 1)
				{
					string _Page = WebContext.Request.QueryString[lw.CTE.DataCte.PagingParam];

					if (!String.IsNullOrWhiteSpace(_Page))
					{
						try
						{
							_currentPage = Int32.Parse(_Page);
						}
						catch
						{
						}
					}
				}
				return _currentPage;
			}
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

		public string PreviousText
		{
			get { return _previousText; }
			set { _previousText = value; }
		}
		public string NextText
		{
			get { return _nextText; }
			set { _nextText = value; }
		}


        public string PreviousClass
        {
            get { return _previousClass; }
            set { _previousClass = value; }
        }
        public string NextClass
        {
            get { return _nextClass; }
            set { _nextClass = value; }
        }
	}

}
