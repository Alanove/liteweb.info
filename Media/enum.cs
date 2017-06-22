using lw.Utils;

namespace lw.Widgets
{
	public enum DefaultWidgetsTypes
	{
		[Description("NULL")]
		NULL = 0,
		[Description("Photo Album")]
		PhotoAlbum = 1,
		[Description("Video Album")]
		VideoAlbum = 2,
		[Description("Mixed Album")]
		MixedAlbum = 3,
		[Description("Downloads List")]
		DownloadsList = 4,
		[Description("Embed Object")]
		EmbedObject = 5,
		[Description("Popup")]
		PopUp = 6,
		[Description("Form")]
		Form = 7,
		[Description("Poll")]
		Poll = 8
	}

	public enum DefaultMediaTypes
	{
		[Description("NULL")]
		NULL = 0,
		[Description("Image")]
		Image = 1,
		[Description("Video")]
		Video = 2,
		[Description("Download")]
		Download = 3,
		[Description("Embed Object")]
		EmbedObject = 4,
		[Description("Popup")]
		PopUp = 5,
		[Description("Form")]
		Form = 6,
		[Description("Poll")]
		Poll = 7
	}

	public enum FileResponses
	{
		[Description("Sucessfully Added")]
		Success = 1,
		[Description("An Error Occured, File not Added")]
		Error = 2,
		[Description("File Already Exists")]
		FileExist = 3,
		[Description("File Not Found")]
		FileNotFound = 4
	}

	public enum MediaStatus
	{
		Hidden = 0,
		Visible = 1,
		All = 3
	}
}
