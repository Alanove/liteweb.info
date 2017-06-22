using System.Configuration;

namespace lw.UrlMapping
{
    /// <summary>
    /// The configuration object used when the <see cref="UrlMappingModule" />
    /// is configured to receive mapping items from the <see cref="XmlUrlMappingProvider" />.
    /// </summary>
    /// <remarks>
    /// Configuration for the XmlUrlMappingProvider is established in the Web.config file
    /// through a custom &lt;configSections&gt; entry, as so:
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
    /// In the above example, the custom configuration section is named "urlMappingModule"
    /// and the corresponding <c>&lt;urlMappingModule&gt;</c> contains the following attributes
    /// for configuring the provider:
    /// <para>
    /// &#8226; <c>providerType</c> identifies the class and assembly for the provider object, and
    /// should be set to "UNLV.IAP.UrlMapping.XmlUrlMappingProvider, UrlMappingModule".
    /// </para>
    /// <para>
    /// &#8226; <c>noMatchAction</c> specifies the UrlMappingModule's behavior in the event an incoming
    /// URL cannot be matched to any of the mapping items listed in the XML mapping file.
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
    /// &#8226; <c>urlMappingFile</c> is a string that specifies an application-relative path to the
    /// XML file containing URL redirection mapping entries.  See the <see cref="XmlUrlMappingProvider" />
    /// documentation for the syntax and an example of this file.
    /// </para>
    /// <para>
    /// &#8226; <c>useDependency</c> is a boolean that determines whether the internally-cached mapping
    /// items should be updated automatically as changes are made in the <c>urlMappingFile</c>.
    /// If set to <c>true</c>, a <see cref="System.Web.Caching.CacheDependency">CacheDependency</see>
    /// object is used internally to poll the <c>urlMappingFile</c> for changes.  If false,
    /// changes made to the <c>urlMappingFile</c> are only incorporated upon the next application
    /// startup.
    /// </para>
    /// </remarks>
    public class XmlUrlMappingProviderConfiguration : UrlMappingProviderConfiguration
    {

        /// <summary>
        /// Specifies an application-relative path to the XML file containing
        /// URL templates and redirection mappings.
        /// </summary>
        [ConfigurationProperty("urlMappingFile", IsRequired=true)]
        public string UrlMappingFile
        {
            get { return (string)this["urlMappingFile"]; }
            set { this["urlMappingFile"] = value; }
        }


        /// <summary>
        /// Specifies whether the XML file containing URL templates and redirection mappings
        /// is polled for changes.
        /// If true, the internal cache of mapping items is refreshed when updates
        /// to the XML file are made.
        /// </summary>
        /// <remarks>
        /// If <c>UseDependency</c> is set to <c>true</c>, a
        /// <see cref="System.Web.Caching.CacheDependency">CacheDependency</see> is used to 
        /// poll for changes in the file specified by the 
        /// <see cref="UrlMappingFile" /> property.
        /// </remarks>
        [ConfigurationProperty("useDependency", IsRequired=false, DefaultValue=false)]
        public bool UseDependency
        {
            get { return (bool)this["useDependency"]; }
            set { this["useDependency"] = value; }
        }


    }
}
