using System;
using System.Configuration;

namespace lw.CTE
{
	public class Folders
	{
		public const string DataSetFolder = "Prv/Conf";

		public const string UserControlsFolder = "~/_Controls";

		public const string CategoriesImages = "/Prv/Images/Categories";

		public const string BrandsImages = "/Prv/Images/Brands";

		public const string NewsImages = "/Prv/Images/News";

		public const string MainSliderFolder = "Images/slides";

		public static string ManagerFolder
		{
			get
			{
				return ConfigurationManager.AppSettings["ManagerFolder"];
			}
		}
		public const string CountriesDs = "Prv/conf/Countries.config";

		public const string CompaniesImages = "Prv/Images/Companies";

		public const string StaticImages = "Prv/Images/Static";

		public const string Images = "images";

		public const string ImagesThumbs = "Prv/Images/lw-thumb";

		public const string GroupsFolders = "Prv/Images/Groups";

		public const string NewsFile = "Prv/Files/News";

		public const string PhotoAlbums = "Prv/Images/PhotoAlbums";

		public const string JobLogos = "Prv/Images/Jobs";

		public const string Downloads = "downloads";
		public const string DownloadsTemp = "downloads/temp";

		public const string AdsFolder = "Prv/Images/Ads";

		public const string CommonPictures = "pictures";

		public const string ErrorLogs = "Prv/logs/errors";

		public const string VideoThumbsFolder = "Prv/Images/Videos";
		public const string VideosFolder = "Prv/Videos";

		public const string CommentsFolder = "prv/comments";

		public const string CaptchaURL = "PRV/handlers/CaptchaImage.ashx";

		public const string NewsTypesFolder = "Prv/Images/NewsTypes";

		public const string Networks = "Prv/Images/Networks";

		public const string ManagerSchemas = "~/Prv/schemas";


		public const string PagesFolder = "Prv/Images/Pages";

		public const string AlternateImage = "Prv/Images/AlternateImages";

		public const string EventsFolder = "Prv/Images/Events";

        public const string FormsAttachementFolder = "Prv/Files/Forms";

		#region Products Images

		public const string ProductsImages = "Prv/Images/Products";
		public const string ProductsFiles = "/Prv/Files/Products";

		#endregion

		#region

		/// <summary>
		/// added for the media
		/// in the future we shall remove "PhotoAlbums", "VideoAlbums" and all related similar folders from the list above
		/// </summary>
		public const string Media = "Prv/Media";

		public const string MediaPhotoAlbums = Media + "/Images";
		public const string MediaVideoAlbums = Media + "/Videos";
		public const string MediaMixedAlbums = Media + "/Mixed";
		public const string MediaDownloads = Media + "/Downloads";
		public const string MediaEmbedObjects = Media + "/EmbedObjects";
		public const string MediaPopUps = Media + "/PopUps";
		public const string MediaForms = Media + "/Forms";
		public const string MediaPolls = Media + "/Polls";

		#endregion
	}
}