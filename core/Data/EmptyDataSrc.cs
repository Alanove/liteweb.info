
namespace lw.Data
{
	/// <summary>
	/// Class can be used to be a data source for CustomDataSrc instread of regular select statements
	/// example used in MemberEducationList
	/// </summary>
	public class EmptyDataSrc : IDataSource
	{
		object _data;
		int _rowsCount = -1;
		string _dataLibrary;
		string _selectCommand;
		bool _hasData = false;
		

		/// <summary>
		/// Gets or sets the Rows Count
		/// </summary>
		public virtual int RowsCount { get { return _rowsCount; } set { _rowsCount = value; } }
		
		/// <summary>
		/// It's just an image of GetData which must be overriden to create something usefull
		/// </summary>
		/// <returns></returns>
		public virtual object GetData() { return _data; }

		/// <summary>
		/// The data source's library
		/// </summary>
		public virtual string DataLibrary { get { return _dataLibrary; } set { _dataLibrary = value; } }
		
		/// <summary>
		/// The data source's select command
		/// </summary>
		public virtual string SelectCommand { get { return _selectCommand; } set { _selectCommand = value; } }

		/// <summary>
		/// returns if the Data Source has data
		/// </summary>
		public virtual bool HasData { get { return _hasData; } set { _hasData = value; } }

		/// <summary>
		/// Returns the data
		/// </summary>
		public virtual object Data { get { return _data; } set { _data = value; } }
	}
}
