
namespace lw.CTE
{
	/// <summary>
	/// Summary description for CTE.
	/// </summary>
	public class CTE
	{
		public CTE()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
	/// <summary>
	/// Global Variables Connection
	/// Global Variables ClassLibrary
	/// </summary>
	public class GVC
	{
		public const string TableXmlConnectionName = "Configurations";
		public const string NodeXmlKeyConnection = "ParamName";
		public const string NodeXmlValueConnection = "ParamValue";
		public const string ConnectionSQL_AppE = "SQL";
		public const string ConnectionAccess_AppE = "Access";
		public const string ConnectionOracle_AppE = "Oracle";
		public const string ConnectionsFile_AppE = "connectionsFile";
		
	}
	/// <summary>
	/// GlobalVariablesDataBase
	/// </summary>
	public class GVD
	{
		public const string RootDataBaseXmlFileConfig = "DataBaseConfig.config";
		
	}
	/// <summary>
	/// GlobalVariablesDataBase
	/// GlobalVariablesClassPath
	/// </summary>
	public class Application
	{
		public const string VirtualRoot ="VirtualRoot";
		public const string ManagerVirtualRoot = "ManagerVirtualRoot";
		public const string WebConfigKeyAdminPath = "ManagerFolder";
	}
	/// <summary>
	/// GlobalVariablesClassPath
	/// GlobalVariablesClassLibrary
	/// </summary>
	public class GVCL
	{
		public const string TableXmlClassLibrary="ClassLibraryApplication";
		public const string NodeXmlKeyClassLibrary = "ClassLibraryName";
		public const string NodeXmlValueClassLibrary = "ConnectionName";
		public const string NodeXmlComponentClassName = "ComponentClass";
		public const string NodeXmlAssemblyName="AssemblyName";
	}

	public class ConfigCte
	{
		public const string ParamFile = "/PRV/Conf/Parameters.Config";
		public const string ConfigCacheKey = "SiteConfiguration";

		public const string StaticContentFolder = "PRV/StaticContent";
		public const string EmailsFolder = "PRV/Emails";
		public const string ErrorsFolder = "Prv/Errors";
		public const string MessagesFolder = "Prv/Messages";
	}

	public class WawSettings
	{
		public const int CompanyImageWidth = 200;
	}
}
