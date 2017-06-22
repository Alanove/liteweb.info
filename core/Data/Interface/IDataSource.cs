
namespace lw.Data
{
	public interface IDataSource
	{
		int RowsCount { get; }
		object GetData();
		string DataLibrary { get; set; }
		string SelectCommand { get; set; }
		bool HasData { get; }
		object Data { get; set; }
	}
}
