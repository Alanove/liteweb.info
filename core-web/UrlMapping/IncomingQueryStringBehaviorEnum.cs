namespace lw.UrlMapping
{
    /// <summary>
    /// Specifies the behavior the UrlMappingModule should follow when incoming URLs have 
    /// query string values associated with them.
    /// </summary>
    /// <remarks>
    /// <para>
    /// To further illustrate how incoming querystring values are treated, consider the
    /// following examples.  
    /// </para>
    /// <para>
    /// If <b>PassThrough</b> is specified, an incoming querystring of "Default.aspx?cat=1" will be treated
    /// as "Default.aspx" for the sake of pattern matching.  If a match is found,
    /// "cat=1" will be appended to the redirection URL as part of the redirected
    /// querystring.
    /// </para>
    /// <para>
    /// If <b>Ignore</b> is specified, an incoming querystring of "Default.aspx?cat=1" will be treated
    /// as "Default.aspx" for the sake of pattern matching.  If a match is found,
    /// "cat=1" will <i>not</i> be appended to the redirection URL.
    /// </para>
    /// <para>
    /// If <b>Include</b> is specified, an incoming querystring of "Default.aspx?cat=1" will be treated
    /// literally as "Default.aspx?cat=1" for the sake of pattern matching.  
    /// If that url is compared to a url template of "Default.aspx", the match is
    /// not made - the url template would need to incorporate
    /// potential querystring input.  For example, a url template may capture a specific querystring
    /// value using a template such as "Default.aspx?cat=[cat]".  In this case, the
    /// redirected URL will append "cat=<i>xxx</i>" to the querystring.
    /// </para>
    /// </remarks>
    public enum IncomingQueryStringBehaviorEnum
    {
        /// <summary>
        /// Ignore incoming querystring values when pattern matching a URL, but
        /// apply those values to the redirected URL.
        /// </summary>
        PassThrough,

        /// <summary>
        /// Ignore incoming querystring values when pattern matching a URL, and
        /// do not apply those values to the redirected URL.
        /// </summary>
        Ignore,

        /// <summary>
        /// Include the querystring on an incoming URL when pattern matching.  
        /// </summary>
        /// <remarks>
        /// </remarks>
        Include

        
    }
}