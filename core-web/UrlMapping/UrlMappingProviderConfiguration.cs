using System;
using System.Configuration;

namespace lw.UrlMapping
{
    /// <summary>
    /// Provides common configuration options for all concrete providers of URL mapping items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Developers who wish to implement their own UrlMappingProvider should inherit from the
    /// UrlMappingProviderConfiguration class to add additional provider-specific configuration
    /// options (if necessary).
    /// </para>
    /// </remarks>
    /// <seealso cref="IUrlMappingProvider" />
    public class UrlMappingProviderConfiguration : ConfigurationSection
    {

        /// <summary>
        /// Creates a UrlMappingProviderConfiguration object.
        /// </summary>
        public UrlMappingProviderConfiguration()
        {
        }

        /// <summary>
        /// Specifies the class Type of the object that will be used to supply URL mapping items
        /// to the UrlMappingModule.
        /// </summary>
        [ConfigurationProperty("providerType", IsRequired=true)]
        public string ProviderTypeString
        {
            get { return (String)this["providerType"]; }
            set { this["providerType"] = value; }
        }


        /// <summary>
        /// Specifies the action to be taken when the UrlMappingModule finds no available redirection
        /// match for an incoming URL.
        /// </summary>
        [ConfigurationProperty("noMatchAction", DefaultValue = NoMatchActionEnum.PassThrough, IsRequired=false)]
        public NoMatchActionEnum NoMatchAction
        {
            get { return (NoMatchActionEnum)this["noMatchAction"]; }
            set { this["noMatchAction"] = value; }
        }


        /// <summary>
        /// Specifies the redirection URL applied when no redirection match is found for an
        /// incoming URL and the <see cref="NoMatchAction" /> property is set to 
        /// <see cref="NoMatchActionEnum.Redirect" />.
        /// </summary>
        [ConfigurationProperty("noMatchRedirectUrl", IsRequired = false)]
        public string NoMatchRedirectUrl
        {
            get { return (String)this["noMatchRedirectUrl"]; }
            set { this["noMatchRedirectUrl"] = value; }
        }


        /// <summary>
        /// Instructs the UrlMappingModule to automatically adjust the action attributes of
        /// rendered &lt;form&gt; tags to match the incoming URL rather than the redirected one.
        /// </summary>
        /// <remarks>
        /// This property defaults to <b>true</b>.  To disable this behaviour, set 
        /// <b>automaticallyUpdateFormAction=false</b> in the UrlMappingModule Web.config section.
        /// </remarks>
        [ConfigurationProperty("automaticallyUpdateFormAction", DefaultValue = true, IsRequired = false)]
        public bool AutomaticallyUpdateFormAction
        {
            get { return Convert.ToBoolean(this["automaticallyUpdateFormAction"]); }
            set { this["automaticallyUpdateFormAction"] = value; }
        }


        /// <summary>
        /// Determines how the UrlMappingModule will treat incoming querystring values
        /// with regard to pattern matching and redirection.
        /// </summary>
        [ConfigurationProperty("incomingQueryStringBehavior", DefaultValue = IncomingQueryStringBehaviorEnum.PassThrough, IsRequired=false)]
        public IncomingQueryStringBehaviorEnum IncomingQueryStringBehavior
        {
            get { return (IncomingQueryStringBehaviorEnum)this["incomingQueryStringBehavior"]; }
            set { this["incomingQueryStringBehavior"] = value; }
        }


        /// <summary>
        /// A list of path extensions that the UrlMappingModule will ignore.
        /// </summary>
        /// <remarks>
        /// Set this attribute to a space-separated list of file extensions that should be
        /// ignored by the UrlMappingModule and thus will be handled normally by ASP.NET
        /// with no redirection.
        /// By default, this list is blank (and all extensions are processed).  
        /// </remarks>
        /// <example>
        /// The following shows an example of setting the <c>ignoreExtensions</c>
        /// attribute on the &lt;urlMappingModule&gt; tag in the <i>Web.config</i>
        /// file such that requests for javascript (.js) and CSS files (.css) 
        /// are not processed by the module:
        /// <code>
        /// &lt;urlMappingModule 
        ///      ...
        ///      ignoreExtensions=".js .css"
        ///      ...
        ///      /&gt;
        /// </code>
        /// </example>
        [ConfigurationProperty("ignoreExtensions", DefaultValue = "", IsRequired = false)]
        public string IgnoreExtensions
        {
            get { return (string)this["ignoreExtensions"]; }
            set { this["ignoreExtensions"] = value; }
        }



        /// <summary>
        /// Specifies at which point in the request cycle the UrlMappingModule
        /// will process incoming URLs.
        /// </summary>
        /// <remarks>
        /// By default, the UrlMappingModule processes incoming URLs in the application's
        /// <c>BeginRequest</c> event.  The developer may wish to change this if using
        /// Forms or Windows authentication.
        /// </remarks>
        /// <seealso cref="UrlProcessingEventEnum" />
        [ConfigurationProperty("urlProcessingEvent", DefaultValue=UrlProcessingEventEnum.BeginRequest, IsRequired = false)]
        public UrlProcessingEventEnum UrlProcessingEvent
        {
            get { return (UrlProcessingEventEnum)this["urlProcessingEvent"]; }
            set { this["urlProcessingEvent"] = value; }
        }



        /// <summary>
        /// Determines whether an explicit authorization check for the redirected URL
        /// will occur.
        /// </summary>
        /// <remarks>
        /// By default, this value is <c>false</c>.  Setting it to <c>true</c> can be
        /// useful in applications that use Forms Authentication to enable &lt;authorization&gt;
        /// configuration to be based on the redirected URL in addition to the incoming URL.       
        /// Note that to perform this authorization congruent to ASP.NET URL authorization, the UrlMappingModule uses the
        /// <see cref="System.Web.Security.UrlAuthorizationModule.CheckUrlAccessForPrincipal">CheckUrlAccessForPrincipal()</see>
        /// method of the <see cref="System.Web.Security.UrlAuthorizationModule">UrlAuthorizationModule</see> class.
        /// </remarks>
        [ConfigurationProperty("authorizeRedirectionUrl", DefaultValue = false, IsRequired = false)]
        public bool AuthorizeRedirectionUrl
        {
            get { return (bool)this["authorizeRedirectionUrl"]; }
            set { this["authorizeRedirectionUrl"] = value; }
        }



        /// <summary>
        /// Used when <see cref="AuthorizeRedirectionUrl" /> is <c>true</c> to
        /// specify the redirection URL to use when a matched redirection is not
        /// authorized for the currently logged in user.
        /// </summary>
        /// <remarks>
        /// When using Forms Authentication, setting <see cref="AuthorizeRedirectionUrl" />
        /// to <c>true</c> and setting AuthorizeFailureRedirectUrl as your application's 
        /// login page enables &lt;authorization&gt;
        /// configuration to be based on the redirected URL rather than the incoming URL,
        /// and the behavior when unauthorized to mimic normal Forms authentication behavior.
        /// If desired, the AuthorizeFailureRedirectUrl may be set to a different
        /// page entirely.
        /// </remarks>
        [ConfigurationProperty("authorizeFailureRedirectUrl", DefaultValue = "", IsRequired = false)]
        public string AuthorizeFailureRedirectUrl
        {
            get { return (string)this["authorizeFailureRedirectUrl"]; }
            set { this["authorizeFailureRedirectUrl"] = value; }
        }


    }
}
