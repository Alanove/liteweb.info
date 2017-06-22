
namespace lw.Content
{
	/// <summary>
	/// Class containing all the constants that are using in the <see cref="Content"/> library
	/// </summary>
	public class cte
	{
		public const string lib = "Testimonials";

		public const string ConfigFile = "testimonials.config";
	}


	public enum DisplayCondition
	{
		/// <summary>
		/// Displays when it is contained withing a IHasData and the HasData property is true
		/// </summary>
		ParentHasData,

		/// <summary>
		/// Displays when it is contained withing a IHasData and the HasData property is false
		/// </summary>
		ParentNoData,
		
		/// <summary>
		/// Displays when one of the inner IDataProperty is visible
		/// </summary>
		CheckInnerProperties,

		/// <summary>
		/// takes multiple datasources in the Source field separated by comma ","
		/// ex: DisplayTest="Multiple" Source="source1,source2"
		/// Will react to both sources (both have no data)
		/// </summary>
		MultipleNoData
	}
}
