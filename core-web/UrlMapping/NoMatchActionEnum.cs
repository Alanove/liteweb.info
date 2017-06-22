namespace lw.UrlMapping
{
    /// <summary>
    /// Specifies the types of actions that may be taken by the UrlMappingModule
    /// when it fails to find a match for redirecting an incoming URL.
    /// </summary>
    public enum NoMatchActionEnum
    {
        /// <summary>
        /// Allow an unmatched URL to continue to be processed normally by the ASP.NET engine.
        /// </summary>
        PassThrough,

        /// <summary>
        /// Throw a <see cref="NoMatchFoundException" /> when presented with an 
        /// unmatched URL.
        /// </summary>
        ThrowException,

        /// <summary>
        /// Return a 404 (page not found) error code to the browser when presented with 
        /// an unmatched URL.
        /// </summary>
        Return404,

        /// <summary>
        /// Redirect to the page specified in the configuration's 'NoMatchRedirectionUrl' 
        /// attribute when presented with an unmatched URL.
        /// </summary>
        Redirect
    }
}