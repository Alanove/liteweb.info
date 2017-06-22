using System;

namespace lw.UrlMapping
{
    /// <summary> 
    /// Specifies the methods a URL mapping provider object must implement to work with
    /// the UrlMappingModule.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="UrlMappingModule">UrlMappingModule</see> receives its list
    /// of URL targets and corresponding redirections from a configured provider.  The
    /// provider must implement the IUrlMappingProvider interface.
    /// </para>
    /// <para>
    /// Concrete implementations of this interface will typically cache a collection of
    /// <see cref="UrlMappingItem" /> objects in the <see cref="Initialize">Initialize</see> method
    /// and provide those to the UrlMappingModule through the <see cref="GetUrlMappings" /> 
    /// method.
    /// </para>
    /// <para>
    /// The UrlMappingModule assembly includes two concrete providers:  
    /// <see cref="XmlUrlMappingProvider" />, for use when storing mapping information in an 
    /// XML file, and <see cref="SqlUrlMappingProvider" />, for use when storing mapping information
    /// in a SQL Server database table.  Developers may use these providers as they are,
    /// create custom providers in a seperate assembly, or include a code file defining a provider 
    /// class in the APP_CODE directory of the web project.  In the latter case, the
    /// <see cref="System.Web.Compilation.BuildManager">BuildManager</see> class is used to 
    /// retrieve the compiled class from the APP_CODE file, which requires the appropriate hosting
    /// permission.  If the hosting permission is not available for the given server, a
    /// compiled assembly or one of the built-in providers must be used instead.
    /// </para>
    /// <para>
    /// Configuration for a provider is closely tied to the provider itself.  The base class
    /// <see cref="UrlMappingProviderConfiguration"/> defines configuration properties used by 
    /// all UrlMappingModule providers (including the <see cref="UrlMappingProviderConfiguration.ProviderTypeString">ProviderTypeString</see>
    /// property which specifies the provider used).  Developers of custom providers may
    /// choose to subclass UrlMappingProviderConfiguration to add additional configuration
    /// properties relevent to the provider.  The XML and SQL providers packaged with the 
    /// UrlMappingModule for example define additional properties in their own
    /// configuration subclasses.
    /// </para>
    /// <para>
    /// In the Web.config file, specify the provider for the UrlMappingModule by first defining
    /// a configuration section appropriate to the provider.  The following example shows this
    /// Web.config entry for the XmlUrlMappingProvider:
    /// </para>
    /// <code>
    ///&lt;configuration&gt;
    ///  &lt;configSections&gt;
    ///    &lt;section name="urlMappingModule" 
    ///      type="UNLV.IAP.UrlMapping.XmlUrlMappingProviderConfiguration, UrlMappingModule"/&gt;
    ///    &lt;/configSections&gt;
    ///&lt;/configuration&gt;
    /// </code>
    /// Then in the <c>urlMappingModule</c> configuration section, use the <c>providerType</c>
    /// attribute to specify the type of the IUrlMappingProvider object.
    /// <code>
    ///&lt;configuration&gt;
    ///  &lt;configSections&gt;
    ///    &lt;section name="urlMappingModule" 
    ///      type="UNLV.IAP.UrlMapping.XmlUrlMappingProviderConfiguration, UrlMappingModule"/&gt;
    ///    &lt;/configSections&gt;
    ///
    /// &lt;urlMappingModule 
    ///      providerType="UNLV.IAP.UrlMapping.XmlUrlMappingProvider, UrlMappingModule"
    ///      noMatchAction="Redirect" 
    ///      noMatchRedirectUrl="NotFound.aspx" 
    ///      incomingQueryStringBehavior="Include" 
    ///      useDependency="true"
    ///      urlMappingFile="~/App_Data/urlMappings.xml" 
    ///      /&gt;
    ///&lt;/configuration&gt;
    /// </code>
    /// </remarks>
    public interface IUrlMappingProvider
    {
        /// <summary>
        /// Provides the <see cref="UrlMappingModule">UrlMappingModule</see> with a collection of items defining how
        /// incoming URLs are to be redirected.
        /// </summary>
        /// <returns>The collection of UrlMappingItems</returns>
        /// <remarks>
        /// Typically the concrete provider will internally cache the collection of URL mappings
        /// in the 
        /// <see cref="IUrlMappingProvider.Initialize">Initialize</see>
        /// method and provide the UrlMappingModule with those mappings 
        /// through the GetUrlMappings method.
        /// </remarks>
        UrlMappingItemCollection GetUrlMappings();

        /// <summary>
        /// Instructs the provider to refresh its internally-cached collection of 
        /// UrlMappingItems
        /// </summary>
        /// <remarks>
        /// This method allows developers who do not wish to use caching dependencies
        /// the ability to manually direct the provider to update its internal cache,
        /// providing the UrlMappingModule with the most current URL mappings.
        /// </remarks>
        void RefreshUrlMappings();


        /// <summary>
        /// Returns the date and time the provider most recently refreshed its internally-cached
        /// collection of UrlMappingItems
        /// </summary>
        /// <returns>the date and time of the most recent refresh</returns>
        DateTime GetLastRefreshTime();


        /// <summary>
        /// Initializes a provider that is supplying the UrlMappingModule with definitions 
        /// for how incoming URLs are to be redirected.
        /// </summary>
        /// <param name="config">The configuration object supplied from the UrlMappingModule</param>
        /// <remarks>
        /// The UrlMappingModule parses configuration items in the Web.config file and passes
        /// a resulting configuration object to the URL mapping provider.  The object will be
        /// of the type <i>UrlMapingProviderConfiguration</i> or 
        /// a provider-specific subclass.  Implementations should initialize URL mappings given
        /// this configuration object.
        /// </remarks>
        void Initialize(UrlMappingProviderConfiguration config);

        /// <summary>
        /// Provides a means for the provider to release any resources allocated 
        /// in the Initialize method.
        /// </summary>
        void Dispose();



        
    }

}
