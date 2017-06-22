using System;
using System.Configuration.Provider;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace lw.UrlMapping
{
    /// <summary>
    /// An implementation of <see cref="IUrlMappingProvider" />, providing the
    /// <see cref="UrlMappingModule" /> with URL templates and redirection mappings
    /// from items stored in an XML file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// With this provider, URL template and redirection mappings are identified as <c>&lt;urlMapping&gt;</c>
    /// nodes in an XML file, typically stored in the APP_DATA directory.  The <c>&lt;urlMapping&gt;</c>
    /// tag requires three attributes:  <c>name</c>, <c>urlTemplate</c>, and
    /// <c>redirection</c>.  
    /// </para>
    /// <para>
    /// The <c>name</c> attribute specifies a developer-relevant name
    /// and is not used directly by the UrlMappingModule.  
    /// </para>
    /// <para>
    /// The <c>urlTemplate</c> attribute is a string
    /// specifying an incoming URL or URL pattern to match.  The urlTemplate may reflect a static
    /// URL, such as <i>Reports.aspx</i>, or may include tokens in [squareBrackets] to imply
    /// a pattern for dynamic matching, such as <i>Reports/[ReportID].aspx</i>.  
    /// </para>
    /// <para>
    /// The <c>redirection</c> attribute is a string specifying the URL to redirect to when
    /// the given <c>urlTemplate</c> is matched on an incoming URL.
    /// </para>
    /// <para>
    /// Optionally, a fourth attribute, <c>enabled</c>, may be included.  Specify "true" or
    /// "false" for the value to enable or disable the mapping (default is "true").
    /// </para>
    /// <para>
    /// The following is an example of an XML file suitable for use by this provider:
    /// <code>
    ///&lt;?xml version="1.0" encoding="utf-8" ?&gt;
    ///&lt;urlMappings&gt;
    ///
    ///  &lt;urlMapping name="default" 
    ///              urlTemplate="Default.aspx" 
    ///              redirection="Default.aspx"
    ///              enabled="true"
    ///              /&gt;
    ///              
    ///  &lt;urlMapping name="msdn" 
    ///              urlTemplate="msdn.aspx" 
    ///              redirection="http://www.msdn.com" 
    ///              /&gt;
    ///              
    ///  &lt;urlMapping name="customerReports"
    ///              urlTemplate="Customers/[ID]/[Action].aspx" 
    ///              redirection="CustReports.aspx" 
    ///              enabled="true"
    ///              /&gt;
    ///
    ///&lt;/urlMappings&gt;
    /// </code>
    /// Note that the <c>urlTemplate</c> attributes all assume an application-relative
    /// incoming URL.  The first item, <i>"default"</i> shows a static redirection to the
    /// same application-relative URL.  
    /// The second, <i>"msdn"</i>, shows a static redirection to an external web site.
    /// </para>
    /// <para>
    /// The third, <i>"customerReports"</i>, shows a dynamic redirection, using [Tokens]
    /// in the <c>urlTemplate</c> to represent portions of a possible incoming URL.  If an
    /// incoming URL matches the pattern, the [ID] and [Action] values are parsed and appended
    /// as querystring values to the redirection page "CustReports.aspx".  For example,
    /// an incoming URL of <i>"Customers/407/Display.aspx"</i> is matched 
    /// according to this third mapping item and redirected to the application URL
    /// <i>"CustReports.aspx?ID=407&amp;Action=Display"</i>.
    /// </para>
    /// <para>
    /// Matches are attempted in the order in which &lt;urlMapping&gt; items appear in the
    /// file.  In the event an incoming URL could match multiple items, the redirection of
    /// the first item matched is used.
    /// </para>
    /// <para>
    /// Assuming the URL mapping file is stored in the APP_DATA directory with the filename
    /// <i>urlMappings.xml</i>, the following shows an example of the Web.config settings
    /// to make use of the provider with this file.  Note that three seperate pieces of 
    /// configuration are required:  the <c>&lt;configSections&gt;</c> section tag entry,
    /// identifying that the provider-specific XmlUrlMappingProviderConfiguration object 
    /// should be used for configuration; the 
    /// <c>&lt;urlMappingModule&gt;</c> entry including the <c>urlMappingFile</c> attribute
    /// specifying the XML file providing the mappings; and the <c>&lt;httpModules&gt;</c>
    /// entry, telling ASP.NET to include the UrlMappingModule in the pipeline.
    /// <code>
    /// &lt;configuration&gt;
    ///  &lt;configSections&gt;
    ///    &lt;section name="urlMappingModule" 
    ///             type="UNLV.IAP.UrlMapping.XmlUrlMappingProviderConfiguration, UrlMappingModule"
    ///             /&gt;
    ///  &lt;/configSections&gt;
    ///    
    ///  &lt;urlMappingModule providerType="UNLV.IAP.UrlMapping.XmlUrlMappingProvider, UrlMappingModule"
    ///                    noMatchAction="ThrowException" 
    ///                    noMatchRedirectUrl="" 
    ///                    incomingQueryStringBehavior="PassThrough" 
    ///                    ignoreExtensions=".js .css .jpg"
    ///                    automaticallyUpdateFormAction="true"
    ///                    urlProcessingEvent="BeginRequest"
    ///                    authorizeRedirectionUrl="false"
    ///                    authorizeFailureRedirectUrl=""
    ///                    useDependency="true"
    ///                    urlMappingFile="~/App_Data/urlMappings.xml" 
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
    /// For additional information on each of the configuration attributes, see
    /// the <see cref="XmlUrlMappingProviderConfiguration" /> object documentation.
    /// </para>
    /// </remarks>
    /// <seealso cref="XmlUrlMappingProviderConfiguration" />
    public class XmlUrlMappingProvider : IUrlMappingProvider
    {
        private const string kCACHE_KEY = "__XmlUrlMappingProvider_cache_key__";

        private IncomingQueryStringBehaviorEnum _qsBehavior;
        private UrlMappingItemCollection _coll;
        private string _urlMappingFile;
        private bool _useDependency;       
        private CacheDependency _fileDependency;

        // to help with debugging, provide a property that indicates when the urlmapping
        // data was last refreshed
        private DateTime _latestRefresh;

        #region IUrlMappingProvider Members

        /// <summary>
        /// Provides the <see cref="UrlMappingModule" /> with an internally-cached
        /// list of URL templates and redirection mappings processed from items
        /// in an XML file.
        /// </summary>
        /// <returns>The collection of URL redirection mappings</returns>
        UrlMappingItemCollection IUrlMappingProvider.GetUrlMappings()
        {
            // if we aren't using sqlDependencies, then return the collection that 
            // was generated upon initialization
            if (!_useDependency) return _coll;

            // if we are using a dependency, check to see if we have a 
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
            XmlUrlMappingProviderConfiguration xmlConfig = (config as XmlUrlMappingProviderConfiguration);
            if (xmlConfig == null)
                throw new ProviderException("Invalid XmlUrlMappingProvider configuration.  Check the web.config file settings.");

            // remember configuration settings
            _urlMappingFile = xmlConfig.UrlMappingFile;
            _useDependency = xmlConfig.UseDependency;
            _qsBehavior = xmlConfig.IncomingQueryStringBehavior;

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

            
            // parse the given xml file to retrieve the listing of URL items;
            // the xml file should include tags in the form:
            // <urlMapping name="" urlTemplate="" redirection="" />

            XmlDocument xml = new XmlDocument();
            string file = HttpContext.Current.Server.MapPath(_urlMappingFile);
            try
            {
                xml.Load(file);
            }
            catch (Exception ex)
            {
                throw new ProviderException("There is an XmlUrlMappingModule error.  The error occurred while loading the urlMappingFile.  A virtual path is required and the file must be well-formed.", ex);
            }

            // parse the file for <urlMapping> tags
            XmlNodeList nodes = xml.SelectNodes("//urlMapping");
            foreach (XmlNode node in nodes)
            {
                // retrieve name, urlTemplate, and redirection attributes;
                // ensure urlTemplate and redirection are present
                string name = XmlAttributeValue(node, "name");
                string urlTemplate = XmlAttributeValue(node, "urlTemplate");
                string redirection = XmlAttributeValue(node, "redirection");
                bool enabled = XmlAttributeBoolean(node, "enabled", true);

                if (enabled)
                {
                    if (string.IsNullOrEmpty(urlTemplate))
                        throw new ProviderException("There is an XmlUrlMappingModule error.  All <urlMapping> tags in the mapping file require a 'urlTemplate' attribute.");

                    if (string.IsNullOrEmpty(urlTemplate))
                        throw new ProviderException("There is an XmlUrlMappingModule error.  All <urlMapping> tags in the mapping file require a 'redirection' attribute.");

                    // still here, we can create the item and add to the collection
                    UrlMappingItem item
                      = UrlMappingHelper.CreateTemplatedMappingItem(
                            name, urlTemplate, redirection, _qsBehavior
                           );

                    _coll.Add(item);
                }
            }
            


            // if we're using a file dependency, generate it now
            if (_useDependency)
            {
                _fileDependency = new CacheDependency(file);
                HttpContext.Current.Cache.Insert(kCACHE_KEY, "dummyValue", _fileDependency);
            }

            // remember the refresh time
            _latestRefresh = System.DateTime.Now;

        }


        
        /// <summary>
        /// Returns as a string the value of the given attribute within the given XML node.
        /// </summary>
        /// <param name="node">the XML node containing the attribute</param>
        /// <param name="attribute">the name of the attribute</param>
        /// <returns>the attribute value as a string, or the empty string if the attribute doesn't exist</returns>
        protected string XmlAttributeValue(XmlNode node, string attribute)
        {
            // return the given attribute value as a string, or the empty string if nonexistant
            if (node.Attributes[attribute] == null)
                return string.Empty;

            return node.Attributes[attribute].Value;
        }

        /// <summary>
        /// Returns as a Boolean the value of the given attribute within the given XML node.
        /// </summary>
        /// <param name="node">the XML node containing the attribute</param>
        /// <param name="attribute">the name of the attribute</param>
        /// <param name="defaultValue">a Boolean value to assign if the attribute is not present in the given node</param>
        /// <returns>
        /// the attribute value as a Boolean; if the attribute value cannot be converted to
        /// a Boolean, an exception is thrown.
        /// </returns>
        protected bool XmlAttributeBoolean(XmlNode node, string attribute, bool defaultValue)
        {
            // if the attribute is missing, return the default
            if (node.Attributes[attribute] == null)
                return defaultValue;

            return Convert.ToBoolean(node.Attributes[attribute].Value);
        }



    }
}
