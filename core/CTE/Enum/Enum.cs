
using lw.Utils;

namespace lw.CTE.Enum
{
	
	public enum LinkType
	{
		Image = 1,
		Title = 2,
		Custom = 3
	}

	public enum ImageResizeType
	{
		[Description("Fixed Width and Height")]
		Crop,

		[Description("Proportional Width and Height")]
		Resize
	}


	public enum PhotoAlbumsLinkType
	{
		Photos,
		ArchiveByType,
		AlbumId
	}
}
