using System;
using System.Data;
using System.Data.SqlClient;

using lw.WebTools;

namespace lw.Data
{
	public class PagingDataSource : IDataSource
	{
		int pageSize = 10;

		string selectCommand = "";
		string connectionString = "";
		string dataLibrary = "";
		int rowsCount = 0;
		int page;
		string orderBy;
		bool _conninit = false;
		SqlConnection connection;


		public PagingDataSource()
		{
		}
		public PagingDataSource(string sql, int page, string orderBy, string library)
		{
			this.selectCommand = sql;
			this.page = page;
			this.orderBy = orderBy;
			this.dataLibrary = library;
		}

		#region Methods
		public object GetData()
		{
			SqlCommand cmd = new SqlCommand();

			cmd.CommandText = lw.CTE.DataCte.PagingProcedure;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection = Connection;

			SqlParameter param = new SqlParameter("@SelectCommand", SqlDbType.NVarChar);
			param.Value = this.selectCommand;
			cmd.Parameters.Add(param);

			param = new SqlParameter("@PageSize", SqlDbType.Int);
			param.Value = pageSize;
			cmd.Parameters.Add(param);

			param = new SqlParameter("@Page", SqlDbType.Int);
			param.Value = page;
			cmd.Parameters.Add(param);

			param = new SqlParameter("@OrderBY", SqlDbType.VarChar);
			param.Value = this.orderBy;
			cmd.Parameters.Add(param);

			SqlDataAdapter adp = new SqlDataAdapter(cmd);
			DataSet ds = new DataSet();

			adp.Fill(ds);

			rowsCount = (int)ds.Tables[0].Rows[0].ItemArray[0];

			return ds.Tables[1];
		}

		#endregion

		#region Get Properties

		public int RowsCount
		{
			get
			{
				return rowsCount;
			}
			set
			{
				rowsCount = value;
			}
		}
		public bool HasData
		{
			get
			{
				return rowsCount > 0;
			}
		}

		#endregion

		#region Properties

		public int PageSize
		{
			get
			{
				object obj = null;
				obj = WebContext.Request[lw.CTE.DataCte.PagingSizeParam];
				if (obj != null && obj.ToString() != "")
					pageSize = Int32.Parse(obj.ToString());

				return pageSize;
			}
			set
			{
				pageSize = value;
			}
		}
		public string SelectCommand
		{
			get
			{
				return selectCommand;
			}
			set
			{
				selectCommand = value;
			}
		}

		public SqlConnection Connection
		{
			get
			{
				if (!_conninit)
				{
					DirectorBase db = new DirectorBase(this.dataLibrary);
					connection = (SqlConnection)db.GetConnection();
					_conninit = true;
				}
				return connection;
			}
			set
			{
				connection = value;
			}
		}
		public string DataLibrary
		{
			get
			{
				return dataLibrary;
			}
			set
			{
				dataLibrary = value;
			}
		}
		public string OrderBy
		{
			get { return orderBy; }
			set { orderBy = value; }
		}


		object _data = null;
		public object Data
		{
			get
			{
				return _data;
			}
			set
			{
				_data = value;
			}
		}
		#endregion
	}
}
