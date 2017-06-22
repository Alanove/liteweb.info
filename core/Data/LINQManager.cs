using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;


namespace lw.Data
{
	/// <summary>
	/// Base class for all LINQ Managers using in the lw library
	/// </summary>
	public class LINQManager
	{
		string dataLibrary = "";
		IDbConnection _connection;
		protected DataContext _dataContext;
		List<Exception> errors = new List<Exception>();

		/// <summary>
		/// All classes must have a datalibrary
		/// Called using the syntax :base(cte.library)
		/// </summary>
		/// <param name="library">DataLibrary libraries are defined in the Data.Config file</param>
		public LINQManager(string library)
		{
			this.dataLibrary = library;
		}

		/// <summary>
		/// Calls datacontext.SubmitChanges
		/// Save all the changes made to the context and commits the to database
		/// </summary>
		public void Save()
		{
			dataContext.SubmitChanges();
		}
		protected IDbConnection Connection
		{
			get
			{
				if (_connection == null)
				{
					DirectorBase db = new DirectorBase(dataLibrary);
					_connection = db.GetConnection();
				}
				return _connection;
			}
		}

		/// <summary>
		/// DataContext must be cast to the used DataContext like (AdsDataContext) 
		/// if not cast the context cannot be used efficiently.
		/// </summary>
		protected DataContext dataContext
		{
			get
			{
				if (_dataContext == null)
				{
					_dataContext = new DataContext(Connection);
				}
				return _dataContext;
			}
		}

		/// <summary>
		/// List of validation or non validation error that may occur in the proccess of getting or updating data
		/// </summary>
		public List<Exception> Errors
		{
			get
			{
				return errors;
			}
			set
			{
				errors = value;
			}
		}

	}
}
