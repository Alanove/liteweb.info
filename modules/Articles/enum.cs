
using lw.Utils;

namespace lw.Articles
{
	public enum NewsStatus
	{
		NoDisplay = 0,
		Archive = 2,
		MainPage = 3
	}
	public enum NewsTagStatus
	{
		NoDisplay = 0,
		Archive = 2,
		MainPage = 3,
		Inherit = 4
	}
	/// <summary>
	/// Type of News link ,
	/// Auto ???
	/// Article linked to the news UniqueName
	/// ArticleID id is added in the end
	/// DownloadFile link directly to the associated news file the file will be downloaded
	/// DirectFileLink link directly to the associated news file the file will open by default browswer behavior
	/// Archive ???
	/// ArchiveByDate ??
	/// ArchiveByType ??
	/// ArchiveByYear ???
	/// </summary>
	public enum NewsLinkType
	{
		Auto,
		Article,
		ArticleID,
		DownloadFile,
		DirectFileLink,
		Archive,
		ArchiveByDate,
		ArchiveByType,
		ArchiveByYear
	}

	public enum TypesSort
	{
		Custom,

		[Description("DateCreated Asc")]
		DateAsc,

		[Description("DateCreated Desc")]
		DateDesc,

		[Description("UserRating Desc")]
		UserRating,

		[Description("Views Desc")]
		PageViews,

		[Description("Ranking Desc")]
		Ranking,

		[Description("Name Desc")]
		NameDesc,

		[Description("Name Asc")]
		NameAsc
	}

	public enum ArticlesSort
	{
		Custom,

		[Description("NewsDate Asc")]
		DateAsc,

		[Description("NewsDate Desc")]
		DateDesc,

		[Description("UserRating Desc")]
		UserRating,

		[Description("PageViews Desc")]
		PageViews,

		[Description("Ranking Desc")]
		Ranking
	}
}
