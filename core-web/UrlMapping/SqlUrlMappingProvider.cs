using System;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;

namespace lw.UrlMapping
{
    /// <summary>
    /// An implementation of <see cref="IUrlMappingProvider" />, providing the
    /// <see cref="UrlMappingModule" /> with URL templates and redirection mappings
    /// from items stored in a SQL Server database table.
    /// </summary>
    /// <remarks>
    /// <para>
    /// With this provider, URL template and redirection mappings are stored in a table 
    /// and retrieved through a stored procedure.  The stored procedure should accept no
    /// parameters and return a result set with three named columns:
    /// [Name], [UrlTemplate], and [Redirection].  
    /// </para>
    /// <para>
    /// The <c>[Name]</c> column contains a developer-relevant name
    /// and is not used directly by the UrlMappingModule.  
    /// </para>
    /// <para>
    /// The <c>[UrlTemplate]</c> column contains is a string
    /// specifying an incoming URL or URL pattern to match.  The urlTemplate may reflect a static
    /// URL, such as <i>Reports.aspx</i>, or may include tokens in [squareBrackets] to imply
    /// a pattern for dynamic matching, such as <i>Reports/[ReportID].aspx</i>.  
    /// </para>
    /// <para>
    /// The <c>[Redirection]</c> column contains a string specifying the URL to redirect to when
    /// the given <c>urlTemplate</c> is matched on an incoming URL.
    /// </para>
    /// <para>
    /// The following shows an example of
    /// a SQL script that creates and populates a table of URL mappings, as well as a 
    /// stored procedure for retrieving the items:
    /// <code>
    ///-- create the mappings table
    ///create table UrlMappings
    ///(
    ///   [Name] varchar(255) NOT NULL
    ///  ,[UrlTemplate] varchar(8000) NOT NULL
    ///  ,[Redirection] varchar(8000) NOT NULL
    ///  ,[SortOrder] numeric(18,0) NOT NULL
    /// )
    ///
    ///
    ///-- create the stored procedure
    ///create procedure GetUrlMappings
    ///as                                    
    ///begin                                 
    ///  select Name,UrlTemplate,Redirection 
    ///    from UrlMappings                  
    ///    order by SortOrder, Name          
    ///end                                   
    ///
    ///
    ///--  populate the table with sample data
    ///insert into UrlMappings 
    ///  values('default', 'Default.aspx', 'Default.aspx', 1);
    ///
    ///insert into UrlMappings
    ///  values('msdn', 'msdn.aspx', 'http://www.msdn.com', 2);
    ///  
    ///insert into UrlMappings
    ///  values('customerReports', 'Customers/[ID]/[Action].aspx', 'CustReports.aspx', 3);
    /// </code>
    /// 
    /// Note that the <c>[UrlTemplate]</c> column values all assume an application-relative
    /// incoming URL.  The first record, <i>'default'</i> shows a static redirection to the
    /// same application-relative URL.  
    /// The second, <i>'msdn'</i>, shows a static redirection to an external web site.
    /// </para>
    /// <para>
    /// The third, <i>'customerReports'</i>, shows a dynamic redirection, using [Tokens]
    /// in the <c>[UrlTemplate]</c> column to represent portions of a possible incoming URL.  If an
    /// incoming URL matches the pattern, the [ID] and [Action] values are parsed and appended
    /// as querystring values to the redirection page 'CustReports.aspx'.  For example,
    /// an incoming URL of <i>'Customers/407/Display.aspx'</i> is matched 
    /// according to this third mapping item and redirected to the application URL
    /// <i>'CustReports.aspx?ID=407&amp;Action=Display'</i>.
    /// </para>
    /// <para>
    /// Matches are attempted in the order in which records appear in the
    /// result set returned by the stored procedure.  
    /// In the event an incoming URL could match multiple items, the redirection of
    /// the first item matched is used.
    /// </para>
    /// <para>
    /// Assuming the table name is [UrlMappings], and the stored procedure is named
    /// [GetUrlMappings], with a connection string name "UrlMappingData" defined in 
    /// the Web.config &lt;connectionStrings&gt; entry, the following shows an example of 
    /// the additional Web.config settings
    /// to make use of this provider.  Note that three seperate pieces of 
    /// configuration are required:  the <c>&lt;configSections&gt;</c> section tag entry,
    /// identifying that the provider-specific SqlUrlMappingProviderConfiguration object 
    /// should be used for configuration; the 
    /// <c>&lt;urlMappingModule&gt;</c> entry specifying the stored procedure and table names,
    /// and the <c>&lt;httpModules&gt;</c>
    /// entry, telling ASP.NET to include the UrlMappingModule in the pipeline.
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
    ///                    ignoreExtensions=".js .css .jpg"
    ///                    automaticallyUpdateFormAction="true"
    ///                    urlProcessingEvent="BeginRequest"
    ///                    authorizeRedirectionUrl="false"
    ///                    authorizeFailureRedirectUrl=""
    ///                    connectionStringName="UrlMappingData"
    ///                    tableName="UrlMappings"
    ///                    procName="GetUrlMappings"
    ///                    useDependency="true"
    ///                    dependencyName="TestDataDep"
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
    /// </para>
    /// <para>
    /// For detailed information on each of the configuration attributes, see
    /// the <see cref="SqlUrlMappingProviderConfiguration" /> object documentation.
    /// Note that the <c>useDependency</c> attribute implies additional configuration
    /// in SQL Server if set to <c>true</c>; see the <see cref="SqlUrlMappingProviderConfiguration.UseDependency">UseDependency</see>
    /// property documentation for more information.
    /// </para>
    /// </remarks>
    /// <seealso cref="SqlUrlMappingProviderConfiguration" />
    public class SqlUrlMappingProvider : IUrlMappingProvider
    {
        private const string kCACHE_KEY = "__SqlUrlMappingProvider_cache_key__";

        private string _connectionString;
        private string _procName;
        private string _tableName;
        private UrlMappingItemCollection _coll;
        private SqlCacheDependency _sqlDependency;
        private bool _useDependency;
        private string _dependencyName;
        private IncomingQueryStringBehaviorEnum _qsBehavior;

        // to help with debugging, provide a property that indicates when the urlmapping
        // data was last refreshed
        private DateTime _latestRefresh;
	

        #region IUrlMappingProvider Members

        /// <summary>
        /// Provides the <see cref="UrlMappingModule" /> with an internally-cached
        /// list of URL templates and redirection mappings processed from records
        /// returned by a SQL Server stored procedure.
        /// </summary>
        /// <returns>The collection of URL redirection mappings</returns>
        UrlMappingItemCollection IUrlMappingProvider.GetUrlMappings()
        {
            // if we aren't using sqlDependencies, then return the collection that 
            // was generated upon initialization
            if (!_useDependency) return _coll;

            // if we are using a sqlDependency, check to see if we have a 
            // valid collection already processed in cache
            if (HttpContext.Current.Cache[kCACHE_KEY] != null)
                return _coll;

            // if not, we need to refresh the url mappings from sql
            RefreshUrlMappingData();
            return _coll;
        }


        /// <summary>
        /// Accepts a configuration object from the <see cref="UrlMappingModule"/>
        /// and initializes the provider.
        /// </summary>
        /// <param name="config">
        /// the configuration settings typed as a <c>UrlMappingProviderConfiguration</c> object; 
        /// the actual object type may be a subclass of <c>UrlMappingProviderConfiguration</c>.
        /// </param>
        void IUrlMappingProvider.Initialize(UrlMappingProviderConfiguration config)
        {
            // cast the configuration object provided by the module
            SqlUrlMappingProviderConfiguration sqlConfig = (config as SqlUrlMappingProviderConfiguration);
            if (sqlConfig == null)
                throw new ProviderException("Invalid SqlUrlMappingProvider configuration.  Check the web.config file settings.");

            // remember configuration settings
            _connectionString = sqlConfig.ConnectionStringName;
            _procName = sqlConfig.ProcName;
            _tableName = sqlConfig.TableName;
            _useDependency = sqlConfig.UseDependency;
            _dependencyName = sqlConfig.DependencyName;
            _qsBehavior = sqlConfig.IncomingQueryStringBehavior;
            

            // if useDependency is set, make sure a dependency name is also
            if (_useDependency && string.IsNullOrEmpty(_dependencyName))
                throw new ProviderException("Invalid SqlUrlMappingProvider configuration.  If 'useDependency' is true, you must supply a 'dependencyName'.  Check the web.config file settings.");

            // initialize the url mappings
            RefreshUrlMappingData();
        }

        /// <summary>
        /// Releases resources used in the <see cref="IUrlMappingProvider.Initialize">Initialize</see>
        /// method; not implemented in this provider.
        /// </summary>
        void IUrlMappingProvider.Dispose()
        {
        }


        /// <summary>
        /// Implements the IUrlMappingProvider method to refresh internally-cached
        /// URL mappings.
        /// </summary>
        void IUrlMappingProvider.RefreshUrlMappings()
        {
            RefreshUrlMappingData();
        }

        /// <summary>
        /// Returns the date and time the provider most recently refreshed its
        /// data
        /// </summary>
        /// <returns>the most recent refresh time</returns>
        DateTime IUrlMappingProvider.GetLastRefreshTime()
        {
            return _latestRefresh;
        }


        #endregion

        /// <summary>
        /// Refreshes the internally-cached collection of URL templates and redirection mappings.
        /// </summary>
        protected void RefreshUrlMappingData()
        {
            if (_coll != null)
                _coll.Clear();
            else
                _coll = new UrlMappingItemCollection();


            // make a connection to the database and execute the SQL stored procedure
            // to retrieve the listing of URL items in the form
            // [Name], [UrlTemplate], [Redirection]

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = _procName;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    da.Dispose();

                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DataRow r = ds.Tables[0].Rows[i];
                            UrlMappingItem item
                              = UrlMappingHelper.CreateTemplatedMappingItem(
                                    r["Name"].ToString(), r["UrlTemplate"].ToString(), r["Redirection"].ToString(), _qsBehavior
                                   );

                            _coll.Add(item);
                        }
                    }
                }
            }

            // if we're using a sqlDependency, generate it now
            if (_useDependency)
            {
                _sqlDependency = new SqlCacheDependency(_dependencyName, _tableName);
                HttpContext.Current.Cache.Insert(kCACHE_KEY, "dummyValue", _sqlDependency, DateTime.Now.AddDays(10), Cache.NoSlidingExpiration);
            }

            // remember the refresh time
            _latestRefresh = System.DateTime.Now;

        }


        /// <summary>
        /// Returns the connection string name associated with the provider from the 
        /// Web.config file.
        /// </summary>
        /// <returns>the connection name as a string</returns>
        protected string GetConnectionString()
        {
			return lw.WebTools.Config.GetConnectionString(_connectionString);
        }





    }
}
