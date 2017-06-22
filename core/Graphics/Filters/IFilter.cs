using System.Drawing;

namespace lw.GraphicUtils.Filters
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public interface IFilter
	{
	  Image ExecuteFilter(Image inputImage);
	}
}
