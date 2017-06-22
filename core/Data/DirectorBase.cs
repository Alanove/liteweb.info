using System;
using System.Data;
using System.Data.SqlClient;

using lw.CTE;
using lw.Utils;
using lw.WebTools;

namespace lw.Data
{
	public class DirectorBase
	{
		protected string AssemblyName;
		protected string GenerateAdapterClass;
		protected string connectionStrType;
		protected IDbConnection Connection;
		protected IDbTransaction Transaction;
		
		
		
		protected LibraryMgr lMgr = new LibraryMgr();
        
		public DirectorBase(string GetInitMyLibraryName)
		{
			initLibraryComponent(GetInitMyLibraryName);
			initConnection();
			
		}

		protected  void initLibraryComponent(string GetInitMyLibraryName)
		{
			DataTable GetTable = XmlManager.GetTableFromXmlDataSet(GVD.RootDataBaseXmlFileConfig, GVCL.TableXmlClassLibrary);
			this.connectionStrType = XmlManager.GetValueFromXmlDataTable(GetTable,GVCL.NodeXmlKeyClassLibrary,GVCL.NodeXmlValueClassLibrary,GetInitMyLibraryName);
			this.GenerateAdapterClass=XmlManager.GetValueFromXmlDataTable(GetTable,GVCL.NodeXmlKeyClassLibrary,GVCL.NodeXmlComponentClassName,GetInitMyLibraryName);
			this.AssemblyName=XmlManager.GetValueFromXmlDataTable(GetTable,GVCL.NodeXmlKeyClassLibrary,GVCL.NodeXmlAssemblyName,GetInitMyLibraryName);
			
        }

		protected void initConnection()
		{
			if(this.Connection == null)
			{
				this.Connection = LibraryMgr.GetConnection(this.connectionStrType);
			}
		}
		public void BeginTransaction()
		{
			if(this.Connection != null)
			{
				this.Transaction = LibraryMgr.BeginTransaction(this.Connection);
				lMgr.Transaction = this.Transaction ;
			}
		}

		public void CommitTransaction()
		{
			if(this.Transaction!=null)
			{
				LibraryMgr.CommitTransaction(this.Transaction);
			}
		}

		public void RollbackTransaction()
		{
			if(this.Transaction!=null)
			{
				LibraryMgr.RollbackTransaction(this.Transaction);
			}
		}
		protected IDataAdapter GetAdapter(string adpName)
		{
			ToCastToComponenet InstanceOfComponent; 
			InstanceOfComponent = (ToCastToComponenet)ObjectTools.InstanceOfStringClass(this.AssemblyName, this.GenerateAdapterClass);
			IDataAdapter adp=(IDataAdapter)InstanceOfComponent.GetDataComponent(adpName);
			return adp;
		}
        protected DataSet FillData(IDataAdapter adapter,DataSet dataSet,string condition)
		{
            string Interncondition;
			if((condition=="")||(condition==null))
			{
				Interncondition="";
			}
			else
			{
				Interncondition = " where " + condition;
			}

			switch(connectionStrType)
			{
				case GVC.ConnectionSQL_AppE:
					((System.Data.SqlClient.SqlDataAdapter)adapter).SelectCommand.CommandText += Interncondition;
					break;
				case GVC.ConnectionAccess_AppE:
					((System.Data.OleDb.OleDbDataAdapter)adapter).SelectCommand.CommandText += Interncondition;
					break;
				default:
					((System.Data.SqlClient.SqlDataAdapter)adapter).SelectCommand.CommandText += Interncondition;
					break;
			}

			lMgr.Connection = LibraryMgr.GetConnection(this.connectionStrType);
			lMgr.SetAdapterConnection(adapter, this.connectionStrType);
			
			try
			{
				adapter.Fill(dataSet);
			}
			catch(Exception exception)
			{
				WebUtils.HandleException(exception);
			}
			
			return dataSet;
        }

		protected void UpdateData(IDataAdapter adapter,DataSet dataSet)
		{
			lMgr.Connection = LibraryMgr.GetConnection(this.connectionStrType);
			lMgr.SetAdapterConnection(adapter,this.connectionStrType);
			try
			{
				adapter.Update(dataSet);
			}
			catch(Exception exception)
			{
				WebUtils.HandleException(exception);
			}
		}

		protected SqlDataReader ExecuteCommandReader(System.Data.IDbCommand command)
		{
			lMgr.Connection = LibraryMgr.GetConnection(this.connectionStrType);
			lMgr.SetCommandConnection(command);
			try
			{
				if(command.Connection.State == ConnectionState.Closed)
                    command.Connection.Open();
				return (SqlDataReader)command.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (Exception exception)
			{
				command.Connection.Close();
				WebUtils.HandleException(exception);
				return null;
			}
		}

		protected void ExecuteCommand(System.Data.IDbCommand command)
		{
			lMgr.Connection = LibraryMgr.GetConnection(this.connectionStrType);
			lMgr.SetCommandConnection(command);
			try
			{
				if(command.Connection.State==ConnectionState.Closed)
                    command.Connection.Open();
				command.ExecuteNonQuery();
				command.Connection.Close();
			}
			catch(Exception exception)
			{
				command.Connection.Close();
				WebUtils.HandleException(exception);
			}
		}
		
		protected void SetCommandParameter(System.Data.IDbCommand command,string paramName,object paramValue)
		{
			((IDataParameter)command.Parameters[paramName]).Value = paramValue;
		}

		protected IDbCommand GetCommand(string cmdName)
		{
			ToCastToComponenet InstanceOfComponent; 
			InstanceOfComponent = (ToCastToComponenet)ObjectTools.InstanceOfStringClass(this.AssemblyName,this.GenerateAdapterClass);
			IDbCommand cmd=(IDbCommand)InstanceOfComponent.GetDataComponent(cmdName);
			return cmd;
		}

		public IDbConnection GetConnection()
		{
			return Connection;
		}
	}
}
