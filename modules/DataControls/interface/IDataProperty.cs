
namespace lw.DataControls
{
	public interface IDataProperty
	{
		string Property { get; set; }
		bool IVisible { get; }
		string Format { get; set; }
	}
}
