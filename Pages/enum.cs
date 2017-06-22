
using lw.Utils;

namespace lw.Pages
{
	public enum PageStatus
	{
		Hidden = 0,
		Published = 1,
		Important = 2,
		Menu = 3,
		Dynamic = 4,
		Deleted = 7
	}

	public enum PagesSort
	{
		Custom,

		[Description("PublishDate Asc")]
		DateAsc,

		[Description("PublishDate Desc")]
		DateDesc,

		[Description("UserRating Desc, Ranking Desc, PublishDate Desc")]
		UserRating,

		[Description("[Views] Desc, Ranking Desc, PublishDate Desc")]
		PageViews,

		[Description("Ranking Desc, PublishDate Desc")]
		Ranking
	}

    public enum PageTypeSort
    {
        Custom,

        [Description("Type Asc")]
        TypeAsc,

        [Description("Type Desc")]
        TypeDesc,

        [Description("ThumbSize Asc")]
        ThumbSizeAsc,

        [Description("ThumbSize Desc")]
        ThumbSizeDesc,

        [Description("MediumSize Asc")]
        MediumSizeAsc,

        [Description("MediumSize Desc")]
        MediumSizeDesc,

        [Description("LargeSize Asc")]
        LargeSizeAsc,

        [Description("LargeSize Desc")]
        LargeSizeDesc
    }

    public enum PageTemplateSort
    {
        Custom,

        [Description("Title Asc")]
        TitleAsc,

        [Description("Title Desc")]
        TitleDesc,

        [Description("Filename Asc")]
        FilenameAsc,

        [Description("Filename Desc")]
        FilenameDesc
    }
}
