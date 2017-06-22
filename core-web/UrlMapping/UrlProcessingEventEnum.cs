namespace lw.UrlMapping
{

    /// <summary>
    /// Determines when in the request cycle an incoming URL is processed by the 
    /// UrlMappingModule
    /// </summary>
    /// <remarks>
    /// When configuring the UrlMappingModule in the <i>Web.config</i> 
    /// <c>&lt;urlMappingModule&gt;</c>
    /// section, set the attribute <c>urlProcessingEvent</c> to any of the UrlProcessingEventEnum
    /// values to specify at which point in the request cycle incoming URLs should be 
    /// processed.  By default, incoming urls are processed in the BeginRequest event,
    /// but developers may wish to specify <c>AuthorizeRequest</c> if using Forms authentication,
    /// or <c>AuthenticateRequest</c> if using Windows authentication.
    /// </remarks>
    public enum UrlProcessingEventEnum
    {
        /// <summary>
        /// Process incoming URLs in the application's 
        /// <see cref="System.Web.HttpApplication.BeginRequest">BeginRequest</see>
        /// event
        /// </summary>
        BeginRequest,

        /// <summary>
        /// Process incoming URLs in the application's 
        /// <see cref="System.Web.HttpApplication.AuthenticateRequest">AuthenticateRequest</see>
        /// event
        /// </summary>
        AuthenticateRequest,

        /// <summary>
        /// Process incoming URLs in the application's 
        /// <see cref="System.Web.HttpApplication.AuthorizeRequest">AuthorizeRequest</see>
        /// event
        /// </summary>
        AuthorizeRequest
    }
}