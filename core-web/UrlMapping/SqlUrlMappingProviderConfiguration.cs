using System;
using System.Configuration;

namespace lw.UrlMapping
{
    /// <summary>
    /// The configuration object used when the <see cref="UrlMappingModule" />
    /// is configured to receive mapping items from the <see cref="SqlUrlMappingProvider" />.
    /// </summary>
    /// <remarks>
    /// Configuration for the SqlUrlMappingProvider is established in the Web.config file
    /// through a custom &lt;configSections&gt; entry, as so:
    /// <code>
    /// &lt;configuration&gt;
    ///  &lt;configSections&gt;
    ///    &lt;section name="urlMappingModule" 
    ///             type="UNLV.IAP.UrlMapping.SqlUrlMappingProviderConfiguration, UrlMappingModule"
    ///             /&gt;
    ///  &lt;/configSections&gt;
    ///    
    ///  &lt;urlMappingModule providerType="UNLV.IAP.UrlMapping.SqlUrlMappingProvider, UrlMappingModule"
    ///                    noMatchAction="ThrowException" 
    ///                    noMatchRedirectUrl="" 
    ///                    incomingQueryStringBehavior="PassThrough" 
    ///                    connectionStringName="TestData"
    ///                    ignoreExtensions=".js .css .jpg"
    ///                    automaticallyUpdateFormAction="true"
    ///                    urlProcessingEvent="BeginRequest"
    ///                    authorizeRedirectionUrl="false"
    ///                    authorizeFailureRedirectUrl=""
    ///	                   tableName="UrlMappings"
    ///	                   procName="GetUrlMappings"
    ///	                   useDependency="true"
    ///	                   dependencyName="TestData"
    ///                    /&gt;
    ///
    ///  &lt;system.web&gt; 
    ///    &lt;httpModules&gt;
    ///      &lt;add name="UrlMappingModule" 
    ///           type="UNLV.IAP.UrlMapping.UrlMappingModule, UrlMappingModule"
    ///           /&gt;
    ///    &lt;/httpModules&gt;
    ///  &lt;/system.web&gt;
    /// 
    ///&lt;/configuration&gt;
    /// </code>
    /// In the above example, the custom configuration section is named "urlMappingModule"
    /// and the corresponding <c>&lt;urlMappingModule&gt;</c> entry contains the following attributes
    /// for configuring the provider:
    /// <para>
    /// &#8226; <c>providerType</c> identifies the class and assembly for the provider object, and
    /// should be set to "UNLV.IAP.UrlMapping.SqlUrlMappingProvider, UrlMappingModule".
    /// </para>
    /// <para>
    /// &#8226; <c>noMatchAction</c> specifies the UrlMappingModule's behavior in the event an incoming
    /// URL cannot be matched to any of the mapping items listed in the mapping table.
    /// For a list of possible actions, see the <see cref="NoMatchActionEnum" /> documentation.
    /// </para>
    /// <para>
    /// &#8226; <c>noMatchRedirectUrl</c> is used when <c>noMatchAction</c> is set 
    /// to "<see cref="NoMatchActionEnum.Redirect">Redirect</see>" to provide a redirection
    /// URL for an unmatched incoming URL.
    /// </para>
    /// <para>
    /// &#8226; <c>incomingQueryStringBehavior</c> specifies how the UrlMappingModule should treat
    /// querystring arguments on an incoming URL.  For a list of possible options, see
    /// the <see cref="IncomingQueryStringBehaviorEnum" /> documentation.
    /// </para>
    /// <para>
    /// &#8226; <c>ignoreExtensions</c> is a list of file extensions, requests for which 
    /// will be ignored completely by the UrlMappingModule. For example, to allow all 
    /// requests for JavaScript files and CSS stylesheets to pass by the UrlMappingModule 
    /// and be processed as normal by ASP.NET without redirection, set this attribute to 
    /// the string <c>".js .css"</c>. Separate multiple items with spaces or commas.
    /// </para>
    /// <para>
    /// &#8226; <c>automaticallyUpdateFormAction</c> is an optional Boolean instructing 
    /// the UrlMappingModule to automatically adjust rendered <c>&lt;form action="…"&gt;</c>
    /// tags such that the <c>action</c> for a redirected form matches the incoming URL. 
    /// The default value is <c>true</c>.  If changed to <c>false</c>, it is incumbant on
    /// the developer to peform this adjustment in some other fashion.
    /// </para>
    /// <para>
    /// &#8226; <c>urlProcessingEvent</c> determines when in the request cycle the 
    /// UrlMappingModule processes the incoming URL. Possible values are defined by 
    /// the <see cref="UrlProcessingEventEnum">UrlProcessingEventEnum</see> enumeration.
    /// </para>
    /// <para>
    /// &#8226; <c>authorizeRedirectionUrl</c> is an optional Boolean that when <c>true</c>
    /// instructs the UrlMappingModule to explicitly authorize the redirection url upon an incoming
    /// URL match.  This may be useful when using the UrlMappingModule in conjunction with Forms 
    /// authentication should the developer wish to maintain authorization rules on 
    /// concrete folders and .aspx pages, rather than solely on the incoming URLs.
    /// </para>
    /// <para>
    /// &#8226; <c>authorizeFailureRedirectUrl</c> is used when <c>authorizeRedirectionUrl</c>
    /// is <c>true</c> to specify a redirection page when the mapped redirection is
    /// unauthorized.
    /// </para>
    /// <para>
    /// &#8226; <c>connectionStringName</c> specifies the name of the &lt;connectionString&gt;
    /// entry that defines the SQL Server database connection
    /// </para>
    /// <para>
    /// &#8226; <c>tableName</c> is the name of the SQL Server database table containing 
    /// URL templates and redirection mappings.
    /// </para>
    /// <para>
    /// &#8226; <c>procName</c> is the name of a SQL Server stored procedure that retrieves
    /// a data set of url templates and redirection mappings.  The procedure must accept
    /// no arguments and return a result set with three named columns:  [Name], [UrlTemplate],
    /// and [Redirection].
    /// </para>
    /// <para>
    /// &#8226; <c>useDependency</c> determines whether the table identified by
    /// <c>tableName</c> should be periodically polled for changes.  If <c>useDependency</c>
    /// is set to <c>true</c>, polling should occur and the internally-cached data for URL
    /// mappings should be refreshed upon updates.  If <c>useDependency</c> is false,
    /// changes in the mapping table will not be reflected until the next application startup.
    /// (note:  see below for additional configuration necessary to support dependencies).
    /// </para>
    /// <para>
    /// &#8226; <c>dependencyName</c> is used when <c>useDependency="true"</c> to specify
    /// the name of the associated <c>sqlCacheDependency</c> entry in the Web.config file.
    /// When a dependency is specified, an additional
    /// Web.config entry defining configuration for the dependency is required.  The following
    /// shows an example of such an entry:
    /// <code>
    ///&lt;system.web&gt;
    ///  &lt;caching&gt;
    ///    &lt;sqlCacheDependency enabled="true"&gt;
    ///      &lt;databases&gt;
    ///        &lt;add name="TestData" connectionStringName="TestData" 
    ///             pollTime="30000"
    ///             /&gt;
    ///      &lt;/databases&gt;
    ///    &lt;/sqlCacheDependency&gt;
    ///  &lt;/caching&gt;
    ///&lt;/system.web&gt;
    ///</code>
    /// Additional configuration in SQL Server is required to support dependencies.
    /// Consult the MSDN documentation for more information:
    /// <a href="http://msdn2.microsoft.com/en-us/library/ms178604(VS.80).aspx">http://msdn2.microsoft.com/en-us/library/ms178604(VS.80).aspx</a>
    /// </para>
    /// </remarks>
    /// 
    public class SqlUrlMappingProviderConfiguration : UrlMappingProviderConfiguration
    {
        /// <summary>
        /// The name of the <c>connectionString</c> item used to establish the SQL Server 
        /// database connection.
        /// </summary>
        [ConfigurationProperty("connectionStringName", IsRequired = true)]
        public string ConnectionStringName
        {
            get { return (String)this["connectionStringName"]; }
            set { this["connectionStringName"] = value; }
        }

        /// <summary>
        /// The name of the stored procedure that is to be executed to retrieve 
        /// URL mapping items.
        /// </summary>
        /// <remarks>
        /// The stored procedure should return a result set that includes three columns:
        /// [Name], [UrlTemplate], and [Redirection].
        /// </remarks>
        [ConfigurationProperty("procName", IsRequired = true)]
        public string ProcName
        {
            get { return (String)this["procName"]; }
            set { this["procName"] = value; }
        }

        /// <summary>
        /// The name of the table that contains URL mapping items.
        /// </summary>
        /// <remarks>
        /// This table name is necessary to support SQL dependencies when 
        /// <see cref="UseDependency" /> is set to <c>true</c>.
        /// </remarks>
        [ConfigurationProperty("tableName", IsRequired = true)]
        public string TableName
        {
            get { return (String)this["tableName"]; }
            set { this["tableName"] = value; }
        }

        /// <summary>
        /// Determines if a <see cref="System.Web.Caching.SqlCacheDependency">SqlCacheDependency</see> is used to poll the 
        /// table containing URL mapping items for updates and refresh the cached data
        /// accordingly.
        /// </summary>
        /// <value>
        /// <c>true</c> if the dependency should be established, <c>false</c>
        /// if not.
        /// </value>
        /// <remarks>
        /// In addition to setting the <c>useDependency</c> attribute to <c>true</c>
        /// in the Web.config UrlModuleMapping configuration section, additional steps are
        /// required to support dependencies in SQL Server 2000 and 2005.  Consult
        /// MSDN for documentation on how to establish a SqlCacheDependency for either
        /// server version:  
        /// <a href="http://msdn2.microsoft.com/en-us/library/ms178604(VS.80).aspx">http://msdn2.microsoft.com/en-us/library/ms178604(VS.80).aspx</a>
        /// </remarks>
        /// <seealso cref="DependencyName"/>
        [ConfigurationProperty("useDependency", IsRequired=false, DefaultValue=false)]
        public bool UseDependency
        {
            get { return (bool)this["useDependency"]; }
            set { this["useDependency"] = value; }
        }

        /// <summary>
        /// The name associated with a Web.config <c>sqlCacheDependency</c> entry
        /// used by this provider when <see cref="UseDependency"/> is <c>true</c>.
        /// </summary>
        /// <example>
        /// For example, given the following Cache configuration in Web.config:
        /// <code>
        ///&lt;system.web&gt;  
        ///  &lt;caching&gt;
        ///    &lt;sqlCacheDependency enabled="true"&gt;
        ///      &lt;databases&gt;
        ///        &lt;add name="TestDataDep" 
        ///             connectionStringName="TestData" 
        ///             pollTime="30000"
        ///             /&gt;
        ///      &lt;/databases&gt;
        ///    &lt;/sqlCacheDependency&gt;
        ///  &lt;/caching&gt;
        ///&lt;/system.web&gt;  
        ///</code>
        /// the <c>DependencyName</c> property for the UrlMappingModule configuration
        /// should be set to "TestDataDep", as so:
        /// <code>
        /// &lt;configuration&gt;
        ///  &lt;configSections&gt;
        ///    &lt;section name="urlMappingModule" type="UNLV.IAP.UrlMapping.SqlUrlMappingProviderConfiguration, UrlMappingModule"/&gt;
        ///  &lt;/configSections&gt;
        ///    
        ///  &lt;urlMappingModule providerType="UNLV.IAP.UrlMapping.SqlUrlMappingProvider, UrlMappingModule"
        ///                    noMatchAction="Redirect" 
        ///                    noMatchRedirectUrl="NotFound.aspx" 
        ///                    connectionStringName="TestData"
        ///                    tableName="UrlMappings"
        ///                    procName="GetUrlMappings"
        ///                    useDependency="true"
        ///                    dependencyName="TestDataDep"
        ///                    /&gt;
        ///                    
        ///  ...   
        ///&lt;/configuration&gt;
        /// </code>
        /// </example>
        /// <seealso cref="UseDependency" />
        [ConfigurationProperty("dependencyName", IsRequired=false, DefaultValue="")]
        public string DependencyName
        {
            get { return (String)this["dependencyName"]; }
            set { this["dependencyName"] = value; }
        }

    

    }
}
