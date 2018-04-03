
namespace lw.CTE
{
	/// <summary>
	/// Present different parameters that are using in _app.config or web.config or Parameters.config
	/// </summary>
	public class parameters
	{
		/// <summary>
		/// Parameter appended as a query string to initialize inline editing within the websites
		/// </summary>
		public const string Edit_Param = "edit-content";

		/// <summary>
		/// Editing parameter value
		/// </summary>
		public const string Edit_Param_Value = "true";

		/// <summary>
		/// Used to set the errors email address.
		/// </summary>
		public const string Errors_Email = "ErrorsEmail";

        /// <summary>
        /// Used to set the errors email address.
        /// </summary>
        public const string Notification_Email = "NotificationEmail";


		/// <summary>
		/// Used as a config parameters to switch between godaddy hosting and others
		/// </summary>
		public const string IsGodaddyHosting = "IsGodaddyHosting";

		/// <summary>
		/// The folder name that is used in Godaddy hosting for virtual directory or IIS application
		/// </summary>
		public const string GodaddyFolderName = "GodaddyFolderName";

		/// <summary>
		/// default login url parameter
		/// </summary>
		public const string LoginURL = "LoginURL";

		/// <summary>
		/// Used to encrypt member passwords
		/// </summary>
		public const string MemberPasswordEncryption = "UJJAL**01";


		/// <summary>
		/// Default culture info settings parameter (used to save in the database)
		/// </summary>
		public const string DefaultDBCutureInfo = "DefaultCultureInfo";


		/// <summary>
		/// Default Search Index Directory Parameter (used for search)
		/// </summary>
		public const string SearchIndexDirectory = "SearchIndexDirectory";


		/// <summary>
		/// Used to indicate if the tag is used in CMS mode
		/// </summary>
		public const string CMSMode = "CMSMode";


		/// <summary>
		/// Indicates image not found's default location.
		/// </summary>
		public const string ImageNotFound = "ImageNotFound";


		/// <summary>
		/// Indicates the size of the video thumb size
		/// </summary>
		public const string VideoThumbSize = "VideoThumbSize";


		/// <summary>
		/// Used to exlude "auto include" lw js files from lw.js.dll
		/// Ex: lw.lwjs, jquery.lwjs
		/// </summary>
		public const string DoNotAutoIncludeLwJsFiles = "DoNotAutoIncludeLwJsFiles";


		/// <summary>
		/// Used to exlude image folders from being seen by in the cms
		/// Ex: corporate, icons
		/// </summary>
		public const string AccessDeniedFolders = "AccessDeniedFolders";


		/// <summary>
		/// Defines if we should use multiple prices for one item
		/// ex: for fruits we might need box, kg and item price
		/// while in other cases we don't need these variables.
		/// </summary>
		public const string UseMultiplePriceForItems = "UseMultiplePriceForItems";

		/// <summary>
		/// Specifies if lwjs should be served minified
		/// </summary>
		public const string LwJSMinify = "LwJSMinify";


		/// <summary>
		/// Defines the preffered domain of the website.
		/// All access from other domains will redirect to this domain
		/// </summary>
		public const string Domain = "Domain";

		/// <summary>
		/// Defines the preffered secure domain of the website.
		/// All access to secure pages will be redirect to this domain
		/// </summary>
		public const string SecureDomain = "SecureDomain";

		/// <summary>
		/// Defines the absolute path for the tags system to be used in the website.
		/// </summary>
		public const string HashTagsAbsPath = "HashTagsAbsPath";

        /// <summary>
        /// All Facebook information
        /// </summary>
        public const string FacebookAccessToken = "Facebook_AccessToken";

        /// <summary>
        /// All Twitter information
        /// </summary>
        public const string TwitterConsumerKey = "Twitter_oauth_consumer_key";
        public const string TwitterConsumerSecret = "Twitter_oauth_consumer_secret";
        public const string TwitterToken = "Twitter_oauth_token";
		public const string TwitterSecretToken = "Twitter_oauth_token_secret";

		/// <summary>
		/// Home page uniquename/url
		/// </summary>
		//public const string HomePageUniqueName = "home";


		/// <summary>
		/// DevelopmentMode takes true/false
		/// </summary>
		public const string DevelopmentMode = "DevelopmentMode";
	}
}
