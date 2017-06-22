using System.Data;
using System.Data.SqlClient;

using lw.CTE;
using lw.WebTools;


namespace lw.Data
{
	public class LibraryMgr
	{
		string _connectionStr;
		public IDbConnection Connection;
		public IDbTransaction Transaction;
		

		public LibraryMgr()
		{
		}
	
		
		public static IDbConnection GetConnection(string connectionStrType)
		{
			IDbConnection conn;
			switch(connectionStrType)
			{
				case GVC.ConnectionSQL_AppE :
					conn = new SqlConnection(Config.GetConnectionString(connectionStrType));
					break;
				case GVC.ConnectionAccess_AppE :
					conn = new System.Data.OleDb.OleDbConnection(Config.GetConnectionString(connectionStrType));
					break;
				default:
					conn = new SqlConnection(Config.GetConnectionString(connectionStrType));
					break;
			}
			return conn;
		}



		public void SetAdapterConnection(IDataAdapter adapter, string connectionStrType)
		{
			IDbConnection conn = GetConnection(connectionStrType);
			switch(connectionStrType)
			{
				case GVC.ConnectionSQL_AppE: this.SetAdapterConnection((System.Data.SqlClient.SqlDataAdapter)adapter);
					break;
				case GVC.ConnectionAccess_AppE: this.SetAdapterConnection((System.Data.OleDb.OleDbDataAdapter)adapter);
					break;
				default: this.SetAdapterConnection((System.Data.SqlClient.SqlDataAdapter)adapter);
					break;
			}
		}

		public void SetAdapterConnection(System.Data.SqlClient.SqlDataAdapter adapter)
		{
			if(adapter.SelectCommand!=null)
				this.SetCommandConnection(adapter.SelectCommand);
			if(adapter.UpdateCommand!=null)
				this.SetCommandConnection(adapter.UpdateCommand);
			if(adapter.InsertCommand!=null)
				this.SetCommandConnection(adapter.InsertCommand);
			if(adapter.DeleteCommand!=null)
				this.SetCommandConnection(adapter.DeleteCommand);
		}

		public void SetAdapterConnection(System.Data.OleDb.OleDbDataAdapter adapter)
		{
			if(adapter.SelectCommand!=null)
				this.SetCommandConnection(adapter.SelectCommand);
			if(adapter.UpdateCommand!=null)
				this.SetCommandConnection(adapter.UpdateCommand);
			if(adapter.InsertCommand!=null)	
				this.SetCommandConnection(adapter.InsertCommand);
			if(adapter.DeleteCommand!=null)
				this.SetCommandConnection(adapter.DeleteCommand);
		}
        
		public void SetCommandConnection(IDbCommand command)
		{
			command.Connection = Connection;
			if(Transaction!=null)
			{
				command.Transaction = Transaction;
			}
		}

		public static IDbTransaction BeginTransaction(IDbConnection connection)
		{
			if(connection.State == ConnectionState.Closed)
			{
				connection.Open();
			}
			return connection.BeginTransaction();
		}
		public static void CommitTransaction(IDbTransaction trans)
		{
			trans.Commit();
		}
		public static void RollbackTransaction(IDbTransaction trans)
		{
			trans.Rollback();
			trans.Connection.Close();
		}
	}
}
