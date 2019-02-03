using System.Data;
using System.Data.SqlClient;

namespace lw.Data
{

	public class DataSource : lw.Data.IDataSource
	{
		string selectCommand = "";
		string connectionString = "";
		string dataLibrary = "";
		int rowsCount = 0;
		string orderBy;
		bool _conninit = false;
		SqlConnection connection;


		public DataSource()
		{
		}
		public DataSource(string sql, string orderBy, string library)
		{
			this.selectCommand = sql;
			this.orderBy = orderBy;
			this.dataLibrary = library;
		}

		#region Methods
		public object GetData()
		{
			SqlCommand cmd = new SqlCommand();

			cmd.CommandText = selectCommand;
			cmd.Connection = Connection;

			string dataKey = selectCommand.ToLower().Replace(" ", "");

			/*
			try
			{
				if (WebContext.ExecutionContext[dataKey] != null)
				{
					return (DataTable)WebContext.ExecutionContext[dataKey];
				}
			}
			catch(Exception Ex)
			{

			}
			 * 
			 * */

			SqlDataAdapter adp = new SqlDataAdapter(cmd);
			DataSet ds = new DataSet();

			adp.Fill(ds);

			rowsCount = (int)ds.Tables[0].Rows.Count;
			/*
			try
			{
				WebContext.ExecutionContext.Add(dataKey, ds.Tables[0]);
			}
			catch
			{
			}*/
			return ds.Tables[0];
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

		public object Data
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		#endregion
	}
}
