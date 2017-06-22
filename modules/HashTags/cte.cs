
namespace lw.HashTags
{
	/// <summary>
	/// Cte class used for HashTags
	/// </summary>
	public class cte
	{
		public const string lib = "HashTags";

	}

	/// <summary>
	/// Enum to define hashtag types.
	/// The int value will be inserted in the database in the table "HashTags_Relations" column: RelationType
	/// </summary>
	public enum HashTagTypes
	{
		Pages = 1,
		Photos = 2,
		Comments = 3,
		Media = 4,
		MediaImages = 5,
		MediaVideos = 6,
		MediaDownloads = 7,
		MediaEmbedObject = 8,
		MediaPopups = 9
	}
}
